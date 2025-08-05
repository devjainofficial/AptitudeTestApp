using AptitudeTestApp.Data;
using AptitudeTestApp.Shared.Enums;
using global::AptitudeTestApp.Data.Models;
using global::AptitudeTestApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTestApp.Application.Services;

public class DataSeedingService(
    ApplicationDbContext context,
    RoleManager<IdentityRole> roleManager,
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration)
{
    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedSuperAdminAsync();

        if (!await context.QuestionCategories.AnyAsync())
        {
            await SeedCategoriesAsync();
        }

        if (!await context.Universities.AnyAsync())
        {
            await SeedUniversitiesAsync();
        }

        if (!await context.Questions.AnyAsync())
        {
            await SeedQuestionsAsync();
        }
    }

    private async Task SeedRolesAsync()
    {
        string[] roleNames = { "SuperAdmin", "Admin", "Student" };

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    private async Task SeedSuperAdminAsync()
    {
        var email = configuration["SuperAdmin:Email"];
        var password = configuration["SuperAdmin:Password"];

        var existingUser = await userManager.FindByEmailAsync(email ?? string.Empty);
        if (existingUser != null)
            return;

        var superAdmin = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(superAdmin, password ?? string.Empty);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
        }
    }

    private async Task SeedCategoriesAsync()
    {
        var categories = new[]
        {
            new QuestionCategory { Name = "Mathematics", Description = "Basic mathematical concepts and calculations" },
            new QuestionCategory { Name = "Logical Reasoning", Description = "Logic puzzles and reasoning questions" },
            new QuestionCategory { Name = "English", Description = "Grammar, vocabulary, and comprehension" },
            new QuestionCategory { Name = "General Knowledge", Description = "Current affairs and general awareness" },
            new QuestionCategory { Name = "Computer Science", Description = "Basic programming and computer concepts" }
        };

        context.QuestionCategories.AddRange(categories);
        await context.SaveChangesAsync();
    }

    private async Task SeedUniversitiesAsync()
    {
        var universities = new[]
        {
            new University { Name = "Sample University", Code = "SAMPLE_UNIV", ContactEmail = "admin@sample.edu", IsActive = true },
            new University { Name = "Test Institute", Code = "TEST_INST", ContactEmail = "contact@test.edu", IsActive = true }
        };

        context.Universities.AddRange(universities);
        await context.SaveChangesAsync();
    }

    private async Task SeedQuestionsAsync()
    {
        var mathCategory = await context.QuestionCategories.FirstAsync(c => c.Name == "Mathematics");
        var reasoningCategory = await context.QuestionCategories.FirstAsync(c => c.Name == "Logical Reasoning");

        var questions = new[]
        {
            new Question
            {
                CategoryId = mathCategory.Id,
                QuestionText = "What is 15 + 25?",
                DifficultyLevel = QuestionDifficulty.Easy,
                Points = 1,
                Options = new List<QuestionOption>
                {
                    new QuestionOption { OptionText = "30", IsCorrect = false, DisplayOrder = 1 },
                    new QuestionOption { OptionText = "35", IsCorrect = false, DisplayOrder = 2 },
                    new QuestionOption { OptionText = "40", IsCorrect = true, DisplayOrder = 3 },
                    new QuestionOption { OptionText = "45", IsCorrect = false, DisplayOrder = 4 }
                }
            },
            new Question
            {
                CategoryId = reasoningCategory.Id,
                QuestionText = "If all roses are flowers and some flowers are red, which statement is necessarily true?",
                DifficultyLevel = QuestionDifficulty.Medium,
                Points = 2,
                Options = new List<QuestionOption>
                {
                    new QuestionOption { OptionText = "All roses are red", IsCorrect = false, DisplayOrder = 1 },
                    new QuestionOption { OptionText = "Some roses are red", IsCorrect = false, DisplayOrder = 2 },
                    new QuestionOption { OptionText = "Some roses are flowers", IsCorrect = true, DisplayOrder = 3 },
                    new QuestionOption { OptionText = "No roses are red", IsCorrect = false, DisplayOrder = 4 }
                }
            }
        };

        context.Questions.AddRange(questions);
        await context.SaveChangesAsync();
    }
}
