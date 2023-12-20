using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities;

public class AccountStatus : BaseEntity
{
    [StringLength(20)]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
