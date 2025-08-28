using Microsoft.Data.SqlClient;
using SecondTry.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace SecondTry.Model
{
    public class Exports
    {
        public static void ExportToCsvFormat(string filePath, IEnumerable<RecordingSession> RSs, IEnumerable<Musician> Ms)
        {
            try
            {
                // Объединяем данные, группируем по имени музыканта
                var groupedSessions = from m in Ms
                                      join s in RSs on m.Id equals s.MusicianId into ms
                                      select new SessionGroupByMusician
                                      {
                                          FullName = m.FullName,
                                          Sessions = ms.ToList()
                                      };

                // Создаем CSV-файл
                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    // Шапка таблицы
                    writer.WriteLine("Имя музыканта;Время начала;Длительность;Название студии;Стоимость часа (рубли)");

                    foreach (var group in groupedSessions.OrderBy(g => g.FullName)) // Группировка по имени
                    {
                        foreach (var session in group.Sessions)
                        {
                            writer.WriteLine($"{group.FullName};{session.StartTime.ToString("dd-MM-yyyy HH:mm:ss")};{session.Duration.ToString(@"hh\:mm\:ss")};{session.StudioName};{session.CostPerHour.ToString("C2", CultureInfo.CreateSpecificCulture("ru"))}");
                        }
                    }

                    Debug.WriteLine("ВЫВОД ПРОИЗВЕДЁН");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ОБРАТИ ВНИМАНИЕ! Обнаружено исключение: ");
                Debug.WriteLine(ex.ToString());
                Debug.WriteLine("ОБРАТИ ВНИМАНИЕ!");
            }
        }
    }
}