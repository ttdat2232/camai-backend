using System.Security.Claims;
using Core.Domain.Entities;
using Core.Domain.Models.DTOs.Auths;
using Core.Domain.Models.Enums;

namespace Core.Domain.Interfaces.Services;

public interface IJwtService
{
    string GenerateToken(Guid userID, string[] roles, TokenType tokenType);

    TokenDetailDto ValidateToken(string token, TokenType tokenType, string[] acceptableRoles = null);

    IEnumerable<Claim> GetClaims(string token, TokenType tokenType);

    Guid GetCurrentUserId();
}
