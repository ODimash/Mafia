namespace Mafia.Lobby.API.Models.RequestModels;

public interface IRequestModel<T>
{
	public T ToCommand();
}
