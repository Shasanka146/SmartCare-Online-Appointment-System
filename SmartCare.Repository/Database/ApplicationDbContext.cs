using Microsoft.EntityFrameworkCore;
using SmartCare.Shared.PatientData;

namespace SmartCare.Repository.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<PatientDetails> Patients => Set<PatientDetails>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<PatientDetails>(entity =>
        {
            entity.ToTable("Patients");
            entity.HasKey(patient => patient.PatientId);
            entity.Property(patient => patient.Name).HasMaxLength(100).IsRequired();
            entity.Property(patient => patient.Gender).HasMaxLength(20).IsRequired();
            entity.Property(patient => patient.Contactno).HasMaxLength(15).IsRequired();
            entity.Property(patient => patient.Address).HasMaxLength(100).IsRequired();
        });
    }
}
