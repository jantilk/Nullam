using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class NullamDbContext : DbContext
{
    public NullamDbContext(DbContextOptions<NullamDbContext> options) : base(options) { }
    
    public DbSet<Person> Persons { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<SocialEvent> SocialEvents { get; set; }
    public DbSet<SocialEventPerson> SocialEventPersons { get; set; }
    public DbSet<SocialEventCompany> SocialEventCompanies { get; set; }
    public DbSet<Resource> Resources { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SocialEventCompany>()
            .HasKey(mc => new { mc.SocialEventId, mc.CompanyId });
        
        modelBuilder.Entity<SocialEventPerson>()
            .HasKey(mc => new { mc.SocialEventId, mc.PersonId });
        
        modelBuilder.Entity<SocialEventPerson>()
            .HasOne(sep => sep.SocialEvent)
            .WithMany(se => se.Persons)
            .HasForeignKey(sep => sep.SocialEventId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SocialEventCompany>()
            .HasOne(sec => sec.SocialEvent)
            .WithMany(se => se.Companies)
            .HasForeignKey(sec => sec.SocialEventId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<SocialEventPerson>()
            .HasOne<Resource>(sep => sep.PaymentType)
            .WithMany()
            .HasForeignKey(sep => sep.ResourceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SocialEventCompany>()
            .HasOne<Resource>(sec => sec.PaymentType)
            .WithMany()
            .HasForeignKey(sec => sec.ResourceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SocialEvent>()
            .Property(se => se.Date)
            .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        
        modelBuilder.Entity<SocialEvent>()
            .Property(se => se.CreatedAt)
            .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        
        modelBuilder.Entity<Resource>()
            .Property(r => r.CreatedAt)
            .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        
        modelBuilder.Entity<Person>()
            .Property(p => p.CreatedAt)
            .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        modelBuilder.Entity<Person>()
            .HasIndex(x => x.IdCode)
            .IsUnique();

        modelBuilder.Entity<Company>()
            .HasIndex(x => x.RegisterCode)
            .IsUnique();

        modelBuilder.Entity<Company>()
            .Property(c => c.CreatedAt)
            .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        
        base.OnModelCreating(modelBuilder);
    }
    
    
}