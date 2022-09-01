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
    public DbSet<Role> Roles { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Rating> Ratings { get; set; }

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

            mb.Property(u => u.Email).IsRequired();

            mb.Property(u => u.RoleId).IsRequired();

            // If u delete Role User alsow will be deleted.
            mb.HasOne(u => u.Role)
           .WithMany(r => r.Users)
           .HasForeignKey(u => u.RoleId)
           .OnDelete(DeleteBehavior.ClientCascade);

            // If u delete User from db his Questions, Answers and Ratings remain in the db.
            mb.HasMany(u => u.Questions)
            .WithOne(q => q.Author)
            .HasForeignKey(q => q.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);

            mb.HasMany(u => u.Answers)
            .WithOne(a => a.Author)
            .HasForeignKey(a => a.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);

            mb.HasMany(u => u.Ratings)
            .WithOne(a => a.Author)
            .HasForeignKey(a => a.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Question>(mb =>
        {
            mb.Property(q => q.Topic).IsRequired();
            mb.Property(q => q.Content).IsRequired();

            // Set the current time as Question Date.
            mb.Property(q => q.Date).HasDefaultValueSql("getdate()");

            // If u delete Question from db ratings and answers for this Question alsow will be deleted.
            mb.HasMany(q => q.Answers)       
            .WithOne(a => a.Question)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.ClientCascade);

            mb.HasMany(q => q.Ratings)
           .WithOne(r => r.Question)
           .HasForeignKey(r => r.QuestionId)
           .OnDelete(DeleteBehavior.ClientCascade);
        });

        modelBuilder.Entity<Answer>(mb =>
        {
           mb.Property(q => q.Content).IsRequired();

           // Set the current time as Answer Date.
           mb.Property(q => q.Date).HasDefaultValueSql("getdate()");

           mb.HasMany(q => q.Ratings)
          .WithOne(r => r.Answer)
          .HasForeignKey(r => r.AnswerId)
          .OnDelete(DeleteBehavior.ClientCascade);
        });

        modelBuilder.Entity<Rating>(mb =>
        {
            mb.Property(r => r.Value).IsRequired();
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("Default"));
    }
}
