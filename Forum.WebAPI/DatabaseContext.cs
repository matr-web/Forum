using Forum.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.WebAPI;

public class DatabaseContext : DbContext
{
    public DatabaseContext() : base()
    { 
    }

    public DatabaseContext(DbContextOptions options) : base()
    {
    }

    public DatabaseContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(mb =>
        {
            mb.Property(u => u.FirstName).IsRequired();
            mb.Property(u => u.FirstName).HasColumnName("First Name");
            mb.Property(u => u.FirstName).HasMaxLength(50);

            mb.Property(u => u.LastName).IsRequired();
            mb.Property(u => u.LastName).HasColumnName("Last Name");
            mb.Property(u => u.LastName).HasMaxLength(50);

            mb.HasMany(u => u.Questions)
            .WithOne(q => q.Author)
            .HasForeignKey(q => q.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);

            mb.HasMany(u => u.Answers)
            .WithOne(a => a.Author)
            .HasForeignKey(a => a.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Question>(mb =>
        {
            mb.HasMany(q => q.Answers)
            .WithOne(a => a.Question)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.ClientCascade);
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("Default"));
    }
}
