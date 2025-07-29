window.copyText = async function (link) {
    if (navigator.clipboard && window.isSecureContext) {
        try {
            await navigator.clipboard.writeText(link);
        } catch (err) {
            console.error("Clipboard writeText failed:", err);
            fallbackCopyText(link);
        }
    } else {
        fallbackCopyText(link);
    }

    function fallbackCopyText(text) {
        const textArea = document.createElement("textarea");
        textArea.value = text;
        textArea.style.position = "fixed";  // prevent scrolling
        textArea.style.left = "-9999px";
        document.body.appendChild(textArea);
        textArea.focus();
        textArea.select();

        try {
            document.execCommand('copy');
        } catch (err) {
            console.error("Fallback copy failed", err);
        }

        document.body.removeChild(textArea);
    }
};
