using Mafia.Lobby.Abstraction.Repositories;
using Mafia.Lobby.DTO.DTOs;
using Mafia.Shared.Contracts.Messaging;
using Mafia.Shared.Contracts.Models;

namespace Mafia.Lobby.Application.Handlers.RoomHandlers.GetRooms;

public class GetRoomsHandler : IQueryHandler<GetRoomsQuery, PagedResult<RoomDto>>
{
	private readonly IRoomQueryRepository  _roomRepository;
	
	public GetRoomsHandler(IRoomQueryRepository roomRepository)
	{
		_roomRepository = roomRepository;
	}

	public Task<PagedResult<RoomDto>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
	{
		return _roomRepository.GetRooms(pageFilter: new PageFilter() { Page = request.Page, PageSize = request.PageSize });
	}
}
