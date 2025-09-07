using AutoMapper;
using Mafia.Lobby.Abstraction.Repositories;
using Mafia.Lobby.Domain.Models;
using Mafia.Lobby.DTO.DTOs;
using Mafia.Shared.Contracts.Models;
using System.Collections.Concurrent;

namespace Mafia.Lobby.Infrasctructure.Persistence.Repositories;

public class InMemoryRoomQueryRepository : IRoomQueryRepository
{
	private readonly InMemoryRoomRepository _roomRepository;
	private readonly IMapper _mapper;

	public InMemoryRoomQueryRepository(InMemoryRoomRepository roomRepository, IMapper mapper)
	{
		_roomRepository = roomRepository;
		_mapper = mapper;
	}

	public Task<PagedResult<RoomDto>> GetRooms(
		PageFilter pageFilter,
		Predicate<Room>? predicate = null,
		CancellationToken cancellationToken = default)
	{
		var query = _roomRepository.Rooms.Values.AsQueryable();
		if (predicate != null)
			query = query.Where(x => predicate.Invoke(x));

		var totalCount = query.Count();
		var result = query.Skip(pageFilter.PageSize * (pageFilter.Page - 1))
			.Take(pageFilter.PageSize)
			// .Select(x => _mapper.Map<RoomDto>(x))
			.ToList();
		
		var pagedResult = new PagedResult<RoomDto>()
		{
			Data = result.Select(x => _mapper.Map<RoomDto>(x)).ToList(),
			Page = pageFilter.Page,
			PageSize = pageFilter.PageSize,
			TotalPages = totalCount / pageFilter.PageSize + (totalCount % pageFilter.PageSize == 0 ? 0 : 1),
			TotalRecords = totalCount
		};

		return Task.FromResult(pagedResult);
	}
}
