using Core.Domain.Enums;
using Core.Domain.Models.DTO;

namespace Core.Domain.DTO;

public class EvidenceDto : BaseDto
{
    public EvidenceType EvidenceType { get; set; }
    public Guid IncidentId { get; set; }
    public Guid CameraId { get; set; }
    public Guid? ImageId { get; set; }

    public virtual ImageDto? Image { get; set; }
    public virtual IncidentDto Incident { get; set; } = null!;
    public virtual CameraDto Camera { get; set; } = null!;
}
