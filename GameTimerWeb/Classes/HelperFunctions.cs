namespace GameTimerWeb
{
    public static class HelperFunctions
    {
        // Player num is 0 indexed, and returns row col, where first player is top right, going clockwise
        private static (int row, int col) RowColFromPlayerNum(int playerNum, int numPlayers)
        {
            int row;
            int col;
            
            // Using int division
            if (playerNum < numPlayers / 2)
            {
                // Second column, going down
                col = 1;
                row = playerNum;
            }
            else
            {
                // First column, going up
                col = 0;
                row = numPlayers - 1 - playerNum;
            }
            
            return (row, col);
        }

        private static int PlayerNumFromRowCol(int row, int col, int numPlayers)
        {
            if (col == 0)
            {
                // First column, going up
                return numPlayers - 1 - row;
            }
            else
            {
                // Second column, going down
                return row;
            }
        }

        /// <summary>
        /// Grid item is 0 indexed, where first item is top left, going across then down
        /// </summary>
        private static int GridItemIndexFromRowCol(int row, int col)
        {
            return row * 2 + col;
        }

        private static (int row, int col) RowColFromGridItemIndex(int gridItemIndex)
        {
            int row = gridItemIndex / 2;
            int col = gridItemIndex % 2;
            return (row, col);
        }

        public static int PlayerNumFromGridItemIndex(int gridItemIndex, int numPlayers)
        {
            (int row, int col) = RowColFromGridItemIndex(gridItemIndex);
            return PlayerNumFromRowCol(row, col, numPlayers);
        }

        public static int GetNumRows(GameTimerState StateContainer)
        {
            int numPlayerRows = (int)Math.Ceiling(((double)StateContainer.CurrentNumberOfPlayers) / 2);
            if (StateContainer.BigReverseButton)
                return numPlayerRows + 1;
            else
                return numPlayerRows;
        }

        public static string GetButtonHeight(GameTimerState StateContainer)
        {
            return $"{82M / GetNumRows(StateContainer)}vh";
        }

        public static string GetButtonTextSize(GameTimerState StateContainer)
        {
            return $"{82M / GetNumRows(StateContainer) / 4M}vh";
        }

        public static string GetButtonMargin(GameTimerState StateContainer)
        {
            return $"{5M / GetNumRows(StateContainer)}vh";
        }

        public static string GetButtonPaperStyle(GameTimerState StateContainer, int? TimerIndex)
        {
            string border = "";
            string margin = GetButtonMargin(StateContainer);
            if (TimerIndex != null && StateContainer.EnableColours)
            {
                var playerColour = StateContainer.GetPlayerColour(TimerIndex.Value);
                border = $"border: 2px solid #{playerColour.R:X2}{playerColour.G:X2}{playerColour.B:X2};";
            }
            return $"margin-top:{margin}; margin-bottom:{margin}; {border}";
        }

        public static string GetButtonStyle(GameTimerState StateContainer)
        {
            return $"height:100%; font-size:{GetButtonTextSize(StateContainer)} !important;";
        }

        public static string GetButtonClass(GameTimerState StateContainer, int? TimerIndex)
        {
            string rotateClass = string.Empty;
            if (TimerIndex != null)
            {
                bool isRightColumn = (double)TimerIndex < ((double)StateContainer.CurrentNumberOfPlayers) / 2;
                rotateClass = isRightColumn ? "btn-txt-rotate-270" : "btn-txt-rotate-90";
            }
            return $"mud-width-full {rotateClass}";
        }

        public static MudBlazor.Color GetDirectionIconColor(GameTimerState StateContainer)
        {
            return StateContainer.ClockWise ? MudBlazor.Color.Tertiary : MudBlazor.Color.Info;
        }

        public static string GetDirectionStyle(GameTimerState StateContainer)
        {
            int multiplier = StateContainer.ClockWise ? 1 : -1;
            return $"transform: scaleX({multiplier});";
        }
    }
}
