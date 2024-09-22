using EntityFrameworkLogic.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;


namespace EntityFrameworkLogic
{
    /// <summary>
    /// Объект контекста базы данных сервиса рейсов
    /// </summary>
    public class ApplicationContext : DbContext
    {
        private IConfiguration _config;

        /// <summary>
        /// Множество сущностей аэропортов в базе данных
        /// </summary>
        public DbSet<Airport> Airports { get; set; }

        /// <summary>
        /// Множество сущностей пар аэропортов в базе данных
        /// </summary>
        public DbSet<AirportsPair> AirportsPairs { get; set; }

        /// <summary>
        /// Множество сущностей базовых рейсов в базе данных
        /// </summary>
        public DbSet<BaseFlight> BaseFlights { get; set; }

        /// <summary>
        /// Множество запланированных рейсов в базе данных
        /// </summary>
        public DbSet<SheduledFlight> SheduledFlights { get; set; }

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="config">Конфигурация приложения</param>
        public ApplicationContext(IConfiguration config)
        {
            _config = config;

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = _config["ServiceSettings:MysqlConnectionString"];

            optionsBuilder.UseMySql(connString, new MySqlServerVersion(new Version(9, 0, 1)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var airportBuilder = modelBuilder.Entity<Airport>();
            ConfigureAirportEntity(ref airportBuilder);

            var airportsPairBuilder = modelBuilder.Entity<AirportsPair>();
            ConfigureAirportsPairEntity(ref airportsPairBuilder);

            var baseFlightBuilder = modelBuilder.Entity<BaseFlight>();
            ConfigureBaseFlightEntity(ref baseFlightBuilder);

            var sheduledFlightBuilder = modelBuilder.Entity<SheduledFlight>();
            ConfigureSheduledFlightEntity(ref sheduledFlightBuilder);
        }

        private void ConfigureAirportEntity(ref EntityTypeBuilder<Airport> entityBuilder)
        {
            entityBuilder.ToTable("Airports");

            entityBuilder.HasKey(x => x.Id);

            entityBuilder.HasIndex(x => x.CodeIata)
                         .IsUnique();

            entityBuilder.Property(x => x.Id)
                         .ValueGeneratedOnAdd()
                         .HasColumnName("Id")
                         .IsRequired();

            entityBuilder.Property(x => x.CodeIata)
                         .HasMaxLength(3)
                         .IsFixedLength()
                         .HasColumnName("CodeIATA")
                         .IsRequired();

            entityBuilder.Property(x => x.Name)
                         .HasMaxLength(255)
                         .HasColumnName("AirportName");
        }

        private void ConfigureAirportsPairEntity(ref EntityTypeBuilder<AirportsPair> entityBuilder)
        {
            entityBuilder.ToTable("AirportsPairs");

            entityBuilder.HasKey(x => x.Id);

            entityBuilder.HasOne(a => a.FirstAirport)
                         .WithMany(b => b.AirportsPairsFirst)
                         .HasForeignKey(a => a.FirstAirportId)
                         .OnDelete(DeleteBehavior.Cascade)
						 .IsRequired();

            entityBuilder.HasOne(a => a.SecondAirport)
                         .WithMany(b => b.AirportsPairsSecond)
                         .HasForeignKey(a => a.SecondAirportId)
                         .OnDelete(DeleteBehavior.Cascade)
						 .IsRequired();

            entityBuilder.Property(x => x.Id)
                         .ValueGeneratedOnAdd()
                         .HasColumnName("Id")
                         .IsRequired();

            entityBuilder.Property(x => x.DistanceInKm)
                         .HasColumnName("DistanceInKm")
                         .IsRequired();
        }

        private void ConfigureBaseFlightEntity(ref EntityTypeBuilder<BaseFlight> entityBuilder)
        {
            entityBuilder.ToTable("BaseFlights");

            entityBuilder.HasKey(x => x.Id);

            entityBuilder.HasOne(a => a.AirportsPair)
                         .WithMany(b => b.BaseFlights)
                         .HasForeignKey(a => a.AirportsPairId)
                         .OnDelete(DeleteBehavior.Cascade)
						 .IsRequired();

            entityBuilder.Property(x => x.Id)
                         .ValueGeneratedOnAdd()
                         .HasColumnName("Id")
                         .IsRequired();

            entityBuilder.Property(x => x.AircraftType)
                         .HasMaxLength(100)
                         .HasColumnName("AircraftType");
        }

        private void ConfigureSheduledFlightEntity(ref EntityTypeBuilder<SheduledFlight> entityBuilder)
        {
            entityBuilder.ToTable("SheduledFlights");

            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Id)
                         .ValueGeneratedOnAdd()
                         .HasColumnName("Id")
                         .IsRequired();

            entityBuilder.HasOne(a => a.BaseFlight)
                         .WithMany(b => b.SheduledFlights)
                         .HasForeignKey(a => a.BaseFlightId)
                         .OnDelete(DeleteBehavior.Cascade)
                         .IsRequired();

            entityBuilder.Property(x => x.Status)
                         .HasColumnName("Status");

            entityBuilder.Property(x => x.SheduledDeparture)
                         .HasColumnName("SheduledDeparture")
                         .IsRequired();

            entityBuilder.Property(x => x.SheduledArrival)
                         .HasColumnName("SheduledArrival")
                         .IsRequired();
        }
    }
}
