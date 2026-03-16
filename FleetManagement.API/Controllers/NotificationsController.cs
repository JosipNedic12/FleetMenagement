using FleetManagement.Application.DTOs;
using FleetManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FleetManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationRepository _repo;

    public NotificationsController(INotificationRepository repo)
    {
        _repo = repo;
    }

    private int? GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)
                 ?? User.FindFirst(JwtRegisteredClaimNames.Sub)
                 ?? User.FindFirst("sub");
        return claim is null ? null : int.Parse(claim.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var notifications = await _repo.GetByUserIdAsync(userId.Value);
        var dtos = notifications.Select(n => new NotificationDto
        {
            NotificationId = n.NotificationId,
            Title = n.Title,
            Message = n.Message,
            Type = n.Type,
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt,
            RelatedEntityType = n.RelatedEntityType,
            RelatedEntityId = n.RelatedEntityId
        }).ToList();

        return Ok(dtos);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount()
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var count = await _repo.GetUnreadCountAsync(userId.Value);
        return Ok(new { count });
    }

    [HttpPut("{id:int}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        await _repo.MarkAsReadAsync(id, userId.Value);
        return NoContent();
    }

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        await _repo.MarkAllAsReadAsync(userId.Value);
        return NoContent();
    }
}
