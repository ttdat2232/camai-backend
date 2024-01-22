using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Models.DTO.Notifications;

public class CreateNotificationDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = null!;

    [Required]
    public string Content { get; set; } = null!;
    public IEnumerable<Guid> SentToId { get; set; } = new HashSet<Guid>();
}
