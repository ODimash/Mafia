
namespace Mafia.Shared.Kernel.Constants
{
    public static class GameSettingConstants
    {
        public const int MinPlayersCount = 5;
        public const int MaxPlayersCount = 21;
        public static readonly TimeSpan MaxPhaseDuration = TimeSpan.FromMinutes(5);
        public static readonly TimeSpan MinPhaseDuration = TimeSpan.FromSeconds(30);
    }
}
