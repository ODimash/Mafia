using Mafia.Shared.Kernel;

namespace Mafia.Games.Domain.Events;

public class CheckedPlayerIsMafiaDomainEvent : DomainEvent
{
	public CheckedPlayerIsMafiaDomainEvent(Guid checkerId, Guid targetId, bool isMafia)
	{
		CheckerId = checkerId;
		TargetId = targetId;
		IsMafia = isMafia;

	}
	public Guid CheckerId { get;  }
	public Guid TargetId { get; }
	public bool IsMafia { get; }
}
