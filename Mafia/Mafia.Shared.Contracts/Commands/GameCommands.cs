using FluentResults;
using Mafia.Shared.Contracts.DTOs;
using Mafia.Shared.Contracts.DTOs.Games;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Shared.Contracts.Commands;

// Публичные команды для межмодульного взаимодействия:

public record StartGameCommand(GameSettingsDto GameSettings, List<Guid> PlayersIdentityId) : ICommand<Result<Guid>> { };

