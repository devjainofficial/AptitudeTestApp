class AntiCheatSystem {
    constructor() {
        this.isInitialized = false;
        this.submissionId = null;
        this.tabSwitchCount = 0;
        this.maxTabSwitches = 3;
        this.isTestActive = false;

        // Bind methods to maintain context
        this.handleVisibilityChange = this.handleVisibilityChange.bind(this);
        this.handleBeforeUnload = this.handleBeforeUnload.bind(this);
        this.handleKeyDown = this.handleKeyDown.bind(this);
        this.handleRightClick = this.handleRightClick.bind(this);
        this.handleCopy = this.handleCopy.bind(this);
        this.handlePaste = this.handlePaste.bind(this);
        this.handleResize = this.handleResize.bind(this);
        this.handleFullscreenChange = this.handleFullscreenChange.bind(this);
    }

    async initialize(submissionId, maxTabSwitches = 3) {
        if (this.isInitialized) return;

        this.submissionId = submissionId;
        this.maxTabSwitches = maxTabSwitches;
        this.isTestActive = true;

        try {
            // Do not call enterFullscreen() here

            this.addEventListeners();
            this.isInitialized = true;

        } catch (error) {
            console.error('Failed to initialize anti-cheat system:', error);
            await this.logEvent('InitializationError', { error: error.message });
        }
    }

    addEventListeners() {
        // Tab switch detection
        document.addEventListener('visibilitychange', this.handleVisibilityChange);
        window.addEventListener('blur', this.handleVisibilityChange);
        window.addEventListener('focus', this.handleVisibilityChange);

        // Prevent page unload
        window.addEventListener('beforeunload', this.handleBeforeUnload);

        // Keyboard shortcuts prevention
        document.addEventListener('keydown', this.handleKeyDown);

        // Right-click prevention
        document.addEventListener('contextmenu', this.handleRightClick);

        // Copy/Paste prevention
        document.addEventListener('copy', this.handleCopy);
        document.addEventListener('paste', this.handlePaste);
        document.addEventListener('cut', this.handleCopy);

        // Window resize detection
        window.addEventListener('resize', this.handleResize);

        // Fullscreen change detection
        document.addEventListener('fullscreenchange', this.handleFullscreenChange);
        document.addEventListener('webkitfullscreenchange', this.handleFullscreenChange);
        document.addEventListener('mozfullscreenchange', this.handleFullscreenChange);
        document.addEventListener('MSFullscreenChange', this.handleFullscreenChange);
    }

    removeEventListeners() {
        document.removeEventListener('visibilitychange', this.handleVisibilityChange);
        window.removeEventListener('blur', this.handleVisibilityChange);
        window.removeEventListener('focus', this.handleVisibilityChange);
        window.removeEventListener('beforeunload', this.handleBeforeUnload);
        document.removeEventListener('keydown', this.handleKeyDown);
        document.removeEventListener('contextmenu', this.handleRightClick);
        document.removeEventListener('copy', this.handleCopy);
        document.removeEventListener('paste', this.handlePaste);
        document.removeEventListener('cut', this.handleCopy);
        window.removeEventListener('resize', this.handleResize);
        document.removeEventListener('fullscreenchange', this.handleFullscreenChange);
        document.removeEventListener('webkitfullscreenchange', this.handleFullscreenChange);
        document.removeEventListener('mozfullscreenchange', this.handleFullscreenChange);
        document.removeEventListener('MSFullscreenChange', this.handleFullscreenChange);
    }

    async handleVisibilityChange() {
        if (!this.isTestActive) return;

        if (document.hidden || document.visibilityState === 'hidden') {
            this.tabSwitchCount++;

            await this.logEvent('TabSwitch', {
                count: this.tabSwitchCount,
                timestamp: new Date().toISOString(),
                visibilityState: document.visibilityState
            });

            if (this.tabSwitchCount >= this.maxTabSwitches) {
                await this.autoSubmitTest('Excessive tab switching detected');
            } else {
                // Show warning
                this.showWarning(`Warning: Tab switching detected! (${this.tabSwitchCount}/${this.maxTabSwitches})`);
            }
        }
    }

    handleBeforeUnload(event) {
        if (!this.isTestActive) return;

        event.preventDefault();
        event.returnValue = 'Are you sure you want to leave? Your test progress may be lost.';
        return event.returnValue;
    }

    async handleKeyDown(event) {
        if (!this.isTestActive) return;

        // Prevent F12, Ctrl+Shift+I, Ctrl+Shift+J, Ctrl+U
        const forbiddenKeys = [
            'F12',
            { key: 'I', ctrl: true, shift: true },
            { key: 'J', ctrl: true, shift: true },
            { key: 'U', ctrl: true },
            { key: 'S', ctrl: true }, // Ctrl+S (Save)
            { key: 'P', ctrl: true }, // Ctrl+P (Print)
            { key: 'A', ctrl: true }, // Ctrl+A (Select All)
        ];

        const isForbidden = forbiddenKeys.some(forbidden => {
            if (typeof forbidden === 'string') {
                return event.key === forbidden;
            } else {
                return event.key.toLowerCase() === forbidden.key.toLowerCase() &&
                    event.ctrlKey === forbidden.ctrl &&
                    (!forbidden.shift || event.shiftKey === forbidden.shift);
            }
        });

        if (isForbidden) {
            event.preventDefault();
            event.stopPropagation();

            await this.logEvent('ForbiddenKeyPressed', {
                key: event.key,
                ctrlKey: event.ctrlKey,
                shiftKey: event.shiftKey,
                altKey: event.altKey
            });

            this.showWarning('This action is not allowed during the test!');
            return false;
        }
    }

    async handleRightClick(event) {
        if (!this.isTestActive) return;

        event.preventDefault();

        await this.logEvent('RightClickAttempt', {
            x: event.clientX,
            y: event.clientY,
            target: event.target.tagName
        });

        this.showWarning('Right-click is disabled during the test!');
        return false;
    }

    async handleCopy(event) {
        if (!this.isTestActive) return;

        event.preventDefault();

        await this.logEvent('CopyAttempt', {
            type: event.type,
            selection: window.getSelection().toString().substring(0, 100)
        });

        this.showWarning('Copy/Cut operations are not allowed during the test!');
        return false;
    }

    async handlePaste(event) {
        if (!this.isTestActive) return;

        event.preventDefault();

        await this.logEvent('PasteAttempt', {
            target: event.target.tagName
        });

        this.showWarning('Paste operation is not allowed during the test!');
        return false;
    }

    async handleResize() {
        if (!this.isTestActive) return;

        await this.logEvent('WindowResize', {
            newSize: `${window.innerWidth}x${window.innerHeight}`,
            timestamp: new Date().toISOString()
        });
    }

    async handleFullscreenChange() {
        if (!this.isTestActive) return;

        const isFullscreen = !!(document.fullscreenElement ||
            document.webkitFullscreenElement ||
            document.mozFullScreenElement ||
            document.msFullscreenElement);

        if (!isFullscreen) {
            await this.logEvent('FullscreenExit', {
                timestamp: new Date().toISOString()
            });

            this.showWarning('Please return to fullscreen mode!');

            // Try to re-enter fullscreen
            setTimeout(() => {
                this.enterFullscreen();
            }, 1000);
        }
    }

    async enterFullscreen() {
        const elem = document.documentElement;

        try {
            if (elem.requestFullscreen) {
                await elem.requestFullscreen();
            } else if (elem.webkitRequestFullscreen) {
                await elem.webkitRequestFullscreen();
            } else if (elem.mozRequestFullScreen) {
                await elem.mozRequestFullScreen();
            } else if (elem.msRequestFullscreen) {
                await elem.msRequestFullscreen();
            }

            // Check after request
            const isFullscreen = !!(document.fullscreenElement ||
                document.webkitFullscreenElement ||
                document.mozFullScreenElement ||
                document.msFullscreenElement);

            if (!isFullscreen) {
                this.showWarning('Failed to enter fullscreen mode. Please allow fullscreen access.');
            }

        } catch (error) {
            console.warn('Could not enter fullscreen:', error);
            await this.logEvent('FullscreenError', { error: error.message });
            this.showWarning('Fullscreen request failed. Please enable fullscreen access.');
        }
    }


    exitFullscreen() {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.webkitExitFullscreen) {
            document.webkitExitFullscreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.msExitFullscreen) {
            document.msExitFullscreen();
        }
    }

    async logEvent(eventType, eventDetails) {
        try {
            await DotNet.invokeMethodAsync('AptitudeTestApp', 'LogAntiCheatEvent',
                this.submissionId, eventType, JSON.stringify(eventDetails));
        } catch (error) {
            console.error('Failed to log anti-cheat event:', error);
        }
    }

    async autoSubmitTest(reason) {
        if (!this.isTestActive) return;

        this.isTestActive = false;

        try {
            await DotNet.invokeMethodAsync('AptitudeTestApp', 'AutoSubmitTest',
                this.submissionId, reason);

            // Show final message
            alert(`Test automatically submitted: ${reason}`);

            // Redirect to completion page
            window.location.href = `/test/completed/${this.submissionId}`;

        } catch (error) {
            console.error('Failed to auto-submit test:', error);
            alert('Test submission failed. Please contact administrator.');
        }
    }

    showWarning(message) {
        // Remove existing warning
        const existingWarning = document.getElementById('anti-cheat-warning');
        if (existingWarning) {
            existingWarning.remove();
        }

        // Create warning element
        const warning = document.createElement('div');
        warning.id = 'anti-cheat-warning';
        warning.style.cssText = `
            position: fixed;
            top: 20px;
            left: 50%;
            transform: translateX(-50%);
            background: #ff6b6b;
            color: white;
            padding: 15px 25px;
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.3);
            z-index: 10000;
            font-family: Arial, sans-serif;
            font-size: 14px;
            font-weight: bold;
            animation: slideDown 0.3s ease-out;
        `;

        warning.textContent = message;
        document.body.appendChild(warning);

        // Auto-remove after 5 seconds
        setTimeout(() => {
            if (warning.parentNode) {
                warning.remove();
            }
        }, 5000);
    }

    cleanup() {
        this.isTestActive = false;
        this.removeEventListeners();
        this.exitFullscreen();

        // Remove any warnings
        const warning = document.getElementById('anti-cheat-warning');
        if (warning) {
            warning.remove();
        }
    }
}

// Global instance
window.antiCheatSystem = new AntiCheatSystem();

// CSS for warning animation
const style = document.createElement('style');
style.textContent = `
    @keyframes slideDown {
        from {
            opacity: 0;
            transform: translateX(-50%) translateY(-20px);
        }
        to {
            opacity: 1;
            transform: translateX(-50%) translateY(0);
        }
    }
`;
document.head.appendChild(style);

window.startAntiCheatSystem = async function (submissionId, maxTabSwitches) {
    try {
        // Step 1: Enter fullscreen - must be user initiated!
        const isFullscreen = !!(document.fullscreenElement ||
            document.webkitFullscreenElement ||
            document.mozFullScreenElement ||
            document.msFullscreenElement);

        if (!isFullscreen) {
            await window.antiCheatSystem.enterFullscreen();
        }

        // Step 2: Initialize anti-cheat after fullscreen is granted
        await window.antiCheatSystem.initialize(submissionId, maxTabSwitches);
    } catch (error) {
        console.error('Failed to start anti-cheat system:', error);
    }
};