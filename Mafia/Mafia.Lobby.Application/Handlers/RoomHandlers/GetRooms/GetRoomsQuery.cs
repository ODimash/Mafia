using Mafia.Lobby.DTO.DTOs;
using Mafia.Shared.Contracts.Messaging;
using Mafia.Shared.Contracts.Models;

namespace Mafia.Lobby.Application.Handlers.RoomHandlers.GetRooms;

public class GetRoomsQuery : IQuery<PagedResult<RoomDto>>
{
	public int Page { get; set; }
	public int PageSize { get; set; }
}
