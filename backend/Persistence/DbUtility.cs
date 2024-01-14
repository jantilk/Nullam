using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DbUtility
{
    public static void InitializeDb(NullamDbContext context)
    {
        context.Database.EnsureCreated();
        context.ChangeTracker.Clear();

        if (!context.SocialEvents.Any())
        {
            SeedData(context);
        }
    }
    
    private static void SeedData(NullamDbContext context)
    {
        SeedResources(context);
        SeedPersons(context);
        SeedCompanies(context);
        SeedEvents(context);
        context.SaveChanges();
    }
    
    private static void SeedResources(NullamDbContext context)
    {
        var predefinedResources = new Resource[]
        {
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Type = "PaymentType", Text = "Pangaülekanne" },
            new() { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Type = "PaymentType", Text = "Sularaha" },
        };
    
        context.Resources.AddRange(predefinedResources);
    }
    
    private static void SeedPersons(NullamDbContext context)
    {
        var individuals = new[]
        {
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
    }
    
    private static void SeedCompanies(NullamDbContext context)
    {
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
    }
    
    private static void SeedEvents(NullamDbContext context)
    {
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

        PopulateEventParticipants(context, futureEvents);
        PopulateEventParticipants(context, pastEvents);
    }
    
    private static DateTime RandomFutureDateGenerator()
    {
        var rnd = new Random();
        // Random future date within the next year
        var futureDate = DateTime.Now.AddDays(rnd.Next(1, 365));
        return futureDate;
    }

    private static DateTime RandomPastDateGenerator()
    {
        var rnd = new Random();
        // Random past date within last 5 years
        var pastDate = DateTime.Now.AddDays(-rnd.Next(1, 365 * 5)); 
        return pastDate;
    }

    private static void PopulateEventParticipants(NullamDbContext context, SocialEvent[] events)
    {
        var rnd = new Random();
        var allPersons = context.Persons.AsNoTracking().ToList();
        var allCompanies = context.Companies.AsNoTracking().ToList();
        var allResources = context.Resources.AsNoTracking().ToList();


        foreach (var eventItem in events)
        {
            // Random number of person participants
            int numberOfPersonParticipants = rnd.Next(1, 6);
            // Random number of company participants
            int numberOfCompanyParticipants = rnd.Next(1, 3);

            // Select unique persons
            var selectedPersonIds = new HashSet<Guid>();
            while (selectedPersonIds.Count < numberOfPersonParticipants)
            {
                var person = allPersons[rnd.Next(allPersons.Count)];
                if (!context.SocialEventPersons.Any(sep => sep.SocialEventId == eventItem.Id && sep.PersonId == person.Id))
                {
                    selectedPersonIds.Add(person.Id);
                }
            }

            foreach (var personId in selectedPersonIds)
            {
                context.SocialEventPersons.Add(new SocialEventPerson
                {
                    SocialEventId = eventItem.Id,
                    PersonId = personId,
                    CreatedAt = DateTime.UtcNow,
                    ResourceId = allResources[0].Id,
                });
            }

            // Select unique companies
            var selectedCompanyIds = new HashSet<Guid>();
            while (selectedCompanyIds.Count < numberOfCompanyParticipants)
            {
                var company = allCompanies[rnd.Next(allCompanies.Count)];
                if (!context.SocialEventCompanies.Any(sec => sec.SocialEventId == eventItem.Id && sec.CompanyId == company.Id))
                {
                    selectedCompanyIds.Add(company.Id);
                }
            }

            foreach (var companyId in selectedCompanyIds)
            {
                context.SocialEventCompanies.Add(new SocialEventCompany
                {
                    SocialEventId = eventItem.Id,
                    CompanyId = companyId,
                    CreatedAt = DateTime.UtcNow,
                    ResourceId = allResources[0].Id,
                    NumberOfParticipants = 2,
                });
            }
        }

        context.SaveChanges();
    }
}