using FluentResults;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.API.RequestModels;

public interface IRequestModel<T>
{
	public T ToCommand();
}
