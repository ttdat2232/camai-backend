using Core.Application.Exceptions;
using Core.Domain.DTO;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class AuthService(IJwtService jwtService, IAccountService accountService, IUnitOfWork unitOfWork) : IAuthService
{
    public async Task<TokenResponseDto> GetTokensByUsernameAndPassword(
        string email,
        string password,
        bool isFromMobile,
        string userIp
    )
    {
        var foundAccount = await unitOfWork.Accounts.GetAsync(
            expression: a =>
                a.Email == email && (a.AccountStatus == AccountStatus.Active || a.AccountStatus == AccountStatus.New),
            orderBy: e => e.OrderBy(a => a.Id)
        );
        if (foundAccount.Values.Count == 0)
            throw new UnauthorizedException("Wrong email or password");

        var account = foundAccount.Values[0];

        var acceptanceRoles = new Role[] { Role.BrandManager, Role.Admin, Role.ShopManager, Role.ShopSupervisor };
        if (!acceptanceRoles.Contains(account.Role))
            throw new UnauthorizedException("Wrong email or password");

        var isHashedCorrect = Hasher.Verify(password, account.Password);
        if (!isHashedCorrect)
            throw new UnauthorizedException("Wrong email or password");
        TokenType accessTokenType = TokenType.WebAccessToken;
        TokenType refreshTokenType = TokenType.WebRefreshToken;
        if (isFromMobile)
        {
            accessTokenType = TokenType.MobileAccessToken;
            refreshTokenType = TokenType.MobileRefreshToken;
        }
        var role = account.Role;
        var accessToken = jwtService.GenerateToken(account.Id, role, account.AccountStatus, accessTokenType, userIp);
        var refreshToken = jwtService.GenerateToken(account.Id, role, refreshTokenType, userIp);
        return new TokenResponseDto { AccessToken = accessToken, RefreshToken = refreshToken };
    }

    // TODO [Huy]: check account status before renew token
    public async Task<string> RenewToken(string oldAccessToken, string refreshToken, bool isFromMobile, string userIp)
    {
        TokenType accessTokenType = TokenType.WebAccessToken;
        TokenType refreshTokenType = TokenType.WebRefreshToken;
        if (isFromMobile)
        {
            accessTokenType = TokenType.MobileAccessToken;
            refreshTokenType = TokenType.MobileRefreshToken;
        }
        var accessTokenDetail = jwtService.ValidateToken(
            oldAccessToken,
            accessTokenType,
            userIp,
            isValidateTime: false
        );
        var refreshTokenDetail = jwtService.ValidateToken(refreshToken, refreshTokenType, userIp);

        if (accessTokenDetail.UserId != refreshTokenDetail.UserId)
            throw new UnauthorizedException("Invalid Tokens");
        if (accessTokenDetail.Token == null)
            throw new UnauthorizedException("Invalid Tokens");

        var account = await accountService.GetAccountById(
            accessTokenDetail.UserId,
            includeAdmin: refreshTokenDetail.UserRole == Role.Admin
        );
        return jwtService.GenerateToken(account.Id, account.Role, account.AccountStatus, accessTokenType, userIp);
    }

    public async Task ChangePassword(ChangePasswordDto changePasswordDto)
    {
        if (changePasswordDto.NewPassword != changePasswordDto.NewPasswordRetype)
            throw new BadRequestException("New password and retype password is not the same");

        if (changePasswordDto.OldPassword == changePasswordDto.NewPassword)
            throw new BadRequestException("New password cannot be the same as old password");

        var currentAccount = accountService.GetCurrentAccount();
        if (!Hasher.Verify(changePasswordDto.OldPassword, currentAccount.Password))
            throw new UnauthorizedException("Wrong password");

        currentAccount.Password = Hasher.Hash(changePasswordDto.NewPassword);
        currentAccount.AccountStatus = AccountStatus.Active;
        unitOfWork.Accounts.Update(currentAccount);
        await unitOfWork.CompleteAsync();
    }
}
