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
    public DbSet<CurriculumProcessType> CurriculumProcessTypes { get; set; }
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
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<CurriculumProcessType>()
        .HasKey(t => new { t.CurriculaId, t.ProcessTypesId });

      modelBuilder.Entity<CurriculumProcessType>()
        .HasOne(cp => cp.Curriculum)
        .WithMany(c => c.CurriculumProcessTypes)
        .HasForeignKey(cp => cp.CurriculaId);

      modelBuilder.Entity<CurriculumProcessType>()
        .HasOne(cp => cp.ProcessType)
        .WithMany(pt => pt.CurriculumProcessType)
        .HasForeignKey(cp => cp.ProcessTypesId);

    }
  }
}