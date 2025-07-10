using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Mafia.User.Domain.ValueObjects
{
    public class PasswordHash
    {
        private const int MinLength = 6;
        private const int MaxLength = 100;

        public string Value { get; }
        public IReadOnlyList<string> Errors { get; }

        private PasswordHash(string value, IReadOnlyList<string>? errors = null)
        {
            Value = value;
            Errors = errors ?? new List<string>();
        }

        public static FluentResults.Result<PasswordHash> Create(string password)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(password))
                errors.Add("Пароль не может быть пустым.");
            else if (password.Length < MinLength)
                errors.Add($"Пароль должен содержать не менее {MinLength} символов.");
            else if (password.Length > MaxLength)
                errors.Add($"Пароль не должен превышать {MaxLength} символов.");

            // Можно добавить дополнительные проверки сложности пароля (буквы, цифры, спецсимволы)

            if (errors.Count > 0)
                return FluentResults.Result.Fail<PasswordHash>(string.Join("; ", errors))
                    .WithValue(new PasswordHash(password, errors));

            // Хеширование пароля (SHA256 для примера, в реальном проекте используйте более безопасные методы)
            var hash = ComputeSha256Hash(password);

            return FluentResults.Result.Ok(new PasswordHash(hash));
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var builder = new StringBuilder();
            foreach (var b in bytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj)
            => obj is PasswordHash other && Value.Equals(other.Value, StringComparison.Ordinal);

        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator ==(PasswordHash left, PasswordHash right) => Equals(left, right);
        public static bool operator !=(PasswordHash left, PasswordHash right) => !Equals(left, right);
    }
}
