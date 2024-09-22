using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace EntityFrameworkLogic
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public class ApplicationContext : DbContext
    {
        private readonly IConfiguration _config = null!;

        /// <summary>
        /// Максимальная длина имени пользователя
        /// </summary>
        public const int MAX_NAME_LENGTH = 100;

        /// <summary>
        /// Множество сущностей пользователей в базе данных
        /// </summary>
        public DbSet<User> Users { get; set; }

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
            ConfigureUsers(modelBuilder);
        }

        private void ConfigureUsers(ModelBuilder modelBuilder)
        {
            var userEntity = modelBuilder.Entity<User>();

            AttachUsersToTable(userEntity);

            ConfigureIdColumn(userEntity);
            ConfigureNameColumn(userEntity);
            ConfigureEmailColumn(userEntity);
            ConfigureRoleColumn(userEntity);
            ConfigureCreatedAtColumn(userEntity);

            ConfigureNameUnique(userEntity);
            ConfigureEmailUnique(userEntity);

            ConfigurePrimaryKey(userEntity);
        }

        private void AttachUsersToTable(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("users");
        }

        private void ConfigureIdColumn(EntityTypeBuilder<User> entity)
        {
            entity
                .Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired();
        }

        private void ConfigureNameColumn(EntityTypeBuilder<User> entity)
        {
            entity
                .Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(MAX_NAME_LENGTH)
                .IsRequired();
        }

        private void ConfigureEmailColumn(EntityTypeBuilder<User> entity)
        {
            entity
                .Property(x => x.Email)
                .HasColumnName("email")
                .IsRequired();
        }

        private void ConfigureRoleColumn(EntityTypeBuilder<User> entity)
        {
            entity
                .Property(x => x.Role)
                .HasColumnName("role");
        }

        private void ConfigureCreatedAtColumn(EntityTypeBuilder<User> entity)
        {
            entity
                .Property(x => x.CreatedAt)
                .HasColumnName("created_at");
        }

        private void ConfigureNameUnique(EntityTypeBuilder<User> entity)
        {
            entity.HasIndex(x => x.Name).IsUnique();
        }

        private void ConfigureEmailUnique(EntityTypeBuilder<User> entity)
        {
            entity.HasIndex(x => x.Email).IsUnique();
        }

        private void ConfigurePrimaryKey(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(x => x.Id);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _config["ServiceSettings:MysqlConnectionString"];

            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(9, 0 ,1)));
        }
    }
}
