using FluentResults;
using Mafia.Shared.Kernel;

namespace Mafia.Lobby.Domain.Models
{
    public class RoomName :  ValueObject
    {
        public string Name { get; private set; }
        
        private RoomName(string name)
        {
            Name = name;
        }

        public static Result<RoomName> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Fail("Room name cannot be empty");

            if (name.Length > 16)
                return Result.Fail("Room name cannot be longer than 16 characters");
            
            return new RoomName(name);
        }

        public override string ToString() => Name;
        public static implicit operator string(RoomName roomName) => roomName.Name;

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}