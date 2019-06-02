using System.Diagnostics;
using System.Windows;

namespace IA
{
    class IA
    {
        public delegate void FinishMoveHandler(int newxtPlayer);
        public event FinishMoveHandler OnFinishMovement;

        private const int Tarjet = 4;
        public int[,] GamePlayersPosition { get; set; }
        public int NextMovement { get; private set; }
        private FourConnect.Table GameTable = null;

        public IA(FourConnect.Table GameTable)
        {
            this.GameTable = GameTable;
        }

        /// <summary>
        /// Getter of current positions table
        /// </summary>
        /// <returns></returns>
        private int[,] GetCurrentGameTable()
        {
            return GameTable.GamePlayersPosition;
        }

        /// <summary>
        /// Event raised when Human Move is done
        /// </summary>
        /// <param name="gamePlayersTable"></param>
        /// <param name="MePlayer"></param>
        public void MakeMoveHandler(int[,]gamePlayersTable , int MePlayer)
        {         
            MoveImprovement(MePlayer, gamePlayersTable);
        }

        private void MoveImprovement(int iAPlayer, int[,]gamePlayersTable)
        {
            GamePlayersPosition = gamePlayersTable;
            int[,] winTaxByColumn = new int[GamePlayersPosition.GetLength(0), 2];
            int freeLastRow = 0;

            //Debug.Print("Player------------> " + iAPlayer);

            for (int col = 0; col < GamePlayersPosition.GetLength(0) ; col++)
            {
                freeLastRow = FourConnect.GameUtils.GetLastFreePositionRow(col, GamePlayersPosition);

                if (freeLastRow > 0)
                {
                    GamePlayersPosition[col, freeLastRow] = iAPlayer;

                    int upDown = FourConnect.GameUtils.GetEqualsUpDown(col, iAPlayer, gamePlayersTable);
                    int leftRight = FourConnect.GameUtils.GetEqualsLeftRight(freeLastRow, iAPlayer, GamePlayersPosition);
                    int diagUpDown = FourConnect.GameUtils.GetEqualsDiagUpLeftDownRight(col, freeLastRow, iAPlayer, GamePlayersPosition);
                    int diagDownLeft = FourConnect.GameUtils.GetEqualsDiagUpLeftDownRight(col, freeLastRow, iAPlayer, GamePlayersPosition);

                    upDown = (freeLastRow + upDown > 3) ? upDown : 0;
                    //leftRight = (freeLastRow + leftRight > 3) ? leftRight : 0;
                    diagUpDown = (freeLastRow + diagUpDown > 3) ? diagUpDown : 0;
                    diagDownLeft = (freeLastRow + diagDownLeft > 3) ? diagDownLeft : 0;

                    int ColumntaxWinTax = FourConnect.GameUtils.GetMaxInt(upDown, FourConnect.GameUtils.
                        GetMaxInt(leftRight, FourConnect.GameUtils.
                        GetMaxInt(diagUpDown, diagDownLeft)));

                    int TotalColumnTaxWin = FourConnect.GameUtils.GetSumWinTax(upDown, leftRight, diagUpDown, diagDownLeft);

                    winTaxByColumn[col, 0] = ColumntaxWinTax;
                    winTaxByColumn[col, 1] = TotalColumnTaxWin;

                    GamePlayersPosition[col, freeLastRow] = 0;

                    //Debug.Print("Column " + col.ToString() + " Up down:          " + upDown.ToString() +     " LeftRight:       " +leftRight.ToString());
                    //Debug.Print("Column " + col.ToString() + " Diagonal Up down: " + diagUpDown.ToString() + " Diagonal DownUp: " + diagDownLeft.ToString());
                }
            }

            Move(GetColumnMaxWinTax(winTaxByColumn));

            OnFinishMovement(FourConnect.GameUtils.SwapPlayer(iAPlayer));
        }

        private void Move(int column)
        {
            NextMovement = column;
        }

        private int GetColumnMaxWinTax(int[,]array)
        {
            int MaxTax = 0;
            int columnMaxTax = 0;
            for( int x = 0; x < array.GetLength(0); x++)
            {
                Debug.Print("Column " + x + " Max tax " + x + " -> " + array[x,0].ToString());
                Debug.Print("         Sum tax " + x + " -> " + array[x, 1].ToString());

                if (array[x, 0] >= MaxTax)
                {
                    if (array[x, 0] == MaxTax)
                    {
                        if (FourConnect.GameUtils.GetMaxInt(array[x, 1], array[columnMaxTax, 1]) > MaxTax)
                        {
                            //MaxTax = array[columnMaxTax, 1];
                            columnMaxTax = FourConnect.GameUtils.GetColumnMaxValue(x, array[x, 1], columnMaxTax, array[columnMaxTax, 1]);
                        }
                        columnMaxTax = x;
                        MaxTax = array[x, 0];
                    }
                    else
                    {
                        MaxTax = array[x, 0];
                        columnMaxTax = x;
                    }
                }
            }
            return columnMaxTax;
        }
    }
}
