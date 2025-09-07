namespace Mafia.Lobby.DTO.DTOs;

public record RoomParticipantDto(Guid Id, Guid UserId, Guid RoomId, bool IsReady);