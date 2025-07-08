
using FluentResults;

namespace Mafia.Shared.Kernel.Errors;

public class ValidationError : Error
{
    public string Code { get; }

    public ValidationError(string code, string message)
        : base(message)
    {
        Code = code;
        WithMetadata("Type", "Validation");
        WithMetadata("Code", code);
    }
}

