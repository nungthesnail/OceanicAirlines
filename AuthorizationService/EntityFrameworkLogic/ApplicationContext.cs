using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;


namespace EntityFrameworkLogic
{
    /// <summary>
    /// Контекст базы данных сервиса аутентификации и авторизации
    /// </summary>
    public class ApplicationContext : DbContext
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Множество хешей паролей пользователей в базе данных
        /// </summary>
        public DbSet<PasswordHash> PasswordHashes { get; set; }

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="config">Конфигурация приложения</param>
        public ApplicationContext(IConfiguration config)
        {
            _config = config;

            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<PasswordHash>();
            ConfigureTable(entity);

            ConfigurePrimaryKey(entity);
            ConfigureLinkedUserId(entity);
            ConfigureHashedPassword(entity);
        }

        private void ConfigureTable(EntityTypeBuilder<PasswordHash> entity)
        {
            entity.ToTable("password_hashes");
        }

        private void ConfigurePrimaryKey(EntityTypeBuilder<PasswordHash> entity)
        {
            entity
                .Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired();

            entity.HasKey(x => x.Id);
        }

        private void ConfigureLinkedUserId(EntityTypeBuilder<PasswordHash> entity)
        {
            entity
                .Property(x => x.LinkedUserId)
                .HasColumnName("LinkedUserId")
                .IsRequired();

            entity
                .HasIndex(x => x.LinkedUserId)
                .IsUnique();
        }

        private void ConfigureHashedPassword(EntityTypeBuilder<PasswordHash> entity)
        {
            entity
                .Property(x => x.HashedPassword)
                .HasColumnName("HashedPassword")
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = _config["ServiceSettings:MysqlConnectionString"];

            optionsBuilder.UseMySql(connString, new MySqlServerVersion(new Version(9, 0, 1)));
        }
    }
}
