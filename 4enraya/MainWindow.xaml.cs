using System.Windows;

namespace FourConnect
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Table mainBoard = new Table();

        public MainWindow()
        {
            InitializeComponent();
            AddMainBoard();
        }

        private void AddMainBoard()
        {
            main.Children.Add(mainBoard);
        }
    }
}
