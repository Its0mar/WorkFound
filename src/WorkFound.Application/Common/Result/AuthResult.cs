namespace WorkFound.Application.Common.Result;

public class AuthResult
{
    public bool Succeeded { get; set; }
    public IEnumerable<string>? Errors { get; set; }
    public Guid? UserId { get; set; }
    public string? Token { get; set; } 
    public string? Role { get; set; }

    public static AuthResult Success(Guid userId, string? token = null, string? role = null) =>
        new()
        {
            Succeeded = true,
            UserId = userId,
            Token = token,
            Role = role
        };

    public static AuthResult Fail(string error) =>
        new() { Succeeded = false, Errors = new[] { error } };

    public static AuthResult Fail(IEnumerable<string> errors) =>
        new() { Succeeded = false, Errors = errors };
}