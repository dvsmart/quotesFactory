using FluentValidation;
using Quotes.Import.Client.Models;

namespace Quotes.Import.Client.Validators
{
    /// <summary>
    /// Validates quotes reader request.
    /// </summary>
    public class QuotesImportRequestValidator : AbstractValidator<QuotesImportRequest>
    {
        public QuotesImportRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(ss => ss.StorageDirectory)
                .Must(NotBeNullOrEmpty)
                .WithMessage("Storage directory is required");

            RuleFor(ss => ss.InputDirectory)
                .Must(NotBeNullOrEmpty)
                .WithMessage("Input directory is required")
                .Must(BeValidDirectory)
                .WithMessage("Invalid Input directory");
        }

        private bool NotBeNullOrEmpty(string? propertyValue)
        {
            return !string.IsNullOrWhiteSpace(propertyValue);
        }

        private bool BeValidDirectory(string data)
        {
            return data != null && Directory.Exists(@$"{data}");
        }
    }
}
