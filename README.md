# BDSA_ProjectBank_TheCTeam
The C Team's BDSA project 2021 ITU

## Install the Entity Framework global tool

```powershell
dotnet tool install --global dotnet-ef
```

## Run SQL Server in Docker Container

```powershell
$password = New-Guid
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
$database = "Studybank"
$connectionString = "Server=localhost;Database=$database;User Id=sa;Password=$password"
```

## Enable User Secrets

```powershell
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:StudyBank" "$connectionString"
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package Microsoft.Extensions.Configuration.UserSecrets
```

## Settings

```csharp
var configuration = LoadConfiguration();
var connectionString = configuration.GetConnectionString("Futurama");

static IConfiguration LoadConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddUserSecrets<Program>();

    return builder.Build();
}
```

## Startup project

```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
```

## Entities project

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

## Add migration

```bash
dotnet ef migrations add InitialMigration
```

## Update database

```bash
dotnet ef database update
```

## ComicsContext

```csharp
public ComicsContext(DbContextOptions<ComicsContext> options) : base(options) { }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder
        .Entity<Character>()
        .Property(e => e.Gender)
        .HasConversion(new EnumToStringConverter<Gender>());
}
```

## DesignTimeDbContextFactory

```csharp
public class ComicsContextFactory : IDesignTimeDbContextFactory<ComicsContext>
{
    public ComicsContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<Program>()
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("Superheroes");

        var optionsBuilder = new DbContextOptionsBuilder<ComicsContext>()
            .UseSqlServer(connectionString);

        return new ComicsContext(optionsBuilder.Options);
    }
}
```

## Seed

```csharp
public static void Seed(ComicsContext context)
{
    context.Database.ExecuteSqlRaw("DELETE dbo.PowerCharacter");
    context.Database.ExecuteSqlRaw("DELETE dbo.Characters");
    context.Database.ExecuteSqlRaw("DELETE dbo.Powers");
    context.Database.ExecuteSqlRaw("DELETE dbo.Cities");
    context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Powers', RESEED, 0)");
    context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Cities', RESEED, 0)");
    context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Characters', RESEED, 0)");

    var metropolis = new City { Name = "Metropolis" };
    var gothamCity = new City { Name = "Gotham City" };
    var themyscira = new City { Name = "Themyscira" };

    var superStrength = new Power { Name = "super strength" };
    var flight = new Power { Name = "flight" };
    var invulnerability = new Power { Name = "invulnerability" };
    var superSpeed = new Power { Name = "super speed" };
    var heatVision = new Power { Name = "heat vision" };
    var freezeBreath = new Power { Name = "freeze breath" };
    var xRayVision = new Power { Name = "x-ray vision" };
    var superhumanHearing = new Power { Name = "superhuman hearing" };
    var healingFactor = new Power { Name = "healing factor" };
    var exceptionalMartialArtist = new Power { Name = "exceptional martial artist" };
    var combatStrategy = new Power { Name = "combat strategy" };
    var inexhaustibleWealth = new Power { Name = "inexhaustible wealth" };
    var brilliantDeductiveSkill = new Power { Name = "brilliant deductive skill" };
    var advancedTechnology = new Power { Name = "advanced technology" };
    var combatSkill = new Power { Name = "combat skill technology" };
    var superhumanAgility = new Power { Name = "superhuman weaponry" };
    var magicWeaponry = new Power { Name = "magic agility" };
    var gymnasticAbility = new Power { Name = "gymnastic ability" };

    context.Superheroes.AddRange(
        new Character { Name = "Clark Kent", AlterEgo = "Superman", Occupation = "Reporter", City = metropolis, Gender = Male, FirstAppearance = DateTime.Parse("1938-04-18"), Powers = new[] { superStrength, flight, invulnerability, superSpeed, heatVision, freezeBreath, xRayVision, superhumanHearing, healingFactor } },
        new Character { Name = "Bruce Wayne", AlterEgo = "Batman", Occupation = "CEO of Wayne Enterprises", City = gothamCity, Gender = Male, FirstAppearance = DateTime.Parse("1939-05-01"), Powers = new[] { exceptionalMartialArtist, combatStrategy, inexhaustibleWealth, brilliantDeductiveSkill, advancedTechnology } },
        new Character { Name = "Diana", AlterEgo = "Wonder Woman", Occupation = "Amazon Princess", City = themyscira, Gender = Female, FirstAppearance = DateTime.Parse("1941-10-21"), Powers = new[] { superStrength, invulnerability, flight, combatSkill, combatStrategy, superhumanAgility, healingFactor, magicWeaponry } },
        new Character { Name = "Selina Kyle", AlterEgo = "Catwoman", Occupation = "Thief", City = gothamCity, Gender = Female, FirstAppearance = DateTime.Parse("1940-04-01"), Powers = new[] { exceptionalMartialArtist, gymnasticAbility, combatSkill } }
    );

    context.SaveChanges();
}
```