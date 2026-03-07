using System.Windows;
using FluxRoute.ViewModels;

namespace FluxRoute.Views;

public partial class MainWindow : Window
{
    private readonly MainViewModel _vm;
    private SettingsWindow? _settingsWindow;

    public MainWindow()
    {
        InitializeComponent();
        _vm = new MainViewModel();
        DataContext = _vm;

        // Открываем настройки когда ViewModel просит
        _vm.OpenSettingsRequested += OnOpenSettingsRequested;
    }

    private void OnOpenSettingsRequested(object? sender, EventArgs e)
    {
        if (_settingsWindow is { IsVisible: true })
        {
            _settingsWindow.Activate();
            return;
        }

        _settingsWindow = new SettingsWindow(_vm) { Owner = this };
        _settingsWindow.Show();
    }
}
