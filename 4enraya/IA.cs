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
            int column = MoveImprovement(MePlayer, gamePlayersTable);
            Move(column, MePlayer);
        }

        /// <summary>
        /// Perform all movements and calculate win tax in every case, finally returns the column
        /// with major winTax
        /// </summary>
        /// <param name="Player"></param>
        /// <param name="gamePlayersTable"></param>
        /// <returns></returns>
        private int MoveImprovement(int Player, int[,]gamePlayersTable)
        {
            GamePlayersPosition = gamePlayersTable;
            double[] winTaxByColumn = new double[GamePlayersPosition.GetLength(0)];
            int freeLastRow = 0;

            for (int col = 0; col < GamePlayersPosition.GetLength(0) ; col++)
            {
                freeLastRow = FourConnect.GameUtils.GetLastFreePositionRow(col, GamePlayersPosition);

                if (freeLastRow > 0)
                {
                    GamePlayersPosition[col, freeLastRow] = Player;

                    double upDown = FourConnect.GameUtils.GetEqualsUpDown(freeLastRow, Player, GamePlayersPosition);
                    double leftRight = FourConnect.GameUtils.GetEqualsLeftRight(freeLastRow, Player, GamePlayersPosition);
                    double diagUpDown = FourConnect.GameUtils.GetEqualsDiagUpLeftDownRight(col, freeLastRow, Player, GamePlayersPosition);
                    double diagDownLeft = FourConnect.GameUtils.GetEqualDiagDownLeftUpRight(col, freeLastRow, Player, GamePlayersPosition);

                    double ColumntaxWinTax = upDown + leftRight + diagUpDown + diagDownLeft;

                    winTaxByColumn[col] = ColumntaxWinTax;

                    GamePlayersPosition[col, freeLastRow] = 0;
                }
            }

            FourConnect.GameUtils.GetAverageArray(winTaxByColumn);
            FourConnect.GameUtils.PrintArray(winTaxByColumn);

            int column = FourConnect.GameUtils.GetColumnMaxValue(winTaxByColumn);

            return column;         
        }

        /// <summary>
        /// Perform the movement and raise event
        /// </summary>
        /// <param name="column"></param>
        /// <param name="iAPlayer"></param>
        private void Move(int column, int iAPlayer)
        {
            NextMovement = column;
            OnFinishMovement(FourConnect.GameUtils.SwapPlayer(iAPlayer));
        }
    }
}
