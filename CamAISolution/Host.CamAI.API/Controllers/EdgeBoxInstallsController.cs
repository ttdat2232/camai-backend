using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EdgeBoxInstallsController(IEdgeBoxInstallService edgeBoxInstallService, IBaseMapping mapper)
    : ControllerBase
{
    /// <summary>
    /// Get all installs of a shop.
    /// </summary>
    /// <remarks>Use for admin, Brand Manager and Shop Manager.</remarks>
    /// <param name="shopId"></param>
    /// <returns></returns>
    [HttpGet("/api/shops/{shopId}/installs")]
    [AccessTokenGuard(Role.Admin, Role.BrandManager, Role.ShopManager)]
    public async Task<PaginationResult<EdgeBoxInstallDto>> GetEdgeBoxInstallsByShop(Guid shopId)
    {
        var edgeBoxInstalls = await edgeBoxInstallService.GetInstallingByShop(shopId);
        return mapper.Map<EdgeBoxInstall, EdgeBoxInstallDto>(edgeBoxInstalls);
    }

    /// <summary>
    /// Get all installs of a brand.
    /// </summary>
    /// <remarks>Use for Admin and Brand Manager.</remarks>
    /// <param name="brandId"></param>
    /// <returns></returns>
    [HttpGet("/api/brands/{brandId}/installs")]
    [AccessTokenGuard(Role.Admin, Role.BrandManager)]
    public async Task<PaginationResult<EdgeBoxInstallDto>> GetEdgeBoxInstallsByBrand(Guid brandId)
    {
        var edgeBoxInstalls = await edgeBoxInstallService.GetInstallingByBrand(brandId);
        return mapper.Map<EdgeBoxInstall, EdgeBoxInstallDto>(edgeBoxInstalls);
    }

    /// <summary>
    /// For admin: Get all install of an edge box
    /// </summary>
    /// <param name="edgeBoxId"></param>
    /// <param name="request">request.EdgeBoxId will be replaced by the path param edgeBoxId </param>
    /// <returns></returns>
    [HttpGet("/api/edgeBoxes/{edgeBoxId}/installs")]
    [AccessTokenGuard(Role.Admin)]
    public async Task<PaginationResult<EdgeBoxInstallDto>> GetEdgeBoxInstallsByEdgeBox(
        [FromRoute] Guid edgeBoxId,
        [FromQuery] SearchEdgeBoxInstallRequest request
    )
    {
        request.EdgeBoxId = edgeBoxId;
        var edgeBoxInstalls = await edgeBoxInstallService.GetEdgeBoxInstall(request);
        return mapper.Map<EdgeBoxInstall, EdgeBoxInstallDto>(edgeBoxInstalls);
    }

    /// <summary>
    /// Search Edge Box Install
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("search")]
    [AccessTokenGuard(Role.Admin)]
    public async Task<PaginationResult<EdgeBoxInstallDto>> SearchEdgeBoxInstall(
        [FromQuery] SearchEdgeBoxInstallRequest request
    )
    {
        return mapper.Map<EdgeBoxInstall, EdgeBoxInstallDto>(await edgeBoxInstallService.GetEdgeBoxInstall(request));
    }

    /// <summary>
    /// Admin leases an edge box to a shop
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [AccessTokenGuard(Role.Admin)]
    public async Task<EdgeBoxInstallDto> LeaseEdgeBox(CreateEdgeBoxInstallDto dto)
    {
        var ebInstall = await edgeBoxInstallService.LeaseEdgeBox(dto);
        return mapper.Map<EdgeBoxInstall, EdgeBoxInstallDto>(ebInstall);
    }

    /// <summary>
    /// Brand manager activates an edge box
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut]
    [AccessTokenGuard(Role.BrandManager)]
    public async Task<EdgeBoxInstallDto> ActivateEdgeBox(ActivateEdgeBoxDto dto)
    {
        var ebInstall = await edgeBoxInstallService.ActivateEdgeBox(dto);
        var res = mapper.Map<EdgeBoxInstall, EdgeBoxInstallDto>(ebInstall);
        res.ActivationCode = null;
        return res;
    }

    /// <summary>
    /// Admin install an edge box
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("{id}/uninstall")]
    [AccessTokenGuard(Role.Admin)]
    public async Task<IActionResult> UninstallEdgeBox(Guid id)
    {
        await edgeBoxInstallService.UninstallEdgeBox(id);
        return NoContent();
    }

    [HttpGet("all")]
    [AccessTokenGuard(Role.Admin)]
    public async Task<ActionResult<List<EdgeBoxInstallDto>>> GetAllEdgeBoxInstall()
    {
        var edgeBoxInstalls = await edgeBoxInstallService.GetAllEdgeBoxInstall();
        return Ok(mapper.Map<List<EdgeBoxInstall>, List<EdgeBoxInstallDto>>(edgeBoxInstalls.ToList()));
    }

    [HttpGet("{id}")]
    [AccessTokenGuard(Role.Admin)]
    public async Task<ActionResult<EdgeBoxInstallDto>> GetEdgeBoxInstallById(Guid id)
    {
        var edgeBoxInstall = await edgeBoxInstallService.GetEdgeBoxInstallById(id);
        return Ok(mapper.Map<EdgeBoxInstall, EdgeBoxInstallDto>(edgeBoxInstall));
    }
}
