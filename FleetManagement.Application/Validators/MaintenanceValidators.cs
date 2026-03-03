using FleetManagement.Application.DTOs;
using FluentValidation;

namespace FleetManagement.Application.Validators;

public class CreateVendorValidator : AbstractValidator<CreateVendorDto>
{
    public CreateVendorValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).EmailAddress().When(x => x.Email != null);
        RuleFor(x => x.Phone).MaximumLength(30).When(x => x.Phone != null);
    }
}

public class CreateMaintenanceOrderValidator : AbstractValidator<CreateMaintenanceOrderDto>
{
    public CreateMaintenanceOrderValidator()
    {
        RuleFor(x => x.VehicleId).GreaterThan(0);
        RuleFor(x => x.VendorId).GreaterThan(0);
        RuleFor(x => x.OdometerKm).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ScheduledAt)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Scheduled date must be in the future.");
        RuleFor(x => x.Description).NotEmpty();
    }
}

public class CreateMaintenanceItemValidator : AbstractValidator<CreateMaintenanceItemDto>
{
    public CreateMaintenanceItemValidator()
    {
        RuleFor(x => x.MaintenanceTypeId).GreaterThan(0);
        RuleFor(x => x.PartsCost).GreaterThanOrEqualTo(0);
        RuleFor(x => x.LaborCost).GreaterThanOrEqualTo(0);
        RuleFor(x => x).Must(x => x.PartsCost + x.LaborCost > 0)
            .WithMessage("At least one of PartsCost or LaborCost must be greater than 0.");
    }
}

public class CancelMaintenanceOrderValidator : AbstractValidator<CancelMaintenanceOrderDto>
{
    public CancelMaintenanceOrderValidator()
    {
        RuleFor(x => x.CancelReason).NotEmpty().MaximumLength(500);
    }
}