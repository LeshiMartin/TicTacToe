using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;

namespace TicTacToe.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void InputElement_OnTapped(object? sender, TappedEventArgs e)
    {
        if (sender is Button button)
        {
            button.Content = new TextBlock()
            {
                Text = "X",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                
            };
        }
    }
}