using EntityFrameworkLogic.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;


namespace EntityFrameworkLogic
{
    /// <summary>
    /// Контекст базы данных сервиса бронирований
    /// </summary>
    public class ApplicationContext : DbContext
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Множество сущностей бронирований в базе данных
        /// </summary>
        public DbSet<Booking> Bookings { get; set; }

        /// <summary>
        /// Множество сущностей пассажиров в базе данных
        /// </summary>
        public DbSet<Passenger> Passengers { get; set; }

        /// <summary>
        /// Множество сущностей связок пассажир-к-бронированию в базе данных
        /// </summary>
        public DbSet<PassengerToBooking> PassengersToBookings { get; set; }

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
            var bookingBuilder = modelBuilder.Entity<Booking>();
            ConfigureBookings(ref bookingBuilder);

            var passengerBuilder = modelBuilder.Entity<Passenger>();
            ConfigurePassengers(ref passengerBuilder);

            var passengerToBookingBuilder = modelBuilder.Entity<PassengerToBooking>();
            ConfigurePassengersToBookings(ref passengerToBookingBuilder);
        }

        private void ConfigureBookings(ref EntityTypeBuilder<Booking> entityBuilder)
        {
            entityBuilder.ToTable("Bookings");

            entityBuilder.HasKey(x => x.Id);

            entityBuilder.HasIndex(x => x.Code)
                         .IsUnique();

            entityBuilder.Property(x => x.Id)
                         .ValueGeneratedOnAdd()
                         .HasColumnName("Id")
                         .IsRequired();

            entityBuilder.Property(x => x.Code)
                         .HasMaxLength(10)
                         .IsFixedLength()
                         .HasColumnName("Code")
                         .IsRequired();

            entityBuilder.Property(x => x.CustomerUserId)
                         .HasColumnName("CustomerUserId")
                         .IsRequired();

            entityBuilder.Property(x => x.Confirmed)
                         .HasColumnName("Confirmed")
                         .IsRequired()
                         .HasDefaultValue(false);

            entityBuilder.Property(x => x.CreatedAt)
                         .HasColumnName("CreatedAt")
                         .HasDefaultValue(null);
        }

        private void ConfigurePassengers(ref EntityTypeBuilder<Passenger> entityBuilder)
        {
            entityBuilder.ToTable("Passengers");

            entityBuilder.HasKey(x => x.Id);

            entityBuilder.HasIndex(x => x.DocumentNumber)
                         .IsUnique();

            entityBuilder.Property(x => x.Id)
                         .ValueGeneratedOnAdd()
                         .HasColumnName("Id")
                         .IsRequired();

            entityBuilder.Property(x => x.FirstName)
                         .HasMaxLength(100)
                         .HasColumnName("FirstName")
                         .IsRequired();

            entityBuilder.Property(x => x.Surname)
                         .HasMaxLength(100)
                         .HasColumnName("Surname")
                         .IsRequired();

            entityBuilder.Property(x => x.MiddleName)
                         .HasMaxLength(100)
                         .HasColumnName("MiddleName")
                         .IsRequired();

            entityBuilder.Property(x => x.DocumentNumber)
                         .HasColumnName("DocumentNumber")
                         .IsRequired();

            entityBuilder.Property(x => x.DocumentIssuerCountry)
                         .HasColumnName("DocumentIssuerCountry")
                         .IsRequired();

            entityBuilder.Property(x => x.BirthDate)
                         .HasColumnName("BirthDate")
                         .IsRequired();

            entityBuilder.Property(x => x.Gender)
                         .HasColumnName("Gender")
                         .IsRequired();

            entityBuilder.Property(x => x.PhoneNumber)
                         .HasMaxLength(15)
                         .HasColumnName("PhoneNumber")
                         .IsRequired();

            entityBuilder.Property(x => x.Email)
                         .HasMaxLength(100)
                         .HasColumnName("Email");
        }

        private void ConfigurePassengersToBookings(ref EntityTypeBuilder<PassengerToBooking> entityBuilder)
        {
            entityBuilder.ToTable("PassengersToBookings");

            entityBuilder.HasKey(x => x.Id);

            entityBuilder.HasOne(a => a.Booking)
                         .WithMany(b => b.PassengersToBookings)
                         .HasForeignKey(a => a.BookingId)
                         .OnDelete(DeleteBehavior.Cascade);

            entityBuilder.HasOne(a => a.Passenger)
                         .WithMany(b => b.PassengerToBookings)
                         .HasForeignKey(a => a.PassengerId)
                         .OnDelete(DeleteBehavior.Cascade);

            entityBuilder.Property(x => x.Id)
                         .ValueGeneratedOnAdd()
                         .HasColumnName("Id")
                         .IsRequired();

            entityBuilder.Property(x => x.CarryOnMaxWeight)
                         .HasColumnName("CarryOnMaxWeight")
                         .IsRequired();

            entityBuilder.Property(x => x.BaggageMaxWeight)
                         .HasColumnName("BaggageMaxWeight")
                         .HasDefaultValue(0f)
                         .IsRequired();
        }
    }
}
