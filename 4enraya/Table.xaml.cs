﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace FourConnect
{
    /// <summary>
    /// Lógica de interacción para Table.xaml
    /// </summary>
    public partial class Table : UserControl
    {
        public int[,] gamePlayersPosition { get; set; } = new int[7, 6];
        public int currentPlayer { get; set; } = 1;

        private BitmapImage[] pieces;

        public Table()
        {
            InitializeComponent();
            InitialLoadImages();
            InitGame();
            RefreshGameTableView();
        }

        /// <summary>
        /// Refresh all positions in table according the two dimension(6,7) integer game array
        /// 0 = empty
        /// 1 = player1 --> red circle
        /// 2 = player2 --> green circle
        /// </summary>
        public void RefreshGameTableView()
        {
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    var child = (System.Windows.Controls.Image)gameTableGrid.Children
                                    .Cast<System.Windows.Controls.Image>()
                                    .Where(e => Grid.GetRow(e).Equals(row) && Grid.GetColumn(e).Equals(col)).FirstOrDefault();

                    child.Source = pieces[0];

                    switch (gamePlayersPosition[col, row])
                    {
                        case 0:
                            child.Source = pieces[0];
                            break;
                        case 1:
                            child.Source = pieces[1];
                            break;
                        case 2:
                            child.Source = pieces[2];
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Reset all values of the game array to 0
        /// 0 = Empty
        /// </summary>
        private void InitGame()
        {
            gamePlayersPosition = GameUtils.InitGame();
        }

        /// <summary>
        /// Load of images to array associated index of array/player
        /// </summary>
        private void InitialLoadImages()
        {
            pieces = new BitmapImage[3];
            pieces[0] = new BitmapImage(new Uri(@"C:\Users\Albert\source\repos\4enraya\4enraya\Resources\Blank.png"));
            pieces[1] = new BitmapImage(new Uri(@"C:\Users\Albert\source\repos\4enraya\4enraya\Resources\Red.png"));
            pieces[2] = new BitmapImage(new Uri(@"C:\Users\Albert\source\repos\4enraya\4enraya\Resources\Green.png", UriKind.Absolute));
        }

        /// <summary>
        /// Mose Event Raised in table returns column clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameTableGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = Mouse.GetPosition(gameTableGrid);

            int row = 0;
            int col = 0;
            double accumulatedHeight = 0.0;
            double accumulatedWidth = 0.0;

            // calc row mouse was over
            foreach (var rowDefinition in gameTableGrid.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                    break;
                row++;
            }

            // calc col mouse was over
            foreach (var columnDefinition in gameTableGrid.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                col++;
            }

            Move(col, row);
        }

        /// <summary>
        /// Calculate and put the current Player piece
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        private void Move(int col, int row)
        {
            int freeLast = GameUtils.GetLastFreePositionColumn(col, gamePlayersPosition);

            if (freeLast > -1)
            {
                gamePlayersPosition[col, freeLast] = currentPlayer;

                RefreshGameTableView();

                if (GameUtils.IsFinished(gamePlayersPosition, col, freeLast, currentPlayer, gamePlayersPosition))
                {
                    MessageBox.Show("Finished Player" + currentPlayer.ToString() +
                                    " wins", "Connect four", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }

                currentPlayer = GameUtils.SwapPlayer(currentPlayer);
            }
        }
    }
}