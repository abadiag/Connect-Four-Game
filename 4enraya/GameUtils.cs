using System.Diagnostics;

namespace FourConnect
{
    /// <summary>
    /// Static class
    /// Amount of needed methods during game 
    /// </summary>
    static class GameUtils
    {

        public static int GetRandomNumber()
        {
            return new System.Random().Next(0,7);
        }

        /// <summary>
        /// Calculate and put the current Player piece
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        private static void Move(int col, int[,] GamePlayersPosition, int CurrentPlayer)
        {
            int freeLast = GameUtils.GetLastFreePositionRow(col, GamePlayersPosition);

            if (freeLast > -1)
            {
                GamePlayersPosition[col, freeLast] = CurrentPlayer;

                if (GameUtils.IsFinished(col, freeLast, CurrentPlayer, GamePlayersPosition))
                {

                }
            }
        }

        /// <summary>
        /// Reset all values of the game array to 0
        /// 0 = Empty
        /// </summary>
        public static int[,] InitGame()
        {
            int[,] gamePlayersPosition = new int[7, 6];

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    gamePlayersPosition[col, row] = 0;
                }
            }

            return gamePlayersPosition;
        }

        /// <summary>
        /// Calculate last up position avaliable in column selected 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="gamePlayersPosition"></param>
        /// <returns></returns>
        public static int GetLastFreePositionRow(int column, int[,] gamePlayersPosition)
        {
            int row = 0;

            for (row = 0; row < 6; row++)
            {
                if (gamePlayersPosition[column, row] > 0) return row - 1;
            }

            return row - 1;
        }

        /// <summary>
        /// Swap value between 1 and 2
        /// </summary>
        /// <param name="currentPlayer"></param>
        /// <returns></returns>
        public static int SwapPlayer(int currentPlayer)
        {
            if (currentPlayer == 2)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        /// <summary>
        /// Improvement of game table, find 4 equal sequential positions in 4 directions
        /// Left to Right
        /// Up to Down
        /// Left Down to Right Up (diagonal)
        /// Up Left to Right Dawn (diagonal)
        /// </summary>
        /// <param name="colImpr"></param>
        /// <param name="rowImpr"></param>
        /// <param name="currentValue"></param>
        /// <param name="gamePlayersPosition"></param>
        /// <returns></returns>
        public static bool IsFinished(int colImpr, int rowImpr, int currentValue, int[,] gamePlayersPosition)
        {
            //Vertical and horizontal calculation
            if (ImprovementUpToDown(colImpr, currentValue, gamePlayersPosition)) return true;
            if (ImprovementLeftToRight(rowImpr, currentValue, gamePlayersPosition)) return true;

            //Diagonals verification
            if (ImproveDiagUpLeftDownRight(colImpr, rowImpr, currentValue, gamePlayersPosition)) return true;
            if (ImproveDiagDownLeftUpRight(colImpr, rowImpr, currentValue, gamePlayersPosition)) return true;

            return false;
        }

        /// <summary>
        /// Improve up to down positions finding 4 conscutive equal value
        /// </summary>
        /// <param name="colImpr"></param>
        /// <param name="currentValue"></param>
        /// <param name="gamePlayersPosition"></param>
        /// <returns></returns>
        public static bool ImprovementUpToDown(int colImpr, int currentValue, int[,] gamePlayersPosition)
        {
            int row = 0;
            int cellValue = 0;

            //Vertical and horizontal calculation
            //UP and DOWN
            int counter = 0;

            for (row = 0; row < 6; row++)
            {
                cellValue = gamePlayersPosition[colImpr, row];
                if (cellValue == currentValue) { counter++; } else { counter = 0; }
                if (counter > 3) return true;
            }

            return false;
        }

        /// <summary>
        /// Improve left to right positions finding 4 conscutive equal value
        /// </summary>
        /// <param name="rowImpr"></param>
        /// <param name="currentValue"></param>
        /// <param name="gamePlayersPosition"></param>
        /// <returns></returns>
        public static bool ImprovementLeftToRight(int rowImpr, int currentValue, int[,] gamePlayersPosition)
        {
            int col = 0;
            int cellValue = 0;
            int counter = 0;

            for (col = 0; col < 7; col++)
            {
                cellValue = gamePlayersPosition[col, rowImpr];
                if (cellValue == currentValue) { counter++; } else { counter = 0; }
                if (counter > 3) return true;
            }

            //Debug.Print("Right & LEFT " + counter.ToString());
            return false;
        }

        /// <summary>
        /// Improve up to down DIAGONAL positions finding 4 conscutive equal value
        /// </summary>
        /// <param name="colImpr"></param>
        /// <param name="rowImpr"></param>
        /// <param name="currentValue"></param>
        /// <param name="gamePlayersPosition"></param>
        /// <returns></returns>
        public static bool ImproveDiagUpLeftDownRight(int colImpr, int rowImpr, int currentValue, int[,] gamePlayersPosition)
        {
            //Diagonal calculation points
            int width = gamePlayersPosition.GetLength(0) - 1;
            int height = gamePlayersPosition.GetLength(1) - 1;
            System.Windows.Point Pini1, Pfin1;

            //UP LEFT POINT
            Pini1 = new System.Windows.Point(colImpr - (height - (height - rowImpr)), rowImpr - (width - (width - colImpr)));
            if (Pini1.X < 0) Pini1.X = 0;
            if (Pini1.Y < 0) Pini1.Y = 0;
            //DOWN RIGHT POINT
            Pfin1 = new System.Windows.Point(colImpr + (height - rowImpr), rowImpr + (width - colImpr));
            if (Pfin1.X > width) Pfin1.X = width;
            if (Pfin1.Y > height) Pfin1.Y = height;

            //Debug.Print("Up left     X " + Pini1.X + "  Y " + Pini1.Y);
            //Debug.Print("Down right  X " + Pfin1.X + "  Y " + Pfin1.Y);

            //DIAGONAL LEFT UP TO RIGHT DOWN        
            int counter = 0;
            int cellValue = 0;
            int rowRun = (int)Pini1.Y;

            for (int col = (int)Pini1.X; col < width + 1; col++)
            {
                cellValue = gamePlayersPosition[col, rowRun];
                if (cellValue == currentValue) { counter++; } else { counter = 0; }
                if (counter > 3) return true;
                rowRun++;
                if (rowRun > height) break;
            }

            //Debug.Print("DIAGONAL LEFT TO RIGHT UP DOWN " + counter.ToString());
            return false;
        }

        /// <summary>
        /// Improve up to down Diagonal positions finding 4 conscutive equal value
        /// </summary>
        /// <param name="colImpr"></param>
        /// <param name="rowImpr"></param>
        /// <param name="currentValue"></param>
        /// <param name="gamePlayersPosition"></param>
        /// <returns></returns>
        public static bool ImproveDiagDownLeftUpRight(int colImpr, int rowImpr, int currentValue, int[,] gamePlayersPosition)
        {
            //Diagonal calculation points
            int width = gamePlayersPosition.GetLength(0) - 1;
            int height = gamePlayersPosition.GetLength(1) - 1;
            System.Windows.Point Pini2, Pfin2;

            //DOWN LEFT PPOINT
            Pini2 = new System.Windows.Point(colImpr - (height - rowImpr), rowImpr + (width - colImpr));
            if (Pini2.X < 0) Pini2.X = 0;
            if (Pini2.Y > height) Pini2.Y = height;

            //UP RIGHT POINT
            Pfin2 = new System.Windows.Point(colImpr + (height + (height - rowImpr)), rowImpr - (width - colImpr));
            if (Pfin2.X > width) Pfin2.X = width;
            if (Pfin2.Y < 0) Pfin2.Y = 0;

            //Debug.Print("UP RIGHT    X " + Pfin2.X + "  Y " + Pfin2.Y);

            int counter = 0;
            int rowRun = height;
            int cellValue = 0;

            for (int col = (int)Pini2.X; col < width + 1; col++)
            {
                cellValue = gamePlayersPosition[col, rowRun];
                if (cellValue == currentValue) { counter++; } else { counter = 0; }
                if (counter > 3) return true;
                //Debug.Print(col.ToString() + "  " + rowRun.ToString());
                rowRun--;
                if (rowRun < 0) break;
            }

            //Debug.Print("DIAGONAL LEFT TO RIGHT DOWN UP " + counter.ToString());
            return false;
        }

        /// <summary>
        /// Up to down positions conscutive equal value
        /// </summary>
        /// <param name="colImpr"></param>
        /// <param name="currentPlayer"></param>
        /// <param name="gamePlayersPosition"></param>
        /// <returns></returns>
        public static double GetTaxWinUpToDown(int colImpr, int rowImpr, int currentPlayer, int[,] gamePlayersPosition)
        {
            int row = 0;
            bool triggerMyCell = false;
            int emptyCellCount = 0;
            int PlayerCellCount = 0;
            int indexCellOk = 1;
            double indexEmptyCell = 0.5;

            for (row = 0; row < 6; row++)
            {

                int cellValue = gamePlayersPosition[colImpr, row];

                if (rowImpr == row) triggerMyCell = true;

                if (cellValue == 0)
                {
                    emptyCellCount++;
                }

                if (cellValue == currentPlayer)
                {
                    PlayerCellCount++;
                }

                if (PlayerCellCount > 3) return 2000;

                if (cellValue != currentPlayer && cellValue != 0 && triggerMyCell)
                {
                   
                    return (emptyCellCount * indexEmptyCell) + (PlayerCellCount * indexCellOk);
                }
            }

            return (emptyCellCount + PlayerCellCount > 3)? 
                (emptyCellCount * indexEmptyCell) + (PlayerCellCount * indexCellOk):0;
        }

        /// <summary>
        /// Calculate index Tax of win game of one move
        /// </summary>
        /// <param name="colImpr"></param>
        /// <param name="rowImpr"></param>
        /// <param name="Player"></param>
        /// <param name="gamePlayersPosition"></param>
        /// <returns></returns>
        public static double GetTaxWinLeftToRight(int colImpr, int rowImpr, int Player, int[,] gamePlayersPosition)
        {
            bool triggerMyCell = false;
            int emptyCellCount = 0;
            int PlayerCellCount = 0;
            int indexCellOk = 1;
            double indexEmptyCell = 0.7;

            Debug.Print(rowImpr.ToString());

            for (int col = 0; col < gamePlayersPosition.GetLength(0); col++)
            {
                int cellValue = gamePlayersPosition[col, rowImpr];

                if (col == colImpr) triggerMyCell = true;

                if (cellValue == Player) PlayerCellCount++;
                if (cellValue == 0) emptyCellCount++;
                if (PlayerCellCount > 3) return 2000;
                if (cellValue != Player && triggerMyCell)
                {
                    
                    return (emptyCellCount * indexEmptyCell )+ (PlayerCellCount * indexCellOk);
                }

                if (cellValue != Player)
                {
                    emptyCellCount = 0;
                    PlayerCellCount = 0;
                }
            }

            return (emptyCellCount + PlayerCellCount > 3) ?
                (emptyCellCount * indexEmptyCell) + (PlayerCellCount * indexCellOk) : 0;
        }

        /// <summary>
        /// Improve up to down DIAGONAL positions finding 4 conscutive equal value
        /// </summary>
        /// <param name="colImpr"></param>
        /// <param name="rowImpr"></param>
        /// <param name="currentValue"></param>
        /// <param name="gamePlayersPosition"></param>
        /// <returns></returns>
        public static double GetTaxWinDiagUpLeDR(int colImpr, int rowImpr, int currentValue, int[,] gamePlayersPosition)
        {
            int indexCellOk = 1;
            double indexEmptyCell = 0.5;

            //Diagonal calculation points
            int width = gamePlayersPosition.GetLength(0) - 1;
            int height = gamePlayersPosition.GetLength(1) - 1;
            System.Windows.Point Pini1, Pfin1;

            //UP LEFT POINT
            Pini1 = new System.Windows.Point(colImpr - (height - (height - rowImpr)), rowImpr - (width - (width - colImpr)));
            if (Pini1.X < 0) Pini1.X = 0;
            if (Pini1.Y < 0) Pini1.Y = 0;
            //DOWN RIGHT POINT
            Pfin1 = new System.Windows.Point(colImpr + (height - rowImpr), rowImpr + (width - colImpr));
            if (Pfin1.X > width) Pfin1.X = width;
            if (Pfin1.Y > height) Pfin1.Y = height;

            //DIAGONAL LEFT UP TO RIGHT DOWN        
            int counter = 0;
            int cellValue = 0;
            int rowRun = (int)Pini1.Y;
            int emptyCell = 0;

            for (int col = (int)Pini1.X; col < width + 1; col++)
            {
                Debug.Print("Player" + currentValue + " counter cells " + counter.ToString() + " empty count " + emptyCell.ToString());

                cellValue = gamePlayersPosition[col, rowRun];

                if (cellValue == currentValue)
                {
                    counter++;
                    if (counter > 3) return 2000;
                }

                if (cellValue == 0) emptyCell++;

                if ((cellValue != currentValue) && cellValue != 0)
                {
                    if (counter + emptyCell < 4) return 0;
                    return counter;
                }

                rowRun++;

                if (rowRun > height) return (counter * indexCellOk) + (emptyCell * indexEmptyCell);            
            }

            return counter;            
        }

        /// <summary>
        /// Up to down Diagonal positions conscutive equal value
        /// </summary>
        /// <param name="colImpr"></param>
        /// <param name="rowImpr"></param>
        /// <param name="currentValue"></param>
        /// <param name="gamePlayersPosition"></param>
        /// <returns></returns>
        public static double GetTaxWinDiagUrightoDownL(int colImpr, int rowImpr, int currentValue, int[,] gamePlayersPosition)
        {
            int indexCellOk = 1;
            double indexEmptyCell = 0.5;

            //Diagonal calculation points
            int width = gamePlayersPosition.GetLength(0) - 1;
            int height = gamePlayersPosition.GetLength(1) - 1;
            System.Windows.Point Pini2, Pfin2;

            //DOWN LEFT PPOINT
            Pini2 = new System.Windows.Point(colImpr - (height - rowImpr), rowImpr + (width - colImpr));
            if (Pini2.X < 0) Pini2.X = 0;
            if (Pini2.Y > height) Pini2.Y = height;

            //UP RIGHT POINT
            Pfin2 = new System.Windows.Point(colImpr + (height + (height - rowImpr)), rowImpr - (width - colImpr));
            if (Pfin2.X > width) Pfin2.X = width;
            if (Pfin2.Y < 0) Pfin2.Y = 0;

            //Debug.Print("UP RIGHT    X " + Pfin2.X + "  Y " + Pfin2.Y);

            int counter = 0;
            int rowHighR = (int)Pfin2.Y;
            int cellValue = 0;
            int emptyCell = 0;

            for (int col = (int)Pfin2.X; col > -1; col--)
            {
                cellValue = gamePlayersPosition[col, rowHighR];

                if (cellValue == currentValue)
                {
                    counter++;
                    if (counter > 3) return 2000;
                }

                if (cellValue == 0) emptyCell++;

                rowHighR++;

                if (rowHighR > height || (cellValue != currentValue && cellValue != 0))
                {
                    if (counter + emptyCell < 4) return 0;
                    return (counter * indexCellOk) + (emptyCell * indexEmptyCell);
                }
            }

            return counter;
        }

        /// <summary>
        /// Max integer of two int
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int GetMaxInt(int a, int b)
        {
            return (a > b) ? a : b;
        }

        /// <summary>
        /// Summatory of 4 vars
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int GetSumWinTax(int a, int b, int c, int d)
        {
            return a + b + c + d;
        }

        /// <summary>
        /// Colums holder of max value
        /// </summary>
        /// <param name="col1"></param>
        /// <param name="int1"></param>
        /// <param name="col2"></param>
        /// <param name="int2"></param>
        /// <returns></returns>
        public static int GetColumnMaxValue(int col1, int int1, int col2, int int2)
        {
            return (int1 > int2) ? col1 : col2;

        }

        /// <summary>
        /// Get column number with highes value
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int GetColumnWithMaxValue(double[]array)
        {
            double maxValueColumn = 0;
            int result = 0;

            for (int col = 0; col < array.Length; col++)
            {
                if (array[col] > maxValueColumn)
                {
                    maxValueColumn = array[col];
                    result = col;
                }
            }
            //Debug.Print(result.ToString());
            return result;
        }

        /// <summary>
        /// Get Max value number of row
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static double GetMaxValueColumn(double[] array)
        {
            double maxValueColumn = 0;

            for (int col = 0; col < array.Length; col++)
            {
                if (array[col] > maxValueColumn)
                {
                    maxValueColumn = array[col];
                   
                }
            }
            //Debug.Print(result.ToString());
            return maxValueColumn;
        }

        /// <summary>
        /// Get average double of array
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static double GetAverageArray(double[]array)
        {
            double AverageArray = 0;
            foreach(double value in array)
            {
                AverageArray = AverageArray + value;
            }

            return AverageArray / array.Length;
        }

        /// <summary>
        /// Print array of win tax
        /// </summary>
        /// <param name="array"></param>
        public static void PrintArray(double[]array)
        {
            string result = "";

            foreach(int x in array)
            {
                result = result + " winTax: " + x.ToString();
            }

            Debug.Print(result);
        }
    }
}