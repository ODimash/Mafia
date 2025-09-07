using Mafia.Lobby.Domain.Enums;
using Mafia.Shared.Contracts.Models.DTOs.Lobby;

namespace Mafia.Lobby.DTO.DTOs;

public record RoomDto(
	Guid Id,
	IReadOnlyList<RoomParticipantDto> Players,
	RoomSettingsDto Settings,
	bool GameStarted,
	Guid OwnerId,
	string Code,
	string Name,
	bool IsPrivate = false,
	string Password = "",
	RoomState State = RoomState.Waiting);
