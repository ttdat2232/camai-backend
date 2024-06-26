using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.Mail;
using Core.Domain.Constants;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Domain.DTO;

public class ShopFromImportFile
{
    public string? ExternalShopId { get; set; }
    public string ShopName { get; set; } = string.Empty;
    public string ShopOpenTime { get; set; } = string.Empty;
    public string ShopCloseTime { get; set; } = string.Empty;
    public string? ShopPhone { get; set; }

    /// <summary>
    /// Shop's address line
    /// </summary>
    public string ShopAddress { get; set; } = null!;
    public string? ExternalShopManagerId { get; set; }
    public string ShopManagerEmail { get; set; } = null!;
    public string ShopManagerName { get; set; } = null!;
    public Gender? ShopManagerGender { get; set; } = Gender.Male;

    /// <summary>
    /// Shop manager's address line
    /// </summary>
    public string ShopManagerAddress { get; set; } = null!;

    private IDictionary<string, object?> ShopValidation()
    {
        var result = new Dictionary<string, object?>();
        if (string.IsNullOrEmpty(ShopName))
            result.Add($"{nameof(ShopName)}", "Cannot be empty and length must be less than or equal to 50");
        else if (ShopName.Length > 50)
            result.Add($"{nameof(ShopName)}", "Shop name's length must be less than or equal to 50");
        if (!string.IsNullOrEmpty(ShopPhone) && !RegexHelper.VietNamPhoneNumber.IsMatch(ShopPhone))
            result.Add($"{nameof(ShopPhone)}", $"{ShopPhone} is wrong, phone must start with +84|84|0 and then 3|5|7|8|9 and then 8 digits from 0-9. E.g: 0982335536");
        if (string.IsNullOrEmpty(ShopAddress))
            result.Add($"{nameof(ShopAddress)}", "Cannot be empty");

        if (string.IsNullOrEmpty(ShopOpenTime))
            result.Add($"{nameof(ShopOpenTime)}", "Cannot be empty");
        else if (!TimeOnly.TryParseExact(ShopOpenTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            result.Add($"{nameof(ShopOpenTime)}", "wrong format, valid format is hh:mm");

        if (string.IsNullOrEmpty(ShopCloseTime))
            result.Add($"{nameof(ShopCloseTime)}", "Cannot be empty");
        else if (!TimeOnly.TryParseExact(ShopCloseTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            result.Add($"{nameof(ShopCloseTime)}", "wrong format, valid format is hh:mm");

        return result;
    }

    private IDictionary<string, object?> AccountValidation()
    {
        var result = new Dictionary<string, object?>();
        if (string.IsNullOrEmpty(ShopManagerEmail))
            result.Add($"{nameof(ShopManagerEmail)}", "Cannot be empty");
        else if (!MailAddress.TryCreate(ShopManagerEmail, out _))
            result.Add($"{nameof(ShopManagerEmail)}", $"{ShopManagerEmail} is wrong format, e.g: example@gmail.com");
        if (string.IsNullOrEmpty(ShopManagerName))
            result.Add($"{nameof(ShopManagerName)}", "Cannot be empty");
        if (ShopManagerName.Length > 50)
            result.Add($"{nameof(ShopManagerName)}", "Manager name's length must be less than or equal to 50");
        if (string.IsNullOrEmpty(ShopManagerAddress))
            result.Add($"{nameof(ShopManagerAddress)}", "Cannot be empty");
        return result;
    }

    public bool IsValid() => !ShopFromImportFileValidation().Any();

    public IDictionary<string, object?> ShopFromImportFileValidation() =>
        AccountValidation()
            .Aggregate(
                ShopValidation(),
                (aggregateDict, next) =>
                {
                    aggregateDict[next.Key] = next.Value;
                    return aggregateDict;
                }
            );

    public Account GetManager() =>
        new()
        {
            Email = ShopManagerEmail,
            Name = ShopManagerName,
            Gender = ShopManagerGender,
            AddressLine = ShopManagerAddress,
            ExternalId = ExternalShopManagerId == string.Empty ? null : ExternalShopManagerId,
        };

    public Shop GetShop() =>
        new()
        {
            ExternalId = ExternalShopId == string.Empty ? null : ExternalShopId,
            Name = ShopName,
            OpenTime = TimeOnly.ParseExact(ShopOpenTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None),
            CloseTime = TimeOnly.ParseExact(ShopCloseTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None),
            Phone = ShopPhone == string.Empty ? null : ShopPhone,
            AddressLine = ShopAddress
        };
}
