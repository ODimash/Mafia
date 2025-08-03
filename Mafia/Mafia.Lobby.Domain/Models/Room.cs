
using FluentResults;
using Mafia.Lobby.Domain.DomainEvents;
using Mafia.Lobby.Domain.Enums;
using Mafia.Shared.Kernel;

namespace Mafia.Lobby.Domain.Models;
public class Room : AggregateRoot<Guid>
{
	private readonly List<RoomParticipant> _players;
    private readonly List<Guid> _cickedPlayers = [];
	public IReadOnlyList<RoomParticipant> Players => _players;
	public RoomSettings Settings { get; private set; }
	public Guid OwnerId { get; private set; }
	public string Password { get; private set; }
    public bool IsPrivate { get; private set; }
	public RoomCode Code { get; }
    public RoomState State { get; private set; }
    public RoomName Name { get; private set; }

	private Room(
        Guid id, 
        RoomSettings settings, 
        Guid ownerId, 
        RoomCode code, 
        IReadOnlyList<RoomParticipant> players, 
        RoomName name,
        bool isPrivate = false,
        string password  = "",
        RoomState state = RoomState.Waiting)
    {
        Id = id;
		Settings = settings;
		OwnerId = ownerId;
		Code = code;
        Name = name;
        IsPrivate = isPrivate;
        State = state;
		Password = password;
        _players = players.ToList();
    }

	public static Result<Room> Create(
        RoomSettings settings, 
        Guid ownerId, 
        RoomCode code, 
        RoomName name,
        bool isPrivate = false,
        string password = ""
        )
	{
		if (ownerId == Guid.Empty)
			return Result.Fail("Room cannot be created without owner");

        Guid roomId = Guid.NewGuid();
        var ownerAsMemberResult = RoomParticipant.Create(ownerId, roomId);
        if (ownerAsMemberResult.IsFailed)
            return ownerAsMemberResult.ToResult();
        
        if (isPrivate && password == string.Empty)
            return Result.Fail("Password cannot be empty");
        
        if (isPrivate && password.Length > 32)
            return Result.Fail("Password cannot be longer than 32 characters");
        
		var room = new Room(roomId, settings, ownerId, code, [ownerAsMemberResult.Value], name, isPrivate, password);
        room.AddDomainEvent(new RoomCreatedDomainEvent(roomId));
        return room;
    }

    public Result Join(Guid userId, string? password = null)
    {
        if (State != RoomState.Waiting) 
            return Result.Fail("Cannot join a room that is not waiting");
        
        if (userId == Guid.Empty)
            return Result.Fail("User cannot be created without owner");
        
        if (Settings.MaxPlayersCount <= _players.Count)
            return Result.Fail("Max players count exceeded");

        if (_players.Any(p => p.UserId == userId))
            return Result.Fail("Cannot join a room that is already joined");
        
        if (_cickedPlayers.Contains(userId))
            return Result.Fail("Cannot join a room that is already cicked");
        
        if (IsPrivate && password != Password)
            return Result.Fail("Passwords do not match");
        
        var createRoomParticipantResult = RoomParticipant.Create(userId, Id);
        if (createRoomParticipantResult.IsFailed)
            return createRoomParticipantResult.ToResult();
        
        _players.Add(createRoomParticipantResult.Value);
        AddDomainEvent(new JoinedNewPlayerDomainEvent(Id, userId, createRoomParticipantResult.Value.Id));
        return Result.Ok();
    }

    public Result Leave(Guid userId)
    {
        if (State != RoomState.Waiting)
            return Result.Fail("Cannot leave a room that is not waiting");
        
        _players.RemoveAll(p => p.UserId == userId);
        AddDomainEvent(new PlayerLeftRoomDomainEvent(Id, userId));
        return Result.Ok();
    }
    
    public Result Kick(Guid userId) 
    {
        if (State != RoomState.Waiting)
            return Result.Fail("Cannot kick a room that is not waiting");

        _players.RemoveAll(p => p.UserId == userId);
        _cickedPlayers.Add(userId);
        AddDomainEvent(new PlayerKickedDomainEvent(Id, userId));
        return Result.Ok();
    }
}
