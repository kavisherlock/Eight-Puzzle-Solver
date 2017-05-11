using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EightPuzzleSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[,] board = new int[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } };

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void onezero_Click(object sender, RoutedEventArgs e)
        {
            swapWithtEmptyNeighbor(1, 0);
        }

        private void oneone_Click(object sender, RoutedEventArgs e)
        {
            swapWithtEmptyNeighbor(1, 1);
        }

        private void onetwo_Click(object sender, RoutedEventArgs e)
        {
            swapWithtEmptyNeighbor(1, 2);
        }

        private void twozero_Click(object sender, RoutedEventArgs e)
        {
            swapWithtEmptyNeighbor(2, 0);
        }

        private void twoone_Click(object sender, RoutedEventArgs e)
        {
            swapWithtEmptyNeighbor(2, 1);
        }

        private void twotwo_Click(object sender, RoutedEventArgs e)
        {
            swapWithtEmptyNeighbor(2, 2);
        }

        private void threezero_Click(object sender, RoutedEventArgs e)
        {
            swapWithtEmptyNeighbor(3, 0);
        }

        private void threeone_Click(object sender, RoutedEventArgs e)
        {
            swapWithtEmptyNeighbor(3, 1);
        }

        private void threetwo_Click(object sender, RoutedEventArgs e)
        {
            swapWithtEmptyNeighbor(3, 2);
        }

        private void swapWithtEmptyNeighbor(int rowNum, int colNum)
        {
            Button currentButton = getButton(rowNum, colNum) as Button;
            Button topElement = getButton(rowNum - 1, colNum) as Button;
            Button leftElement = getButton(rowNum, colNum - 1) as Button;
            Button bottomElement = getButton(rowNum + 1, colNum) as Button;
            Button rightElement = getButton(rowNum, colNum + 1) as Button;

            trySwapButtonContents(currentButton, topElement);
            trySwapButtonContents(currentButton, leftElement);
            trySwapButtonContents(currentButton, bottomElement);
            trySwapButtonContents(currentButton, rightElement);
        }

        private void trySwapButtonContents(Button button1, Button button2)
        {
            if (button1 == null || button2 == null)
            {
                return;
            }

            if (0 == String.Compare(button2.Content.ToString(), ""))
            {
                button2.Content = button1.Content;
                button1.Content = "";
            }
        }

        private UIElement getButton(int rowNum, int colNum)
        {
            if (rowNum < 1 || colNum < 0 || rowNum > 3 || colNum > 2)
            {
                return null;
            }

            return GameGrid.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == rowNum && Grid.GetColumn(i) == colNum);
        }
    }
}
