using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Application.Specifications.Repositories;

/// <summary>
/// Using this to fetching all Classes (Tables) which relating to Account that equal with provied Id.
/// </summary>
/// <param name="id">Query account Id</param>
public class AccountByIdRepoSpec : EntityByIdSpec<Account, Guid>
{
    public AccountByIdRepoSpec(Guid id)
        : base(a => a.Id == id && a.Role != Role.SystemHandler)
    {
        AddIncludes(a => a.Brand!.Logo, a => a.Brand!.Banner, a => a.Brand!.BrandManager, a => a.ManagingShop);
        AddIncludes(a => a.Ward!.District.Province);
        AddIncludes(a => a.ManagingShop);
        AddIncludes(a => a.Employee);
    }
}
