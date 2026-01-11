using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TicTacToe.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<TicTacToeTile> Tiles { get; set; } = new();

    public string Greeting { get; } = "Hello to Avalonia!";
    [ObservableProperty] private string _winner = string.Empty;
    [ObservableProperty] private int _pressedTimes;

    public int Row { get; set; }
    public int Column { get; set; }

    public MainWindowViewModel()
    {
        const int size = 150;
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                Tiles.Add(new TicTacToeTile()
                {
                    Row = i,
                    Column = j,
                    Margin = new Thickness(size * j + 5, size * i + 5, 0, 0),
                });
            }
        }
    }

    [RelayCommand]
    private void Press(TicTacToeTile tile)
    {
        if (!string.IsNullOrEmpty(Winner))
            return;
        tile.Text = "X";
        tile.IsSelected = true;
        Winner = CheckForWinner();
        if (!string.IsNullOrEmpty(Winner))
            return;
        RandomSelection();
        Winner = CheckForWinner();
    }

    [RelayCommand]
    private void Restart()
    {
        Winner = string.Empty;
        foreach (var tile in Tiles)
        {
            tile.IsSelected = false;
            tile.Text = string.Empty;
        }
    }

    private void RandomSelection()
    {
        var unselected = Tiles.Where(x => !x.IsSelected).ToList();
        var rnd = Random.Shared.Next(0, unselected.Count);
        unselected[rnd].IsSelected = true;
        unselected[rnd].Text = "O";
    }

    private string CheckForWinner()
    {
        var xSelected = Tiles.Where(x => x.Text.Equals("x", StringComparison.InvariantCultureIgnoreCase)).ToList();
        if (CheckForWinner(xSelected))
        {
            return "User";
        }

        var ySelected = Tiles.Where(x => x.Text.Equals("y", StringComparison.InvariantCultureIgnoreCase)).ToList();
        return CheckForWinner(ySelected) ? "Computer" : string.Empty;
    }

    private bool CheckForWinner(IReadOnlyList<TicTacToeTile> tiles)
    {
        if (tiles.Count < 3)
            return false;

        var rowGrouped = tiles.GroupBy(x => x.Row);
        if (rowGrouped.Any(x => x.Count() == 3))
            return true;
        var columnGrouped = tiles.GroupBy(x => x.Column);
        if (columnGrouped.Any(x => x.Count() == 3))
            return true;
        if (!tiles.Any(x => x is { Row: 0, Column: not (0 and 2) }))
            return false;
        if (!tiles.Any(x => x is { Row: 2, Column: not (0 and 2) }))
            return false;
        if (tiles.Any(x => x is { Row: 1, Column: not 1 }))
            return false;
        return tiles.Any(x => x is { Row: 0, Column: 0 }) ? tiles.Any(x => x is { Row: 2, Column: 2 }) : tiles.Any(x => x is { Row: 2, Column: 0 });
    }
}

public partial class TicTacToeTile : ObservableObject
{
    [ObservableProperty] private int _row;
    [ObservableProperty] private int _column;
    [ObservableProperty] private Thickness _margin;
    [ObservableProperty] private string _text = string.Empty;
    [ObservableProperty] private bool _isSelected;
}