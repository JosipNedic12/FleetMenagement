using FleetManagement.Application.DTOs;
using FluentValidation;

public class UpdateVehicleAssignmentValidator : AbstractValidator<UpdateVehicleAssignmentDto>
{
    public UpdateVehicleAssignmentValidator()
    {
        RuleFor(x => x.AssignedTo)
            .Must(date => date == null || date >= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("AssignedTo cannot be in the past.");
    }
}