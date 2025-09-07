using AutoMapper;
using Mafia.Games.Domain.Models;
using Mafia.Shared.Contracts.DTOs.Games;
using Mafia.Shared.Contracts.Models.DTOs.Games;

namespace Mafia.Games.Application.Mappers;

public class DtoProfile : Profile
{
	public DtoProfile()
	{
		CreateMap<Game, GameDto>().ReverseMap();
		CreateMap<Player, PlayerDto>().ReverseMap();
		CreateMap<Player, PlayerWithRoleDto>().ReverseMap();
		CreateMap<GameSettings, GameSettingsDto>().ReverseMap();
		CreateMap<GamePhase, GamePhaseDto>().ReverseMap();
		CreateMap<PlayerAction, PlayerActionDto>().ReverseMap();
	}
}
