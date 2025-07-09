
using FluentResults;
using Mafia.Game.Domain.Enums;
using Mafia.Game.Domain.ValueObjects;
using Mafia.Shared.Kernel;
using Mafia.Shared.Kernel.Errors;

namespace Mafia.Game.Domain.Entities;

public class Vote : Entity<Guid>
{
    public Guid VoterId { get; }
    public Guid TargetId { get; }
   
    private Vote(Guid id, Guid voterId, Guid targetId, DateTime createdAt)
    {
        Id = id;
        VoterId = voterId;
        TargetId = targetId;
        CreatedAt = createdAt;
    }

    public static Result<Vote> Create(Player voter, Player targetPlayer, GamePhase gamePhase)
    {
        if (voter.IsKilled)
            return Result.Fail("The player already died");

        if (gamePhase.Type == PhaseType.Voting) 
            return Result.Ok(new Vote(Guid.NewGuid(), voter.Id, targetPlayer.Id, DateTime.UtcNow));

        return Result.Ok(new Vote(Guid.NewGuid(), voter.Id, targetPlayer.Id, DateTime.UtcNow));
    }

}
