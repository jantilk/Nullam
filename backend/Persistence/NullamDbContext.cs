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
    
    //TODO: run Initialize only on dev mode
    public static void Initialize(NullamDbContext context)
    {
        context.Database.EnsureCreated();

        if (!context.SocialEvents.Any())
        {
            var rnd = new Random();

            var predefinedResources = new Resource[]
            {
                new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Type = "PaymentType", Text = "Pangaülekanne" },
                new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Type = "PaymentType", Text = "Sularaha" },
            };
            
            context.Resources.AddRange(predefinedResources);
            context.SaveChanges();
            
            var individuals = new[]
            {
                // Female individuals
                new Person { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, FirstName = "Kadri", LastName = "Tamm", IdCode = "49008250163" },
                new Person { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, FirstName = "Liis", LastName = "Kask", IdCode = "42512040152" },
                new Person { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, FirstName = "Maarja", LastName = "Lepik", IdCode = "44305254734" },
                new Person { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, FirstName = "Piret", LastName = "Saar", IdCode = "60012063706" },
                new Person { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, FirstName = "Anneli", LastName = "Kivi", IdCode = "47402250206" },
                new Person { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, FirstName = "Kerli", LastName = "Mägi", IdCode = "45308079531" },
                new Person { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, FirstName = "Eva", LastName = "Talvik", IdCode = "43111293752" },
                new Person { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, FirstName = "Laura", LastName = "Veski", IdCode = "44808056517" },
                new Person { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, FirstName = "Kristi", LastName = "Jõgi", IdCode = "45105040004" },
                new Person { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, FirstName = "Sandra", LastName = "Kukk", IdCode = "47908115268" },

                // Male individuals
                new Person { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, FirstName = "Marten", LastName = "Lepik", IdCode = "34404279501" },
                new Person { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, FirstName = "Jaan", LastName = "Mägi", IdCode = "32409186020" },
                new Person { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, FirstName = "Siim", LastName = "Talvik", IdCode = "51707070218" },
                new Person { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, FirstName = "Kalev", LastName = "Veski", IdCode = "36307040174" },
                new Person { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, FirstName = "Lauri", LastName = "Jõgi", IdCode = "36202142277" },
                new Person { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, FirstName = "Tõnis", LastName = "Kukk", IdCode = "35604044982" },
                new Person { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, FirstName = "Priit", LastName = "Tamm", IdCode = "33003262725" },
                new Person { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, FirstName = "Marko", LastName = "Kask", IdCode = "34607255292" },
                new Person { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, FirstName = "Oliver", LastName = "Kivi", IdCode = "50309150230" },
                new Person { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, FirstName = "Kristjan", LastName = "Saar", IdCode = "33409073748" },
            };
            
            context.Persons.AddRange(individuals);
            context.SaveChanges();

            var companies = new[]
            {
                new Company { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "Põhja Tech", RegisterCode = 12345678 },
                new Company { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "Lõuna Foods", RegisterCode = 23456789 },
                new Company { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "Ida Innovations", RegisterCode = 34567890 },
                new Company { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "Lääne Logistics", RegisterCode = 45678901 },
                new Company { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "Tulevik Designs", RegisterCode = 56789012 },
                new Company { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "Minevik Heritage", RegisterCode = 67890123 },
                new Company { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "Kesklinna Constructions", RegisterCode = 78901234 },
                new Company { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "Välismaa Ventures", RegisterCode = 89012345 },
                new Company { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "Meri Marine", RegisterCode = 90123456 },
                new Company { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Name = "Taeva Tech", RegisterCode = 10234567 }
            };

            context.Companies.AddRange(companies);
            context.SaveChanges();
            
            var locationsInEstonia = new [] { "Tallinn", "Tartu", "Pärnu", "Narva", "Viljandi", "Rakvere", "Kuressaare" };
            var additionalInfosForEvents = new [] { 
                "Heategevuslik aastaball", 
                "Tehnoloogiaettevõtete kohtumine", 
                "Klassikalise muusika kontsert", 
                "Toidu- ja kultuurifestival", 
                "Rahvusvaheline ärikonverents" 
            };
            var additionalInfosForPersonParticipants = new [] { 
                "Kogukonnaürituste entusiast", 
                "Tehnoloogiaettevõtja ja kõneleja", 
                "Klassikalise muusika armastaja ja viiuldaja", 
                "Toidukriitik ja reisiblogija", 
                "Rahvusvahelise korporatsiooni tegevjuht"
            };
            var additionalInfosForCompanyParticipants = new [] { 
                "Juhtiv tarkvaraarendusettevõte", 
                "Tuntud kohalik pagaritöökoda ja kohvik", 
                "Rahvusvaheline turundusagentuur", 
                "E-kaubanduse idufirma", 
                "Innovatiivne rohetehnoloogia ettevõte"
            };
            
            var futureEvents = new SocialEvent[] {
                new () { 
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,    
                    Name = "Tuleviku Tehnoloogiakonverents", 
                    Date = RandomFutureDateGenerator(), 
                    Location = "Tallinn", 
                    AdditionalInfo = "Uued suunad tehnoloogias" 
                },
                new () { 
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Name = "Rahvusvaheline Hariduskonverents", 
                    Date = RandomFutureDateGenerator(), 
                    Location = "Tartu", 
                    AdditionalInfo = "Innovatsioon hariduses" 
                },
                new () { 
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Name = "Startup Pitching Üritus", 
                    Date = RandomFutureDateGenerator(), 
                    Location = "Narva", 
                    AdditionalInfo = "Idufirmade esitlused ja võrgustumine" 
                }
            };

            var pastEvents = new SocialEvent[] {
                new() { 
                    Id = Guid.NewGuid(),
                    CreatedAt    = DateTime.UtcNow,
                    Name = "Jazzimuusika Festival", 
                    Date = RandomPastDateGenerator(), 
                    Location = "Pärnu", 
                    AdditionalInfo = "Elav jazzimuusika ja esinejad" 
                },
                new() { 
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Name = "Linnafestival", 
                    Date = RandomPastDateGenerator(), 
                    Location = "Rakvere", 
                    AdditionalInfo = "Kultuuri ja kunsti tähistamine" 
                },
                new() { 
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Name = "Kohaliku Toidu Laat", 
                    Date = RandomPastDateGenerator(), 
                    Location = "Viljandi", 
                    AdditionalInfo = "Kohalikud toidutootjad ja talunikud" 
                },
                new() { 
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Name = "Filmifestival", 
                    Date = RandomPastDateGenerator(), 
                    Location = "Tartu", 
                    AdditionalInfo = "Uued ja põnevad filmid üle maailma" 
                },
                new() { 
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Name = "Raamatumess", 
                    Date = RandomPastDateGenerator(), 
                    Location = "Tallinn", 
                    AdditionalInfo = "Kirjandus, kohtumised autoritega" 
                },
                new() { 
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Name = "Teadusnädal", 
                    Date = RandomPastDateGenerator(), 
                    Location = "Tartu", 
                    AdditionalInfo = "Teadustegevuse tutvustamine ja töötoad" 
                },
                new() { 
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Name = "Ökofestival", 
                    Date = RandomPastDateGenerator(), 
                    Location = "Pärnu", 
                    AdditionalInfo = "Keskkonnasõbralikud lahendused ja haridus" 
                },
                new() { 
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Name = "Kunstiöö", 
                    Date = RandomPastDateGenerator(), 
                    Location = "Viljandi", 
                    AdditionalInfo = "Öösel toimuvad kunstiüritused ja näitused" 
                },
                new() { 
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Name = "Rahvusvaheline Tantsupäev", 
                    Date = RandomPastDateGenerator(), 
                    Location = "Narva", 
                    AdditionalInfo = "Erinevate rahvuste tantsutraditsioonid" 
                },
                new() { 
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Name = "Ajaloofestival", 
                    Date = RandomPastDateGenerator(), 
                    Location = "Rakvere", 
                    AdditionalInfo = "Ajaloolised etendused ja näitused" 
                }
            };

            context.SocialEvents.AddRange(futureEvents.Concat(pastEvents));
            context.SaveChanges();

            foreach (var sEvent in futureEvents.Concat(pastEvents))
            {
                int numberOfParticipants = rnd.Next(1, 6);
                var allPersons = context.Persons.ToList();
                var allCompanies = context.Companies.ToList();
                var allResources = context.Resources.ToList();
                
                if (rnd.NextDouble() < 0.8)
                {
                    sEvent.Location = locationsInEstonia[rnd.Next(locationsInEstonia.Length)];
                    sEvent.AdditionalInfo = additionalInfosForEvents[rnd.Next(additionalInfosForEvents.Length)];
                }

                for (int i = 0; i < numberOfParticipants; i++)
                {
                    if (rnd.NextDouble() > 0.5 && allPersons.Any()) 
                    {
                        var person = allPersons[rnd.Next(allPersons.Count)];
                        var resource = allResources[rnd.Next(allResources.Count)];

                        if (!context.SocialEventPersons.Any(sep =>
                                sep.SocialEventId == sEvent.Id && sep.PersonId == person.Id))
                        {
                            context.SocialEventPersons.Add(new SocialEventPerson
                            {
                                SocialEventId = sEvent.Id,
                                PersonId = person.Id,
                                CreatedAt = DateTime.UtcNow,
                                ResourceId = resource.Id
                            });
                        }
                    }
                    else if (allCompanies.Any())
                    {
                        var company = allCompanies[rnd.Next(allCompanies.Count)];
                        var resource = allResources[rnd.Next(allResources.Count)];

                        if (!context.SocialEventCompanies.Any(sec => sec.SocialEventId == sEvent.Id && sec.CompanyId == company.Id))
                        {
                            context.SocialEventCompanies.Add(new SocialEventCompany
                            {
                                SocialEventId = sEvent.Id,
                                CompanyId = company.Id,
                                CreatedAt = DateTime.UtcNow,
                                NumberOfParticipants = rnd.Next(1, 100),
                                ResourceId = resource.Id
                            });
                        }
                    }
                }
                
                if (!context.SocialEvents.Any(se => se.Id == sEvent.Id))
                {
                    context.SocialEvents.Add(sEvent);
                }
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
        var pastDate = DateTime.Now.AddDays(-rnd.Next(1, 365 * 5)); // Random past date within last 5 years
        return pastDate;
    }
}