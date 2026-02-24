using FleetManagement.Application.DTOs;
using FluentValidation;

public class CreateDriverValidator : AbstractValidator<CreateDriverDto>
{
    public CreateDriverValidator()
    {
        RuleFor(x => x.EmployeeId).GreaterThan(0);
        RuleFor(x => x.LicenseNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LicenseExpiry)
            .Must(date => date > DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("License expiry must be a future date.");
        RuleFor(x => x.LicenseCategoryIds)
            .NotEmpty().WithMessage("At least one license category is required.");
    }
}