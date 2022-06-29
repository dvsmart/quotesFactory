using FluentValidation;
using Quotes.Reader.Client.Models;

namespace Quotes.Reader.Client.Validators
{
    /// <summary>
    /// Validates quotes reader request.
    /// </summary>
    public class QuotesGroupReaderRequestValidator : AbstractValidator<QuotesGroupReaderRequest>
    {
        public QuotesGroupReaderRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(ss => ss.StorageDirectory)
                .Must(NotBeNullOrEmpty)
                .WithMessage("Storage directory is required");

            RuleFor(ss => ss.OutputDirectory)
                .Must(NotBeNullOrEmpty)
                .WithMessage("Output directory is required");

            RuleFor(ss => ss.GroupName)
                .Must(NotBeNullOrEmpty)
                .WithMessage("Group name is required");

            RuleFor(ss => ss.AsOfDateString)
                .Must(NotBeNullOrEmpty)
                .WithMessage("Date is required");

            RuleFor(ss => ss.AsOfDateString)
                .Must(InvalidDate)
                .WithMessage("Invalid date");

        }

        private bool NotBeNullOrEmpty(string? propertyValue)
        {
            return !string.IsNullOrWhiteSpace(propertyValue);
        }

        private bool InvalidDate(string data)
        {
            return data != null && DateTime.TryParse(data, out DateTime asOfDate);
        }
    }
}
