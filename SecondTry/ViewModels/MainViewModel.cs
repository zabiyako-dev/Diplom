using SecondTry.Models;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;
using System;
using Microsoft.EntityFrameworkCore;
using SecondTry.Model;

namespace SecondTry.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";

    private ObservableCollection<Musician> _musicians;
    private ObservableCollection<RecordingSession> _recordingsession;

    public ObservableCollection<Musician> Musicians
    {
        get => _musicians;
        set => this.RaiseAndSetIfChanged(ref _musicians, value);
    }

    public ObservableCollection<RecordingSession> RecordingSession
    {
        get => _recordingsession;
        set => this.RaiseAndSetIfChanged(ref _recordingsession, value);
    }

    private Musician _selectedMusician;
    private RecordingSession _selectedRecordingSession;

    public bool HasSelectedMusician => SelectedMusician != null;
    public bool HasSelectedSession => SelectedRecordingSession != null;

    public bool IsSelectedMusicianAvailable => SelectedMusician != null;
    public bool IsSelectedSessionAvailable => SelectedRecordingSession != null;

    public Musician SelectedMusician
    {
        get => _selectedMusician;
        set => this.RaiseAndSetIfChanged(ref _selectedMusician, value);
    }

    public RecordingSession SelectedRecordingSession
    {
        get => _selectedRecordingSession;
        set => this.RaiseAndSetIfChanged(ref _selectedRecordingSession, value);
    }

    public MainViewModel()
    {
        LoadData(); // Загрузка данных при старте
    }

    private void LoadData()
    {
        try
        {
            using (var db = new AppDbContext())
            {
                if (!db.Database.CanConnect()) throw new Exception("Нет соединения с базой данных.");
                Debug.WriteLine("Все записи из базы данных");
                Debug.WriteLine($"Музыкантов: {db.Musicians.Count()}");
                foreach (var musician in db.Musicians.Include(m => m.Sessions))
                    Debug.WriteLine($"{musician.Id}: {musician.FullName}, инструмент: {musician.Instrument}");

                Debug.WriteLine($"Сессий записи звука: {db.RecordingSessions.Count()}");

                Musicians = new ObservableCollection<Musician>(db.Musicians.Include(m => m.Sessions));
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("ОБРАТИ ВНИМАНИЕ! Обнаружено исключение: ");
            Debug.WriteLine(ex.ToString());
            Debug.WriteLine("ОБРАТИ ВНИМАНИЕ!");
        }
    }

    public void LoadRecordingSessions_of_selectedMusician()
    {
        if (_selectedMusician == null) return;

        using (var db = new AppDbContext())
        {
            Debug.WriteLine($"ID выбранного музыканта {_selectedMusician.Id}");
            Debug.WriteLine(db.RecordingSessions.ToList().ElementAt(0).MusicianId);
            RecordingSession = new ObservableCollection<RecordingSession>(
                db.RecordingSessions.Where(rs => rs.MusicianId == _selectedMusician.Id).ToList());
        }
    }

    public decimal Calculations()
    {
        if (_selectedMusician == null || _selectedRecordingSession == null)
        {
            Debug.WriteLine("Не выбран музыкант или же сессия записи");
            return -1;
        }

        decimal result = _selectedRecordingSession.CostPerHour * _selectedRecordingSession.Duration.Hours;

        return result;
    }

    public void SaveData(string FIO, string Instrument, string StartTime, string Duration, string StudioName, string CostPerHour)
    {
        using (var db = new AppDbContext()) // Используем созданный ранее контекст
        {
            // Проверяем заполненность полей
            if (FIO != string.Empty &&
                Instrument != string.Empty &&
                StartTime != string.Empty &&
                Duration != string.Empty &&
                StudioName != string.Empty &&
                CostPerHour != string.Empty)
            {

                // Преобразование текста в нужный формат
                string fullName = FIO.Trim();
                string instrument = Instrument.Trim();

                // Добавление нового музыканта
                var musician = new Musician
                {
                    FullName = fullName,
                    Instrument = instrument
                };
                db.Musicians.Add(musician); // Добавляем музыканта в контекст

                // Парсим поля длительности и стоимости
                TimeSpan duration;
                bool isValidDuration = TimeSpan.TryParse(Duration, out duration);

                decimal costPerHour;
                bool isValidCost = decimal.TryParse(CostPerHour, out costPerHour);

                if (isValidDuration && isValidCost)
                {
                    // Заполняем остальные поля и создаем новый сеанс записи
                    var recordingSession = new RecordingSession
                    {
                        StartTime = DateTime.Parse(StartTime),
                        Duration = duration,
                        StudioName = StudioName.Trim(),
                        CostPerHour = costPerHour,
                        Musician = musician // Присваиваем связь с музыкантом
                    };

                    db.RecordingSessions.Add(recordingSession); // Добавляем запись в контекст
                                                                // Сохраняем изменения в базе данных
                    db.SaveChanges();
                }
            }

        }
    }

    public void PrintDataToExcel()
    {
        Exports.ExportToCsvFormat("output.csv", RecordingSession, Musicians);
    }
}
