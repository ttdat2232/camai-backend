using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Entities;

public class Role : BaseEntity
{
    [StringLength(50)]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new HashSet<Account>();
}
