using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class EdgeBoxInstallDto : BaseDto
{
    public Guid EdgeBoxId { get; set; }
    public DateTime? LastSeen { get; set; }
    public Guid ShopId { get; set; }
    public string? OperatingSystem { get; set; }
    public string? ActivationCode { get; set; }
    public DateTime? UninstalledTime { get; set; }
    public EdgeBoxActivationStatus ActivationStatus { get; set; }
    public EdgeBoxInstallStatus EdgeBoxInstallStatus { get; set; }
    public string? IpAddress { get; set; }

    public EdgeBoxDto EdgeBox { get; set; } = null!;
    public ShopDto Shop { get; set; } = null!;
}
