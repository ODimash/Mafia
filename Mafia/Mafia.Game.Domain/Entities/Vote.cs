
using FluentResults;
using Mafia.Game.Domain.Enums;
using Mafia.Game.Domain.ValueObjects;
using Mafia.Shared.Kernel;
using Mafia.Shared.Kernel.Errors;

namespace Mafia.Game.Domain.Entities;

public class Vote : Entity<Guid>
{

    public PhaseType Phase { get; }
    public Guid VoterId { get; }
    public Guid TargetId { get; }
   
    private Vote(Guid id, PhaseType phase, Guid voterId, Guid targetId, DateTime createdAt)
    {
        Id = id;
        Phase = phase;
        VoterId = voterId;
        TargetId = targetId;
        CreatedAt = createdAt;
    }

    public static Result<Vote> Create(Player voter, Player targetPlayer, GamePhase gamePhase)
    {
        if (voter.IsKilled)
            return Result.Fail("The player already died");

        if (gamePhase.Type == PhaseType.Voting) 
            return Result.Ok(new Vote(Guid.NewGuid(), PhaseType.Voting, voter.Id, targetPlayer.Id, DateTime.UtcNow));

        if (voter.Role.ActivePhase != gamePhase.Type)
            return Result.Fail("Player can't vote right now");

        return Result.Ok(new Vote(Guid.NewGuid(), PhaseType.Voting, voter.Id, targetPlayer.Id, DateTime.UtcNow));
    }

}
