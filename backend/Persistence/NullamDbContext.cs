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
        
        base.OnModelCreating(modelBuilder);
    }
    
    public static void Initialize(NullamDbContext context)
    {
        // Ensure database is created
        context.Database.EnsureCreated();

        // Check if the database already has social events
        if (!context.SocialEvents.Any())
        {
            var locationsInEstonia = new[] { "Tallinn", "Tartu", "Pärnu", "Narva", "Kohtla-Järve", "Viljandi", "Rakvere", "Maardu", "Sillamäe", "Kuressaare" };
            var additionalInfos = new[] { "Üritus vabas Õhus", null, "Siseruumides toimuv Üritus", null, null, "Võimalus osaleda töötubades", null, null, null, "Muusikaline etteaste" };
            var rnd = new Random();

            // Future Events
            var futureEvents = new SocialEvent[]
            {
                new()  { Name = "Aenean commodo", Date = RandomFutureDateGenerator(), Location = locationsInEstonia[rnd.Next(locationsInEstonia.Length)], Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, AdditionalInfo = additionalInfos[rnd.Next(additionalInfos.Length)] },
                new() { Name = "Fusce ex dui, finibus eu luctus vel", Date = RandomFutureDateGenerator(), Location = locationsInEstonia[rnd.Next(locationsInEstonia.Length)], Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, AdditionalInfo = additionalInfos[rnd.Next(additionalInfos.Length)] },
                new() { Name = "Nunc lobortis metus eu massa viverra ultri", Date = RandomFutureDateGenerator(), Location = locationsInEstonia[rnd.Next(locationsInEstonia.Length)], Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, AdditionalInfo = additionalInfos[rnd.Next(additionalInfos.Length)] },
                new() { Name = "Integer nec nulla vitae", Date = RandomFutureDateGenerator(), Location = locationsInEstonia[rnd.Next(locationsInEstonia.Length)], Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, AdditionalInfo = additionalInfos[rnd.Next(additionalInfos.Length)] }
            };

            // Past Events
            var pastEvents = new SocialEvent[]
            {
                new() { Name = "Aenean commodo", Date = RandomPastDateGenerator(), Location = locationsInEstonia[rnd.Next(locationsInEstonia.Length)], Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, AdditionalInfo = additionalInfos[rnd.Next(additionalInfos.Length)] },
                new() { Name = "Fusce ex dui, finibus eu luctus vel", Date = RandomPastDateGenerator(), Location = locationsInEstonia[rnd.Next(locationsInEstonia.Length)], Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, AdditionalInfo = additionalInfos[rnd.Next(additionalInfos.Length)] },
                new() { Name = "Nunc lobortis metus eu massa viverra ultri", Date = RandomPastDateGenerator(), Location = locationsInEstonia[rnd.Next(locationsInEstonia.Length)], Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, AdditionalInfo = additionalInfos[rnd.Next(additionalInfos.Length)] },
                new() { Name = "Integer nec nulla vitae", Date = RandomPastDateGenerator(), Location = locationsInEstonia[rnd.Next(locationsInEstonia.Length)], Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, AdditionalInfo = additionalInfos[rnd.Next(additionalInfos.Length)] },
                new() { Name = "Praesent molestie dapibus lorem", Date = RandomPastDateGenerator(), Location = locationsInEstonia[rnd.Next(locationsInEstonia.Length)], Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, AdditionalInfo = additionalInfos[rnd.Next(additionalInfos.Length)] }
            };

            // Add future and past events to the context
            foreach (var sEvent in futureEvents.Concat(pastEvents))
            {
                context.SocialEvents.Add(sEvent);
            }

            context.SaveChanges();
        }
    }

    private static DateTime RandomFutureDateGenerator()
    {
        var rnd = new Random();
        var futureDate = DateTime.Now.AddDays(rnd.Next(1, 365)); // Random future date within the next year
        return futureDate;
    }

    private static DateTime RandomPastDateGenerator()
    {
        var rnd = new Random();
        var pastDate = DateTime.Now.AddDays(-rnd.Next(1, 365)); // Random past date within the last year
        return pastDate;
    }
}