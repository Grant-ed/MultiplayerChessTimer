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
    }
}
