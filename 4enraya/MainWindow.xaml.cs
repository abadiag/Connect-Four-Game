using System;
using System.Windows;

namespace FourConnect
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Table mainBoard = new Table();
        IA.IA iAClass;

        public MainWindow()
        {
            InitializeComponent();

            iAClass = new IA.IA(mainBoard);
            mainBoard.OnFinishMovement += OnHumanMovePerformed;
            iAClass.OnFinishMovement += OnIAMovementPerformed;
            AddMainBoard();
        }

        private void OnIAMovementPerformed(int nextPlayer)
        {
            mainBoard.Move(iAClass.NextMovement, false);
            mainBoard.CurrentPlayer = nextPlayer;
            //iAClass.MakeMoveHandler(iAClass.GamePlayersPosition, nextPlayer);
        }

        private void OnHumanMovePerformed(int[,] GamePlayersPosition, FourConnect.MoveEventargs moveEventargs)
        {
            iAClass.GamePlayersPosition = GamePlayersPosition;
            iAClass.MakeMoveHandler(GamePlayersPosition, moveEventargs.NextPlayer);
        }

        private void AddMainBoard()
        {
            main.Children.Add(mainBoard);
        }
    }
}
