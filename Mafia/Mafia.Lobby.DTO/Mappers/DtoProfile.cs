using AutoMapper;
using Mafia.Lobby.Domain.Models;
using Mafia.Lobby.DTO.DTOs;
using Mafia.Shared.Contracts.Models.DTOs.Lobby;

namespace Mafia.Lobby.DTO.Mappers;

public class DtoProfile : Profile
{
	public DtoProfile()
	{
		CreateMap<Room, RoomDto>().ReverseMap();
		CreateMap<RoomParticipant, RoomParticipantDto>().ReverseMap();
		CreateMap<RoomSettings, RoomSettingsDto>()
			.ForMember(dest => dest.DayDiscussionDurationInSeconds, opt => opt.MapFrom(src => (int)src.DayDiscussionDuration.TotalSeconds))
			.ForMember(dest => dest.NightDurationInSeconds, opt => opt.MapFrom(src => (int)src.NightDuration.TotalSeconds))
			.ForMember(dest => dest.VotingDurationInSeconds, opt => opt.MapFrom(src => (int)src.VotingDuration.TotalSeconds))
			.ReverseMap()
			.ForMember(dest => dest.DayDiscussionDuration, opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.DayDiscussionDurationInSeconds)))
			.ForMember(dest => dest.NightDuration, opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.NightDurationInSeconds)))
			.ForMember(dest => dest.VotingDuration, opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.VotingDurationInSeconds)));
	}
}
