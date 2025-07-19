
using FluentResults;
using Mafia.Shared.Kernel;

namespace Mafia.Lobby.Domain.Models
{
    public class RoomParticipant : Entity<Guid>
    {

        public Guid UserId { get; }
        public Guid RoomId { get; }
        public bool IsReady { get; private set; }
		
        private RoomParticipant(Guid userId, Guid roomId, bool isReady)
        {
            UserId = userId;
            RoomId = roomId;
            IsReady = isReady;
        }

        public static Result<RoomParticipant> Create(Guid userId, Guid roomId)
        {
            if (userId == Guid.Empty)
                return Result.Fail("User ID can not be empty");

            if (roomId == Guid.Empty)
                return  Result.Fail("Room ID can not be empty");

            return new RoomParticipant(userId, roomId, true);
        }
    }
}