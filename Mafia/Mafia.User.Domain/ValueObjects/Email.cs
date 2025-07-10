using System.Text.RegularExpressions;

namespace Mafia.User.Domain.ValueObjects
{
    public class Email
    {
        private static readonly Regex EmailRegex =
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Value { get; }
        public IReadOnlyList<string> Errors { get; }

        private Email(string value, IReadOnlyList<string>? errors = null)
        {
            Value = value;
            Errors = errors ?? new List<string>();
        }

        public static FluentResults.Result<Email> Create(string email)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(email))
                errors.Add("Email не может быть пустым.");
            else if (!EmailRegex.IsMatch(email))
                errors.Add("Некорректный формат email.");

            if (errors.Count > 0)
                return FluentResults.Result.Fail<Email>(string.Join("; ", errors))
                    .WithValue(new Email(email, errors));

            return FluentResults.Result.Ok(new Email(email.Trim()));
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj)
            => obj is Email other && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();

        public static bool operator ==(Email left, Email right) => Equals(left, right);
        public static bool operator !=(Email left, Email right) => !Equals(left, right);
    }
}
