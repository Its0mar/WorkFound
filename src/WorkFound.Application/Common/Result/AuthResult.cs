using System.Text.Json.Serialization;

namespace WorkFound.Application.Common.Result;

public class AuthResult
{
    public bool Succeeded { get; set; }
    public IEnumerable<string>? Errors { get; set; }
    public Guid? UserId { get; set; }
    public string? Token { get; set; } 
    [JsonIgnore]
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpireOn { get; set; }
    public string? Role { get; set; }

    public static AuthResult Success(Guid userId, string? token = null,string? refreshToken = null,DateTime? refreshTokenExpireOn = null ,string? role = null) =>
        new()
        {
            Succeeded = true,
            UserId = userId,
            Token = token,
            RefreshToken = refreshToken,
            RefreshTokenExpireOn = refreshTokenExpireOn,
            Role = role
        };

    public static AuthResult Fail(string error) =>
        new() { Succeeded = false, Errors = [error] };

    public static AuthResult Fail(IEnumerable<string> errors) =>
        new() { Succeeded = false, Errors = errors };
}