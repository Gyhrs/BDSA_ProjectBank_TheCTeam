using System.Collections;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure;
using System;
using System.Collections.Generic;

public static class SeedExtensions
{
    public static IHost Seed(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<StudyBankContext>();

            SeedCharacters(context);
        }
        return host;
    }

    private static void SeedCharacters(StudyBankContext context)
    {

        if (!context.Projects.Any())
        {

            List<Project> projects = new List<Project>();
            for (int i = 0; i < 10; i++)
            {
                projects.Add(RandomProject());
            }

            foreach (var p in projects)
            {
                Console.WriteLine("---- Project -----");

                Console.WriteLine(p.Name);
                Console.WriteLine(p.Description);
                Console.WriteLine("By: " + p.CreatedBy.Name + " " + p.CreatedBy.Email);
                Console.WriteLine("Startdate: " + p.StartDate.ToString("dd/MM/yyyy") + "\nEnd date: " + p.EndDate.ToString("dd/MM/yyyy"));
                Console.WriteLine("Supervisors: ");
                foreach (var s in p.Supervisors)
                {
                    Console.WriteLine(s.Name + " " + s.Email);
                }

                Console.WriteLine();
            }
        }
    }

    public static Project RandomProject()
    {
        Project p = new Project();

        // Create random title, start/end date and CreatedBy Supervisor
        p.Name = RandomTitle();
        p.StartDate = DateTime.Parse("1/2/2022");
        p.EndDate = DateTime.Parse("24/6/2022");
        p.CreatedBy = RandomSupervisor();
        p.Supervisors = new List<Supervisor>();

        // Create 1-3 random super visor for the project
        Random r = new Random();
        for (int i = 0; i < r.Next(1, 3); i++)
        {
            p.Supervisors.Add(RandomSupervisor());
        }

        p.Description = RandomDescription(p.Name);

        return p;
    }

    public static string RandomDescription(string title)
    {
        // Description is build up of 1 starter, title + ?. Then a section about the firm, and then a conclusion
        Random r = new Random();

        List<string> starters = new List<string> { "Are you about to write your master’s thesis or a similar project and interested in exploring",
                                                   "Have an interesting idea involving",
                                                   "Do you wanna grow and explore the market of",
                                                   "Have you been wondering how to improve the current state of",
                                                   "Are you interested in exploring the future of"};
        List<string> final = new List<string>();

        final.Add(starters[r.Next(0, starters.Count - 1)]);
        final.Add(title + "?");

        return string.Join(" ", final);
    }

    public static Supervisor RandomSupervisor()
    {
        // Name is build up of 1 first name and 2 surnames
        Random r = new Random();

        Supervisor u = new Supervisor();
        List<string> firstnames = new List<string> { "Lasse", "Anton", "Nikoline", "Tue", "Philip", "Peter", "Asger", "Vilhelm", "Axel", "Lucas", "Alma", "Mille", "Dagmar", "Louise", "Sofie", "Sofia" };
        List<string> surnames = new List<string> { "Klausen", "Burman", "Fuchs", "Bertelsen", "Cronval", "Gyrs", "Kjærgaard", "Hviid", "Andersen", "Birch", "Dyrholm" };

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
        u.Email = final[0] + final[1] + "@hotmail.com";

        return u;
    }

    public static string RandomTitle()
    {
        // Title is build up of 2 buzzwords and 1 ending.
        Random r = new Random();

        List<string> buzzwords = new List<string> { "Rolling-Wave planning", "Agile and Lean", "Blockchain", "Performance enhancing", "NFT", "Database Management", "Maximizing", "Profit Obtaining", "Metaverse", "Photoscanning", "Research", "Development", "Algorithm", "API", "KTP", "MVC", "MVVM", "DNA", "Smart", "Web 3", "Wi-Fi", "Internet of things", "Internet of", "Backend", "Frontend", "Bugs", "Code", "HTML", "CSS", "Java", "C#", "C++", "Javascript", "Web", "Cache", "Golang", "gRPC", "Byzantine Generals Problem", "Debugging", "Deployment", "Chess", "Testing", "Test driven Development", "Documentation", "Domain", "Framework", "Git", "Github", "BitBucket", "REST", "HTTPS", "Information Architecture", "Language", "Minification", "Library", "Mobilefirst", "LINQ", "Data", "MySQL", "MongoDB", "PHP", "Operating System", "Plugin", "Responsive Design", "UX Design", "UI", "Version Control", "Web" };
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