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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SocialEventCompany>()
            .HasKey(mc => new { mc.SocialEventId, mc.CompanyId });
        
        modelBuilder.Entity<SocialEventPerson>()
            .HasKey(mc => new { mc.SocialEventId, mc.PersonId });
        
        // TODO
        // Other entity configurations?
        
        base.OnModelCreating(modelBuilder);
    }
    
    public static void Initialize(NullamDbContext context)
    {
        // TODO
        // Add initial data
    }
}