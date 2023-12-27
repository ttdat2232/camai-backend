using Core.Domain.Repositories;
using Core.Domain.Services;
using Core.Domain.Entities;
using Core.Domain.Models;
using Core.Application.Specifications.Accounts.Repositories;
using Core.Application.Exceptions;

namespace Core.Application;

public class AccountService(IUnitOfWork unitOfWork, IJwtService jwtService) : IAccountService
{
    public Task<PaginationResult<Account>> GetAccount(
        Guid? guid = null,
        DateTime? from = null,
        DateTime? to = null,
        int pageSize = 1,
        int pageIndex = 0
    )
    {
        /* var specification = new AccountSearchSpecification(guid, from, to, pageSize, pageIndex);
         return accountRepo.GetAsync(specification);*/
        throw new NotImplementedException();
        //return accountRepo.GetAsync(specification);
    }

    public async Task<Account> GetAccountById(Guid id)
    {
        var foundAccounts =await unitOfWork.Accounts.GetAsync(new AccountByIdRepoSpec(id));
        if (foundAccounts.Values.Count == 0)
            throw new NotFoundException(typeof(Account), id);
        return foundAccounts.Values[0];
    }

    public Task<Account> GetCurrentAccount()
    {
        return GetAccountById(jwtService.GetCurrentUserId());
    }
}
