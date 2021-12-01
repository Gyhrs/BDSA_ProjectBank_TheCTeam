using System.Collections;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure;
using System;
using System.Collections.Generic;

namespace MyApp.Server;

public static class SeedExtensions
{
    public static IHost Seed(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<StudyBankContext>();

            SeedProjects(context);
        }
        return host;
    }

    private static void SeedProjects(StudyBankContext context)
    {
        context.Database.Migrate();


        context.Database.ExecuteSqlRaw("DELETE dbo.Projects");
        context.Database.ExecuteSqlRaw("DELETE dbo.ProjectSupervisor");
        context.Database.ExecuteSqlRaw("DELETE dbo.ProjectTag");
        context.Database.ExecuteSqlRaw("DELETE dbo.StudyBankUser");
        context.Database.ExecuteSqlRaw("DELETE dbo.Tags");
        //context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Projects', RESEED, 0)");

        
        List<Project> projects = new List<Project>();
        List<Supervisor> supervisors = new List<Supervisor>();


        List<string> existingEmails = new List<string>();

        for (int i = 0; i < 50; i++)
        {
            supervisors.Add(RandomSupervisor(existingEmails));
        }

        for (int i = 0; i < 200; i++)
        {
            projects.Add(RandomProject(supervisors));
        }

        // foreach (var p in projects)
        // {
        //     Console.WriteLine("---- Project -----");

        //     Console.WriteLine(p.Name);
        //     Console.WriteLine(p.Description);
        //     Console.WriteLine("By: " + p.CreatedBy.Name + " " + p.CreatedBy.Email);
        //     Console.WriteLine("Startdate: " + p.StartDate.ToString("dd/MM/yyyy") + "\nEnd date: " + p.EndDate.ToString("dd/MM/yyyy"));
        //     Console.WriteLine("Supervisors: ");
        //     foreach (var s in p.Supervisors)
        //     {
        //         Console.WriteLine(s.Name + " " + s.Email);
        //     }

        //     Console.WriteLine();
        // }

        foreach (var p in projects) {
            context.Projects.Add(p);
        }
        context.SaveChanges();
    }

    public static Project RandomProject(List<Supervisor> supervisors)
    {
        Project p = new Project();
        Random r = new Random();


        // Create random title, start/end date and CreatedBy Supervisor
        p.Name = RandomTitle();
        p.StartDate = DateTime.Parse("1/2/2022");
        p.EndDate = DateTime.Parse("24/6/2022");
        p.CreatedBy = supervisors[r.Next(0, supervisors.Count - 1)];
        p.Supervisors = new List<Supervisor>();

        // Create 1-3 random super visor for the project
        for (int i = 0; i < r.Next(1, 3); i++)
        {
            p.Supervisors.Add(supervisors[r.Next(0, supervisors.Count - 1)]);
        }

        p.Description = RandomDescription(p.Name);

        return p;
    }

    public static string RandomDescription(string title)
    {

        string[] buzzwords = title.Split(" ");



        // Description is build up of 1 starter, title + ?. Then a section about the firm, and then a conclusion
        Random r = new Random();
        string company = RandomCompany();


        List<string> starters = new List<string> { "Are you about to write your master’s thesis or a similar project and interested in exploring",
                                                   "Have an interesting idea involving",
                                                   "Do you wanna grow and explore the market of",
                                                   "Have you been wondering how to improve the current state of",
                                                   "Are you interested in exploring the future of"};

        List<string> workWithCompany = new List<string>
        { "Then join us at " + company + "! ",
          "We're interested in working with you! Help us at " + company + ". ",
          "We're looking for students to help us at " + company + ". ",
          "At " + company + " we invent the future. Join us! ",
          "By: " + company + ". "
        };

        List<string> workWithbuzzwords = new List<string>
        { "Work with " + buzzwords[0] + " and " + buzzwords[1] + " alongside talented individuals at " + company + " and help invent the future! ",
          "Do you wanna work with " + buzzwords[0] + " and " + buzzwords[1] + "?" ,
          "Can you see yourself working with " + buzzwords[0] + " and " + buzzwords[1] + "? ",
          "Do you like " + buzzwords[1] + "? What about " + buzzwords[0] + "? ",
          "Help us at " + company + " working with " + buzzwords[0] + " and " + buzzwords[1] + ". ",
          "Come write your master’s thesis about " + title + " at " + company + ". "
        };

        List<string> adjectives = new List<string>
        {
            "hardworking",
            "enthusiastic",
            "creative",
            "ambitious",
            "clever",
            "fearless",
            "stylish",
            "adventurous",
            "intelligent",
            "bright",
            "brainy",
            "savvy",
            "perceptive",
            "sharp",
            "audacious",
            "interesting",
            "strong",
            "likeable",
            "beautiful",
            "ferocious",
            "smart",
            "not stupid",
            "genius",
            "boy wonder",
            "scientist",
            "team player",
            "passionate",
            "self-driven",
            "curious",
        };

        adjectives = adjectives.OrderBy(a => Guid.NewGuid()).ToList();

        List<string> aboutYou = new List<string>
        {
            "We expect you to be " + adjectives[0] + ", " + adjectives[1] + " and " + adjectives[2] + ". ",
            "We seek " + adjectives[0] + " and " + adjectives[2] + " students. ",
            "Are you a " + adjectives[0] + ", " + adjectives[1] + " and " + adjectives[2] + " individual? ",
            "Do you see yourself as " + adjectives[0] + ", " + adjectives[1] + " and " + adjectives[2] + "? ",
            adjectives[0] + ", " + adjectives[1] + ", " + adjectives[2] + ", " + adjectives[3] + ", " + adjectives[4] + ", " + adjectives[4] + "! These are just few of the attributes we expect you to have! "
        };

        adjectives = adjectives.OrderBy(a => Guid.NewGuid()).ToList();


        List<string> wannaWorkIn = new List<string>
        {
            "Are you willing to be a part of a work environment where everyone is " + adjectives[0] + ", " + adjectives[1] + " and " + adjectives[2] + ", and passionate about " + buzzwords[0] + " and " + buzzwords[1] + "? ",
            "We aim to develop a " + title + " and we want you to help us! Join " + adjectives[0] + ", " + adjectives[1] + " and " + adjectives[2] + " people at " + company + ". ",
            "Do you have a thesis idea that is focusing either on " + buzzwords[0] + " or " + buzzwords[1] + "? Please do not hesitate to reach out! Your research does not necessarily have to fit perfectly into one of our projects - if you have a good idea and you are " +  adjectives[1] + " and " + adjectives[2] + ", we would love to hear it and help you realise it! ",
        };

        List<string> final = new List<string>();

        final.Add(starters[r.Next(0, starters.Count - 1)]);
        final.Add(buzzwords[0] + " and " + buzzwords[1] + "? ");
        final.Add(workWithCompany[r.Next(0, workWithCompany.Count - 1)]);
        final.Add(workWithbuzzwords[r.Next(0, workWithbuzzwords.Count - 1)]);
        final.Add(aboutYou[r.Next(0, aboutYou.Count - 1)]);
        final.Add(wannaWorkIn[r.Next(0, wannaWorkIn.Count - 1)]);


        return string.Join(" ", final);
    }

    public static string RandomCompany()
    {
        // Build a company name with 2 starters and 1 ending
        Random r = new Random();
        List<string> start = new List<string>() {
            "Tex", "Pad", "Swift", "Just", "Fast", "Blur", "Gradient", "Optical", "Polaroid", "All", "Blue", "Red", "Jay", "Super", "Charge", "Dev", "Soft", "Root", "Web", "Smooth", "Easy", "Mind", "Green", "Future", "Factory", "Coffee", "Code", "Monkey", "Tiger", "Lion", "Giraffe", "Look", "Omni", "Bus", "Page", "Widget", "Hq", "Bank", "Software", "Path"
        };

        List<string> ending = new List<string>() {
            " Co", " Inc", " ApS", " AAT", " LLC"
        };

        List<string> final = new List<string>();

        while (final.Count < 3)
        {
            string word = start[r.Next(0, start.Count - 1)];
            if (!final.Contains(word))
            {
                final.Add(word);
            }
        }

        final.Add(ending[r.Next(0, ending.Count - 1)]);

        return string.Join("", final);
    }

    public static Supervisor RandomSupervisor(List<string> existingEmails)
    {
        // Name is build up of 1 first name and 2 surnames
        Random r = new Random();

        Supervisor u = new Supervisor();
        List<string> firstnames = new List<string> { "Lasse", "Anton", "Nikoline", "Tue", "Philip", "Peter", "Asger", "Vilhelm", "Axel", "Lucas", "Alma", "Mille", "Dagmar", "Louise", "Sofie", "Sofia" };
        List<string> surnames = new List<string> { "Klausen", "Burman", "Fuchs", "Bertelsen", "Cronval", "Gyhrs", "Kjærgaard", "Hviid", "Andersen", "Birch", "Dyrholm" };
        List<string> domains = new List<string> { "@hotmail.com", "@gmail.com", "@outlook.com", "@outlook.dk"};
        List<string> final = new List<string>();

        final.Add(firstnames[r.Next(0, firstnames.Count - 1)]);

        while (final.Count < 3)
        {
            string word = surnames[r.Next(0, surnames.Count - 1)];
            if (!final.Contains(word))
            {
                final.Add(word);
            }
        }

        u.Name = string.Join(" ", final);

        // Email is build up of firstname + first surname + @hotmail.com

        var emailName = final[0] + final[1];

        while(existingEmails.Contains(emailName)){
            emailName += r.Next(1, 99);
            System.Console.WriteLine("FOUND A DUPLICATE!");
        }
        System.Console.WriteLine(existingEmails.Count);
        existingEmails.Add(emailName);

        emailName += domains[r.Next(0, domains.Count - 1)];
        u.Email = emailName;
        System.Console.WriteLine(emailName);

        return u;
    }

    public static string RandomTitle()
    {
        // Title is build up of 2 buzzwords and 1 ending.
        Random r = new Random();

        List<string> buzzwords = new List<string> { "Rolling-Wave_planning", "Agile_and_Lean", "Blockchain", "Performance_enhancing", "NFT", "Database_Management", "Maximizing", "Profit_Obtaining", "Metaverse", "Photoscanning", "Research", "Development", "Algorithm", "API", "KTP", "MVC", "MVVM", "DNA", "Smart", "Web_3", "Wi-Fi", "Internet_of_things", "Backend", "Frontend", "Bugs", "Code", "HTML", "CSS", "Java", "C_", "C++", "Javascript", "Web", "Cache", "Golang", "gRPC", "Byzantine_Generals_Problem", "Debugging", "Deployment", "Chess", "Testing", "Test_Driven_Development", "Documentation", "Domain", "Framework", "Git", "Github", "BitBucket", "REST", "HTTPS", "Information_Architecture", "Language", "Minification", "Library", "Mobilefirst", "LINQ", "Data", "MySQL", "MongoDB", "PHP", "Operating_System", "Plugin", "Responsive_Design", "UX_Design", "Bug-fixing", "UI", "Version_Control", "Web" };
        List<string> endwords = new List<string> { "Study", "Thesis", "Activity", "Program", "Project", "Entrepreneurship" };

        List<string> final = new List<string>();


        while (final.Count < 2)
        {
            string word = buzzwords[r.Next(0, buzzwords.Count - 1)];
            if (!final.Contains(word))
            {
                final.Add(word);
            }
        }
        final.Add(endwords[r.Next(0, endwords.Count - 1)]);

        return string.Join(" ", final.ToArray());
    }
}