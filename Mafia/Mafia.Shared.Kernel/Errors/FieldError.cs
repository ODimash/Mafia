
using FluentResults;

namespace Mafia.Shared.Kernel.Errors;

public class FieldError : ValidationError
{
    public string Field { get; }

    public FieldError(string field, string code, string message) : base(code, message)
    {
        Field = field ?? string.Empty;
    }
}
