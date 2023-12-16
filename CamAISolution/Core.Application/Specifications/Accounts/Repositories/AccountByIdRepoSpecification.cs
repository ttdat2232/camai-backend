using Core.Application.Specifications.Repositories;
using Core.Domain.Entities;

namespace Core.Application.Specifications.Accounts.Repositories;

/// <summary>
/// Using this to fetching all Classes (Tables) which relating to Account that equal with provied Id.
/// </summary>
/// <param name="id">Query account Id</param>
public class AccountByIdRepoSpecification : RepositorySpecification<Account>
{
    public AccountByIdRepoSpecification(Guid id) : base(a => a.Id == id)
    {
        //AddIncludes("Shops");
        //AddIncludes("Roles");
    }
}
