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
        private bool firstMove = true;

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
            if (firstMove)
            {
                Move(FourConnect.GameUtils.GetRandomNumber(), MePlayer);
                firstMove = false;
            }
            else
            {
                AttakOrdefending(gamePlayersTable, MePlayer);
            }
           
        }

        /// <summary>
        /// Improve IaPlayer and Human player mevements, 
        /// get the max tax win and column to defend 
        /// true indicate Attak
        /// </summary>
        /// <returns></returns>
        private void AttakOrdefending(int[,] gamePlayersTable, int MePlayer)
        {
            double[] dataIA = GetMaxTaxColumn(MePlayer, gamePlayersTable);
            double[] dataHuman = GetMaxTaxColumn(FourConnect.GameUtils.SwapPlayer(MePlayer), gamePlayersTable);

            if (dataIA[1] > dataHuman[1])
            {
                //Attack
                Move((int)dataIA[0], MePlayer);
                Debug.Print("Attak: IA wintax " + dataIA[1] + "  Human wintax " + dataHuman[1]);
            }
            else
            {
                //Defending
                Move((int)dataHuman[0], MePlayer);
                Debug.Print("Defend: Human wintax " + dataHuman[1] + "  IA wintax " + dataIA[1]);
            }

        }

        /// <summary>
        /// Perform all movements and calculate win tax in every case, finally returns the column
        /// with major winTax and Wintax of this column
        /// </summary>
        /// <param name="Player"></param>
        /// <param name="gamePlayersTable"></param>
        /// <returns></returns>
        private double[]GetMaxTaxColumn(int Player, int[,]gamePlayersTable)
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

                    double upDown = FourConnect.GameUtils.GetTaxWinUpToDown(col, freeLastRow, Player, GamePlayersPosition);
                    double leftRight = FourConnect.GameUtils.GetTaxWinLeftToRight(col, freeLastRow, Player, GamePlayersPosition);
                    double diagUpDown = FourConnect.GameUtils.GetTaxWinDiagUpLeDR(col, freeLastRow, Player, GamePlayersPosition);
                    double diagDownLeft = FourConnect.GameUtils.GetTaxWinDiagUrightoDownL(col, freeLastRow, Player, GamePlayersPosition);

                    double ColumntaxWinTax = upDown + leftRight + ((diagUpDown + diagDownLeft));
                    
                    winTaxByColumn[col] = ColumntaxWinTax;

                    GamePlayersPosition[col, freeLastRow] = 0;
                }
            }
            
            double average = FourConnect.GameUtils.GetAverageArray(winTaxByColumn);
            double maxValue = FourConnect.GameUtils.GetMaxValueColumn(winTaxByColumn);
            int column = FourConnect.GameUtils.GetColumnWithMaxValue(winTaxByColumn);

            return new double[] { column, maxValue + average };         
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
