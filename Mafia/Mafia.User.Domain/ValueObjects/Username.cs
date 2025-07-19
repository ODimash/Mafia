using System.Text.RegularExpressions;

namespace Mafia.User.Domain.ValueObjects;

public class Username
{
    private static readonly Regex UsernameRegex =
        new(@"^[a-zA-Z0-9_]{3,32}$", RegexOptions.Compiled);

    public string Value { get; }
    public IReadOnlyList<string> Errors { get; }

    private Username(string value, IReadOnlyList<string>? errors = null)
    {
        Value = value;
        Errors = errors ?? new List<string>();
    }

    public static FluentResults.Result<Username> Create(string username)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(username))
            errors.Add("Имя пользователя не может быть пустым.");
        else if (!UsernameRegex.IsMatch(username))
            errors.Add("Имя пользователя должно содержать от 3 до 32 символов: латиница, цифры, подчёркивание.");

        if (errors.Count > 0)
            return FluentResults.Result.Fail<Username>(string.Join("; ", errors))
                .WithValue(new Username(username, errors));

        return FluentResults.Result.Ok(new Username(username.Trim()));
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj)
        => obj is Username other && Value.Equals(other.Value, StringComparison.Ordinal);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(Username left, Username right) => Equals(left, right);
    public static bool operator !=(Username left, Username right) => !Equals(left, right);
}