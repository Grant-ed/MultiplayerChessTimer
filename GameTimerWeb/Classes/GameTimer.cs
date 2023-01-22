namespace GameTimerWeb
{
    public class GameTimer
    {

        #region Constants

        public static readonly TimeSpan DEFAULT_TIME_PER_PLAYER = new TimeSpan(0, 3, 0);

        public static readonly TimeSpan DEFAULT_INCREMENT = TimeSpan.Zero;

        public static readonly TimeSpan CLOCK_TICKSPEED = TimeSpan.FromMilliseconds(10);

        private static readonly int DEFAULT_NUMBER_OF_PLAYERS = 4;

        private static readonly bool INITIAL_GAME_PAUSE_STATE = true;

        private static readonly bool INITIAL_CLOCKWISE = true;

        #endregion

        public GameTimer()
        {
            InitializeGameTimer();
        }

        #region Variables

        private List<TimeSpan> playersTimeLeft = new();

        public TimeSpan? GetPlayerTimeLeft(int playerNumber)
        {
            if (playerNumber >= playersTimeLeft.Count)
                return null;
            
            return playersTimeLeft[playerNumber];
        }

        private int? currentPlayerIndex = null;

        public int? CurrentPlayerIndex
        {
            get => currentPlayerIndex;
        }

        private bool paused = INITIAL_GAME_PAUSE_STATE;

        public bool Paused
        {
            get => paused;
        }

        public AutoResetEvent timerSignal = new AutoResetEvent(INITIAL_GAME_PAUSE_STATE);

        public int CurrentNumberOfPlayers
        {
            get => playersTimeLeft.Count;
        }

        private DateTime lastTimeTicked = DateTime.Now;

        private bool clockWise = INITIAL_CLOCKWISE;

        public bool ClockWise
        {
            get => clockWise;
        }

        #endregion

        #region Methods

        public void InitializeGameTimer()
        {
            playersTimeLeft = Enumerable.Repeat(TimePerPlayer, NumberOfPlayersSetting).ToList();
            currentPlayerIndex = null;
            paused = INITIAL_GAME_PAUSE_STATE;
            if (KeepDirectionBetweenGames == false)
                clockWise = clockWiseInitialGameDirection;
            NotifyGameStateChanged();
        }

        public void PauseGameTimer()
        {
            if (timerSignal.Reset() == false)
                return;
            paused = true;
            NotifyGameStateChanged();
        }

        public void ResumeGameTimer()
        {
            if (timerSignal.Set() == false)
                return;
            paused = false;
            lastTimeTicked = DateTime.Now;
            NotifyGameStateChanged();
        }
        
        public void NextPlayer(int requestingPlayerIndex)
        {
            if (paused && requestingPlayerIndex == currentPlayerIndex)
            {
                ResumeGameTimer();
                return;
            }

            // If the requesting player is not the current player, ignore the request, unless the current player is null
            if (currentPlayerIndex != null && requestingPlayerIndex != currentPlayerIndex)
                return;

            if (currentPlayerIndex != null && paused == false && playersTimeLeft[currentPlayerIndex.Value] > TimeSpan.Zero)
                playersTimeLeft[currentPlayerIndex.Value] += Increment;

            if (currentPlayerIndex == null)
            {
                if (clockWise)
                    currentPlayerIndex = requestingPlayerIndex - 1;
                else
                    currentPlayerIndex = requestingPlayerIndex + 1;
            }

            if (clockWise)
                currentPlayerIndex = (currentPlayerIndex + 1) % playersTimeLeft.Count;
            else
                currentPlayerIndex = (currentPlayerIndex - 1) % playersTimeLeft.Count;

            // Skip to next player that has time left, if the next player doesn't have any time
            if (playersTimeLeft[currentPlayerIndex.Value] <= TimeSpan.Zero) 
            {
                if (playersTimeLeft.Any(t => t > TimeSpan.Zero))
                    NextPlayer(currentPlayerIndex.Value);
                else
                {
                    currentPlayerIndex = null;
                    PauseGameTimer();
                    NotifyGameStateChanged();
                    return;
                }
            }
            
            Tick(null);
            
            if (paused)
                ResumeGameTimer();
            else
                NotifyGameStateChanged();
        }

        public void ReverseDirection()
        {
            clockWise = !clockWise;
            NotifyGameStateChanged();
        }

        public void Tick(object? _)
        {
            if (!paused && currentPlayerIndex.HasValue)
            {
                TimeSpan timeSinceLastTick = DateTime.Now - lastTimeTicked;
                playersTimeLeft[currentPlayerIndex.Value] -= timeSinceLastTick;
                lastTimeTicked = DateTime.Now;

                if (playersTimeLeft[currentPlayerIndex.Value] <= TimeSpan.Zero)
                {
                    playersTimeLeft[currentPlayerIndex.Value] = TimeSpan.Zero;
                    NextPlayer(currentPlayerIndex.Value);
                    PauseGameTimer();
                }
                NotifyGameStateChanged();
            }
        }
        
        #endregion

        #region Settings

        private TimeSpan timePerPlayer = DEFAULT_TIME_PER_PLAYER;

        public TimeSpan TimePerPlayer
        {
            get => timePerPlayer;
            set
            {
                timePerPlayer = value;
                NotifySettingsStateChanged();
            }
        }

        private TimeSpan increment = DEFAULT_INCREMENT;

        public TimeSpan Increment
        {
            get => increment;
            set
            {
                increment = value;
                NotifySettingsStateChanged();
            }
        }

        private int numberOfPlayersSetting = DEFAULT_NUMBER_OF_PLAYERS;

        public int NumberOfPlayersSetting
        {
            get => numberOfPlayersSetting;
            set
            {
                numberOfPlayersSetting = value;
                NotifySettingsStateChanged();
            }
        }

        private bool clockWiseInitialGameDirection = INITIAL_CLOCKWISE;

        public bool ClockWiseInitialGameDirection
        {
            get => clockWiseInitialGameDirection;
            set
            {
                clockWiseInitialGameDirection = value;
                NotifySettingsStateChanged();
            }
        }

        private bool keepDirectionBetweenGames = false;

        public bool KeepDirectionBetweenGames
        {
            get => keepDirectionBetweenGames;
            set
            {
                keepDirectionBetweenGames = value;
                NotifySettingsStateChanged();
            }
        }

        #endregion

        #region Events

        private void NotifyGameStateChanged() => OnGameStateChanged?.Invoke();

        private void NotifySettingsStateChanged() => OnSettingsStateChange?.Invoke();

        public event Action? OnGameStateChanged;

        public event Action? OnSettingsStateChange;

        #endregion

    }
}
