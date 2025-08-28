using Avalonia.Controls;
using DynamicData.Kernel;
using SecondTry.ViewModels;

namespace SecondTry.Views;

public partial class MainView : UserControl
{

    public MainView()
    {
        InitializeComponent();
    }

    private void Musicians_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        ((MainViewModel)DataContext!).LoadRecordingSessions_of_selectedMusician();
    }

    private void Session_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        CalculationResult.Text = ((MainViewModel)DataContext!).Calculations().ToString();
    }

    private void Autorization_Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // Проверка на пустые поля
        if (Login.Text == string.Empty || Password.Text == string.Empty
            || Login.Text == null || Password.Text == null)
        {
            AutorizationErrorText.IsVisible = true;
        }
        // Проверка верности введённых данных
        else if (Login.Text.ToLower().Trim() == "admin"
            && Password.Text.ToLower().Trim() == "admin")
        {
            AutorizationErrorText.IsVisible = false;
            AutorizationForm.IsVisible = false;
            MainBackground.IsVisible = false;
        }
        // если данные неверны
        else
        {
            AutorizationErrorText.IsVisible = true;
        }
    }

    private void InsertButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // если введены значения во все поля
        if (FIO_textbox.Text != string.Empty &&
            Instrument_combobox.SelectedIndex != -1 &&
            StartTime_textbox.Text != string.Empty &&
            Duration_textbox.Text != string.Empty &&
            StudioName_textbox.Text != string.Empty &&
            CostPerHour_textbox.Text != string.Empty)
        {
            InputErrorText.IsVisible = false;
            baseInputButton.ContextFlyout!.Hide();
            ((MainViewModel)DataContext!).SaveData(
                FIO_textbox.Text!,
                Instrument_combobox.SelectedValue!.ToString()!,
                StartTime_textbox.Text!,
                Duration_textbox.Text!,
                StudioName_textbox.Text!,
                CostPerHour_textbox.Text!);
        }
        else
            InputErrorText.IsVisible = true;
    }
}
