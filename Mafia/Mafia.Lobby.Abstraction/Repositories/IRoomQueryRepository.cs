using Mafia.Lobby.Domain.Models;
using Mafia.Lobby.DTO.DTOs;
using Mafia.Shared.Contracts.Models;

namespace Mafia.Lobby.Abstraction.Repositories;

public interface IRoomQueryRepository
{
	Task<PagedResult<RoomDto>> GetRooms(
		PageFilter pageFilter, 
		Predicate<Room>? predicate = null, 
		CancellationToken cancellationToken = default);
}
