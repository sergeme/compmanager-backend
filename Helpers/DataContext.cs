using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CompManager.Entities;

namespace CompManager.Helpers
{
  public class DataContext : DbContext
  {
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Competence> Competences { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Curriculum> Curricula { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Process> Processes { get; set; }
    public DbSet<ProcessType> ProcessTypes { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Vocable> Vocables { get; set; }
    private readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      // connect to sqlite database
      options.UseNpgsql(Configuration.GetConnectionString("CompManagerBackend"));
      options.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Curriculum>()
      .HasMany(x => x.ProcessTypes)
      .WithMany(x => x.Curricula)
      .UsingEntity<CurriculumProcessType>(
      x => x.HasOne(x => x.ProcessType)
      .WithMany().HasForeignKey(x => x.ProcessTypeId),
      x => x.HasOne(x => x.Curriculum)
      .WithMany().HasForeignKey(x => x.CurriculumId)
      );

      modelBuilder.Entity<Course>()
      .HasMany(x => x.Locations)
      .WithMany(x => x.Courses)
      .UsingEntity<CourseLocation>(
      x => x.HasOne(x => x.Location)
      .WithMany().HasForeignKey(x => x.LocationId),
      x => x.HasOne(x => x.Course)
      .WithMany().HasForeignKey(x => x.CourseId)
      );

      modelBuilder.Entity<Competence>()
      .HasMany(x => x.Tags)
      .WithMany(x => x.Competences)
      .UsingEntity<CompetenceTag>(
      x => x.HasOne(x => x.Tag)
      .WithMany().HasForeignKey(x => x.TagId),
      x => x.HasOne(x => x.Competence)
      .WithMany().HasForeignKey(x => x.CompetenceId)
      );
    }
  }
}