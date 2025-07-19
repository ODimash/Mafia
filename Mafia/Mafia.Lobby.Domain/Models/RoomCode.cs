using FluentResults;
using Mafia.Shared.Kernel;

namespace Mafia.Lobby.Domain.Models
{
    public class RoomCode : ValueObject
    {
        public string Code { get; private set; }

        private RoomCode(string code)
        {
            Code = code;
        }

        public static Result<RoomCode> Create(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Result.Fail("Code cannot be empty");
            
            if (code.Length != 6)
                return Result.Fail("Code must have length of 6");
            
            if (!IsAlphaNumeric(code))
                return Result.Fail("Code must have length of a letter and a number");
            
            return new RoomCode(code);  
        }

        private static bool IsAlphaNumeric(string code)
        {
            foreach (char c in code)
            {
                if (!char.IsLetterOrDigit(c)) return false;
            }   
            return true;
        }
        
        public static implicit operator string(RoomCode roomCode) => roomCode.Code;
        public override string ToString()
        {
            return Code;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}