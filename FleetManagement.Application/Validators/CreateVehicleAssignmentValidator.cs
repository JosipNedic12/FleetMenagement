using FleetManagement.Application.DTOs;
using FluentValidation;

public class CreateVehicleAssignmentValidator : AbstractValidator<CreateVehicleAssignmentDto>
{
    public CreateVehicleAssignmentValidator()
    {
        RuleFor(x => x.VehicleId).GreaterThan(0);
        RuleFor(x => x.DriverId).GreaterThan(0);
        RuleFor(x => x.AssignedFrom).NotEmpty();
        RuleFor(x => x.AssignedTo)
            .Must((dto, assignedTo) => assignedTo == null || assignedTo > dto.AssignedFrom)
            .WithMessage("AssignedTo must be after AssignedFrom.");
    }
}