using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace SecondTry.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Musician> Musicians { get; set; }
        public DbSet<RecordingSession> RecordingSessions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            // Путь к серверу SQL Server и имя базы данных
            var connectionString = $"Server=(localdb)\\mssqllocaldb;Database=SoundstudioData.mdf;Trusted_Connection=True;";
            optionsBuilder.UseSqlServer(connectionString, sqlServerOptionsAction =>
            {
                sqlServerOptionsAction.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация отношений между моделями остается прежней
            modelBuilder.Entity<RecordingSession>()
                .HasOne(rs => rs.Musician)
                .WithMany(m => m.Sessions)
                .HasForeignKey(rs => rs.MusicianId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}