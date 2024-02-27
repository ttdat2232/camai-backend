using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class EdgeBox : BusinessEntity
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;

    [StringLength(50)]
    public string? Name { get; set; }
    public string? Version { get; set; }
    public Guid EdgeBoxModelId { get; set; }

    // TODO [Duy]: Edge box configuration
    public EdgeBoxStatus EdgeBoxStatus { get; set; }
    public EdgeBoxLocation EdgeBoxLocation { get; set; }

    public virtual EdgeBoxModel EdgeBoxModel { get; set; } = null!;
    public virtual ICollection<EdgeBoxInstall> Installs { get; set; } = new HashSet<EdgeBoxInstall>();
}
