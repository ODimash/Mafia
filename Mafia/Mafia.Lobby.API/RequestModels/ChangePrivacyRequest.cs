using Mafia.Lobby.Application.Handlers.RoomHandlers.ChangePrivacy;

namespace Mafia.Lobby.API.RequestModels;

public record ChangePrivacyRequest(bool IsPrivate, string? Password)
{
	public ChangePrivacyCommand ToCommand(Guid roomId, Guid userId) => new()
	{
		RoomId = roomId, 
		UserId = userId, 
		Password = Password, 
		IsPrivate = true
	};
}
