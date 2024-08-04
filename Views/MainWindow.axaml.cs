using Avalonia.Controls;
using Avalonia.Interactivity;

namespace GamIt.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        GameManager.ResyncAll().GetAwaiter().GetResult();
    }
}