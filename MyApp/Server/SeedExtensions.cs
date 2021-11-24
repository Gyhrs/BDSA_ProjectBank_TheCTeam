using System.Collections;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure;
using System;

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

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(RandomTitle());
            }
        }
    }

    public static string RandomTitle() {
        List<string> buzzwords = new List<string>{"Rolling-Wave planning", "Agile and Lean", "Blockchain", "Performance enhancing", "NFT", "Database Management", "Maximizing", "Profit Obtaining", "Metaverse", "Photoscanning", "Research", "Development", "Algorithm", "API", "KTP", "MVC", "MVVM", "DNA", "Smart", "Web 3", "Wi-Fi", "Internet of things", "Internet of", "Backend", "Frontend", "Bugs", "Code", "HTML", "CSS", "Java", "C#", "C++", "Javascript", "Web", "Cache", "Golang", "gRPC", "Byzantine Generals Problem", "Debugging", "Deployment", "Chess", "Testing", "Test driven Development", "Documentation", "Domain", "Framework", "Git", "Github", "BitBucket", "REST", "HTTPS", "Information Architecture", "Language", "Minification", "Library", "Mobilefirst", "LINQ", "Data", "MySQL", "MongoDB", "PHP", "Operating System", "Plugin", "Responsive Design", "UX Design", "UI", "Version Control", "Web"};
        List<string> endwords = new List<string>{"Study", "Thesis", "Activity", "Program", "Project", "Entrepreneurship"};

        List<string> final = new List<string>();

        Random r = new Random();

        while (final.Count < 3) {
            string word = buzzwords[r.Next(0, buzzwords.Count - 1)];
            if(!final.Contains(word))
            {
                final.Add(word);
            }
        }
        final.Add(endwords[r.Next(0, endwords.Count - 1)]);
        
        return string.Join(" ", final.ToArray());
    }
}