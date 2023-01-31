namespace GameTimerWeb
{
    [Serializable]
    public class GameTimerSettings
    {
        
        #region Constants

        [NonSerialized]
        public static readonly string LOCAL_STORAGE_KEY = "GameTimerSettings";

        [NonSerialized]
        public static readonly TimeSpan DEFAULT_TIME_PER_PLAYER = new TimeSpan(0, 3, 0);

        [NonSerialized]
        public static readonly TimeSpan DEFAULT_INCREMENT = TimeSpan.Zero;

        [NonSerialized]
        public static readonly int DEFAULT_NUMBER_OF_PLAYERS = 4;

        [NonSerialized]
        public static readonly bool DEFAULT_KEEP_DIRECTION = false;

        [NonSerialized]
        public static readonly bool DEFAULT_ENABLE_COLOURS = false;

        [NonSerialized]
        public static readonly bool DEFAULT_BIG_REVERSE = false;

        #endregion

        public GameTimerSettings()
        {
            TimePerPlayer = DEFAULT_TIME_PER_PLAYER;
            Increment = DEFAULT_INCREMENT;
            NumberOfPlayersSetting = DEFAULT_NUMBER_OF_PLAYERS;
            KeepDirectionBetweenGames = DEFAULT_KEEP_DIRECTION;
            EnableColours = DEFAULT_ENABLE_COLOURS;
            PlayerColours = new Dictionary<int, System.Drawing.Color>();
            BigReverse = DEFAULT_BIG_REVERSE;
        }

        public TimeSpan TimePerPlayer { get; set; }

        public TimeSpan Increment { get; set; }

        public int NumberOfPlayersSetting { get; set; }

        public bool KeepDirectionBetweenGames { get; set; }

        public bool EnableColours { get; set; }

        public bool BigReverse { get; set; }

        [NonSerialized]
        public Dictionary<int, System.Drawing.Color> PlayerColours;
        
    }
}
