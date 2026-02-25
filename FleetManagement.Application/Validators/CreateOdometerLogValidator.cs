using FleetManagement.Application.DTOs;
using FluentValidation;

namespace FleetManagement.Application.Validators;

public class CreateOdometerLogValidator : AbstractValidator<CreateOdometerLogDto>
{
    public CreateOdometerLogValidator()
    {
        RuleFor(x => x.VehicleId)
            .GreaterThan(0);

        RuleFor(x => x.OdometerKm)
            .GreaterThan(0).WithMessage("Odometer reading must be greater than 0.");

        RuleFor(x => x.LogDate)
            .NotEmpty()
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Log date cannot be in the future.");

        RuleFor(x => x.Notes)
            .MaximumLength(500).When(x => x.Notes != null);
    }
}