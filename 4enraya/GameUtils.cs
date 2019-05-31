﻿using System.Diagnostics;

namespace FourConnect
{
    /// <summary>
    /// Static class
    /// Amount of needed methods during game 
    /// </summary>
    static class GameUtils
    {
        /// <summary>
        /// Reset all values of the game array to 0
        /// 0 = Empty
        /// </summary>
        public static int[,] InitGame()
        {
            int [,]gamePlayersPosition = new int[7, 6];

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
        public static int GetLastFreePositionColumn(int column, int[,] gamePlayersPosition)
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
        /// <param name="currentPositions"></param>
        /// <param name="colImpr"></param>
        /// <param name="rowImpr"></param>
        /// <param name="currentValue"></param>
        /// <param name="gamePlayersPosition"></param>
        /// <returns></returns>
        public static bool IsFinished(int[,] currentPositions, int colImpr, int rowImpr, int currentValue, int[,] gamePlayersPosition)
        {
            int row = 0;
            int col = 0;
            int cellValue = 0;

            Debug.Print("Player" + currentValue);

            //Vertical and horizontal calculation
            //UP and DOWN
            int counter = 0;

            for (row = 0; row < 6; row++)
            {
                cellValue = gamePlayersPosition[colImpr, row];
                if (cellValue == currentValue) { counter++; } else { counter = 0; }
                if (counter > 3) return true;
            }
            Debug.Print("UP & DOWN " + counter.ToString());

            //RIGHT to LEFT
            counter = 0;

            for (col = 0; col < 7; col++)
            {
                cellValue = gamePlayersPosition[col, rowImpr];
                if (cellValue == currentValue) { counter++; } else { counter = 0; }
                if (counter > 3) return true;
            }
            Debug.Print("Right & LEFT " + counter.ToString());

            //Diagonal calculation points
            int width = gamePlayersPosition.GetLength(0) - 1;
            int height = gamePlayersPosition.GetLength(1) - 1;
            System.Windows.Point Pini1, Pfin1, Pini2, Pfin2;

            //UP LEFT POINT
            Pini1 = new System.Windows.Point(colImpr - (height - (height - rowImpr)), rowImpr - (width - (width - colImpr)));
            if (Pini1.X < 0) Pini1.X = 0;
            if (Pini1.Y < 0) Pini1.Y = 0;
            //DOWN RIGHT POINT
            Pfin1 = new System.Windows.Point(colImpr + (height - rowImpr), rowImpr + (width - colImpr));
            if (Pfin1.X > width) Pfin1.X = width;
            if (Pfin1.Y > height) Pfin1.Y = height;

            Debug.Print("Up left     X " + Pini1.X + "  Y " + Pini1.Y);
            Debug.Print("Down right  X " + Pfin1.X + "  Y " + Pfin1.Y);
            
            //DOWN LEFT PPOINT
            Pini2 = new System.Windows.Point(colImpr - (height - rowImpr), rowImpr + (width - colImpr));
            if (Pini2.X < 0) Pini2.X = 0;
            if (Pini2.Y > height) Pini2.Y = height;

            //UP RIGHT POINT
            Pfin2 = new System.Windows.Point(colImpr + (height + (height - rowImpr)), rowImpr - (width - colImpr));
            if (Pfin2.X > width) Pfin2.X = width;
            if (Pfin2.Y < 0) Pfin2.Y = 0;

            Debug.Print("DOWN LEFT   X " + Pfin1.X + "  Y " + Pfin1.Y);
            Debug.Print("UP RIGHT    X " + Pfin2.X + "  Y " + Pfin2.Y);

            //DIAGONAL LEFT UP TO RIGHT DOWN        
            counter = 0;
            int rowRun = (int)Pini1.Y;

            for (col = (int)Pini1.X; col < width + 1; col++)
            {
                cellValue = gamePlayersPosition[col, rowRun];
                if (cellValue == currentValue) { counter++; } else { counter = 0; }
                if (counter > 3) return true;
                rowRun++;
                if (rowRun > height) break;
            }
            Debug.Print("DIAGONAL LEFT TO RIGHT UP DOWN " + counter.ToString());

            //DIAGONAL LEFT DOWN TO RIGHT UP        
            counter = 0;
            rowRun = height;

            for (col = (int)Pini2.X; col < width + 1; col++)
            {
                cellValue = gamePlayersPosition[col, rowRun];
                if (cellValue == currentValue) { counter++; } else { counter = 0; }
                if (counter > 3) return true;
                Debug.Print(col.ToString() + "  " + rowRun.ToString());
                rowRun--;
                if (rowRun < 0) break;
            }

            Debug.Print("DIAGONAL LEFT TO RIGHT DOWN UP " + counter.ToString());

            return false;
        }
    }
}