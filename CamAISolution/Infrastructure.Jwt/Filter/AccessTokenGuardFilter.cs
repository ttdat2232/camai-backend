using Core.Application.Exceptions;
using Core.Domain.Models.DTO.Accounts;
using Core.Domain.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Jwt.Guard;

public class AccessTokenGuardFilter(IAccountService accountService, int[] roles) : IAuthorizationFilter
{
    public async void OnAuthorization(AuthorizationFilterContext context)
    {
        var tokenStr = context.HttpContext.Request.Headers.Authorization.ToString();
        var jwtService = context.HttpContext.RequestServices.GetRequiredService(typeof(IJwtService)) as IJwtService;
        if (jwtService == null)
            throw new ServiceUnavailableException("Service is unavailable");

        if (!tokenStr.StartsWith("Bearer "))
            throw new BadRequestException("Missing Bearer or wrong type");

        tokenStr = tokenStr["Bearer ".Length..];
        var token = jwtService.ValidateToken(tokenStr, TokenTypeEnum.AccessToken, roles);

        // validate account status is not new
        var account = await accountService.GetAccountById(token.UserId);
        if (account.AccountStatusId == AccountStatusEnum.New)
            throw new NewAccountException(account);
    }
}
