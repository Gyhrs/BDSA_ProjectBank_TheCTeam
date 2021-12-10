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
        //context.Database.Migrate();
        // context.Database.ExecuteSqlRaw("DELETE dbo.Projects");
        // context.Database.ExecuteSqlRaw("DELETE dbo.ProjectSupervisor");
        // context.Database.ExecuteSqlRaw("DELETE dbo.ProjectTag");
        // context.Database.ExecuteSqlRaw("DELETE dbo.Users");
        // context.Database.ExecuteSqlRaw("DELETE dbo.Tags");
        //context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Projects', RESEED, 0)");
        
        PopulateDatabase(context);
    }
    ///<summary> 
    ///Default version of PopulateDatabase method with 50 Supervisors, 200 Projects, and 1-7 Tags per Project. 
    ///Adds the 200 randomly generated Projects (with 50 randomly assigned Supervisors, and a number of Tags) and 
    ///Tags (that are assigned to the corresponding Projects) to the context, and saves the context.
    ///</summary>
    public static void PopulateDatabase(StudyBankContext context) 
    {
        PopulateDatabase(context, 50, 200, 1, 7);
    }

    ///<summary> 
    ///Adds randomly generated Projects (with randomly assigned Supervisors and Tags) and 
    /// Tags (that are assigned corresponding Projects) to the context, and saves the context.
    ///</summary>
    ///<param name="context">the StudyBankContext to be modified</param>
    ///<param name="sNum">the number of generated Supervisors</param>
    ///<param name="pNum">the number of generated Projects</param>
    ///<param name="minTags">the minimum number of Tags per Project</param>
    ///<param name="maxTags">the maximum number of Tags per Project</param>
    public static void PopulateDatabase(StudyBankContext context, int sNum, int pNum, int minTags, int maxTags)
    {
        List<Supervisor> supervisors = GenerateSupervisors(sNum);
        List<Project> projects = GenerateProjects(pNum, supervisors);
        List<Tag> tags = GetTags();

        Random r = new Random();

        foreach (var p in projects) 
        {
            // Select a number of random tags for the project
            for (int i = 0; i < r.Next(minTags, maxTags+1); i++)
            {
                Tag t = tags[r.Next(0, tags.Count - 1)];
                p.Tags.Add(t); 
                t.Projects.Add(p);
            }

            context.Projects.Add(p);
        }

        foreach (var t in tags)
        {
            context.Tags.Add(t);
        }
        context.SaveChanges();
    }


    /// <summary>
    /// Build a specified number of supervisors
    /// </summary>
    private static List<Supervisor> GenerateSupervisors(int amount)
    {
        List<Supervisor> supervisors = new List<Supervisor>();
        List<string> existingEmails = new List<string>();

        for (int i = 0; i < amount; i++)
        {
            supervisors.Add(GenerateRandomSupervisor(existingEmails));
        }
        
        return supervisors;
    }

    /// <summary>
    /// Build a specified number of projects, all associated with at least one of the generated supervisors.
    /// </summary>
    private static List<Project> GenerateProjects(int amount, List<Supervisor> supervisors)
    {
        List<Project> projects = new List<Project>();

        for (int i = 0; i < amount; i++)
        {
            projects.Add(GenerateRandomProject(supervisors));
        }

        return projects;
    }

    /// <summary>
    /// Build one random project. Will use randomly generated buzzwords to make a title and description, will have a start/ end date and a 
    /// random CreatedBy supervisor. There are 1-3 random supervisors associated with a project.
    /// </summary>
    private static Project GenerateRandomProject(List<Supervisor> supervisors)
    {
        Project project = new Project();
        Random r = new Random();

        List<string> randomWords = SelectTwoRandomBuzzwords(); 
        string title = GenerateTitle(randomWords);

        project.Name = title;
        project.StartDate = DateTime.Parse("1/2/2022");
        project.EndDate = DateTime.Parse("24/6/2022");
        project.CreatedBy = supervisors[r.Next(0, supervisors.Count - 1)];
        project.Description = GenerateRandomDescription(randomWords, title);
        
        // Select 1-3 random supervisors for the project
        project.Supervisors = new List<Supervisor>();
        for (int i = 0; i < r.Next(1, 3); i++)
        {
            project.Supervisors.Add(supervisors[r.Next(0, supervisors.Count - 1)]);
        }

        return project;
    }

    ///<summary>
    /// Iterates through a list of strings to create tags with a corresponding tagname. Returns a List<Tag>
    ///</summary>
    private static List<Tag> GetTags()
    {
        List<string> tagNames = new List<string>
        {
            "AccessibleDesign", "Agile", "AI", "Algorithms", "Art", "Blazor", "Business", "C#", 
            "Computing", "Consulting", "CSS", "Cyber-security", "Design", "Development", "Digital", 
            "Discord", "Encryption", "F#", "GDPR", "Go", "Golang", "Hacking", "Hands-on", "Hashing", 
            "HTML", "Innovation", "Integration", "Java", "Lean", "MachineLearning", "Microsoft", 
            "NoSQL", "Online", "OS", "Partnership", "Profit", "Python", "SASS", "SCRUM", "Software", 
            "SOLID", "SQL", "SqlLite", "Testdriven", "UI", "Usability", "UserFriendly", "UserInterface", 
            "UX", "VideoGames", "XML", "Zoom"
        };

        List<Tag> tags = new List<Tag>();

        foreach (string tagName in tagNames) 
        {
            Tag t = new Tag();
            t.Name = tagName;
            tags.Add(t);
        }
        return tags;
    }

    ///<summary>
    /// Description is build up of intro, some lines that use the buzzwords, then info about the company, and a conclusion.
    ///</summary>
    private static string GenerateRandomDescription(List<string> buzzwords, string title)
    {
        Random r = new Random();
        string company = GenerateRandomCompany();

        List<string> intro = new List<string> 
        { 
            "Are you about to write your master’s thesis or a similar project and interested in exploring",
            "Have an interesting idea involving",
            "Do you wanna grow and explore the market of",
            "Have you been wondering how to improve the current state of",
            "Are you interested in exploring the future of"
        };

        List<string> workWithCompany = new List<string>
        { 
            "Then join us at " + company + "! ",
            "We're interested in working with you! Help us at " + company + ". ",
            "We're looking for students to help us at " + company + ". ",
            "At " + company + " we invent the future. Join us! ",
            "By: " + company + ". "
        };

        List<string> workWithbuzzwords = new List<string>
        { 
            "Work with " + buzzwords[0] + " and " + buzzwords[1] + " alongside talented individuals at " + company + " and help invent the future! ",
            "Do you wanna work with " + buzzwords[0] + " and " + buzzwords[1] + "?" ,
            "Can you see yourself working with " + buzzwords[0] + " and " + buzzwords[1] + "? ",
            "Do you like " + buzzwords[1] + "? What about " + buzzwords[0] + "? ",
            "Help us at " + company + " working with " + buzzwords[0] + " and " + buzzwords[1] + ". ",
            "Come write your master’s thesis about " + title + " at " + company + ". "
        };

        List<string> adjectives = new List<string>
        {
            "adventurous", "ambitious", "audacious", "beautiful", "boy wonder", "brainy", "bright", 
            "clever", "creative", "curious", "enthusiastic", "fearless", "ferocious", "genius", 
            "hardworking", "intelligent", "interesting", "likeable", "not stupid", "passionate", 
            "perceptive", "savvy", "scientist", "self-driven", "sharp", "smart", "strong", "stylish", 
            "team player"
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

        List<string> fullDescription = new List<string>();

        fullDescription.Add(intro[r.Next(0, intro.Count - 1)]);
        fullDescription.Add(buzzwords[0] + " and " + buzzwords[1] + "? ");
        fullDescription.Add(workWithCompany[r.Next(0, workWithCompany.Count - 1)]);
        fullDescription.Add(workWithbuzzwords[r.Next(0, workWithbuzzwords.Count - 1)]);
        fullDescription.Add(aboutYou[r.Next(0, aboutYou.Count - 1)]);
        fullDescription.Add(wannaWorkIn[r.Next(0, wannaWorkIn.Count - 1)]);

        return string.Join(" ", fullDescription);
    }

    
    /// <summary>
    /// Build a company name with 2 starters and 1 ending
    /// </summary>
    private static string GenerateRandomCompany()
    {
        Random r = new Random();
        List<string> companyName = new List<string>();
        
        List<string> firstParts = new List<string>() 
        {
            "All", "Bank", "Blue", "Blur", "Bus", "Charge", "Code", "Coffee", "Dev", "Easy", "Factory", 
            "Fast", "Future", "Giraffe", "Gradient", "Green", "Hq", "Jay", "Just", "Lion", "Look", "Mind", 
            "Modern", "Monkey", "Omni", "Optical", "Pad", "Page", "Path", "Polaroid", "Red", "Root", 
            "Smooth", "Soft", "Software", "Super", "Swift", "Tex", "Tiger", "Web", "Widget"
        };
        List<string> lastParts = new List<string>() { "ApS", "AAT", "Co", "Inc", "LLC" };

        while (companyName.Count < 3)
        {
            string word = firstParts[r.Next(0, firstParts.Count - 1)];
            if (!companyName.Contains(word))
            {
                companyName.Add(word);
            }
        }

        companyName.Add(" " + lastParts[r.Next(0, lastParts.Count - 1)]);

        return string.Join("", companyName);
    }

    /// <summary>
    /// A Supervisor is generated and given an unique email. Returns supervisor.
    /// </summary>
    private static Supervisor GenerateRandomSupervisor(List<string> existingEmails)
    {
        // Name is build up of 1 first name and 2 surnames
        Random r = new Random();
        Supervisor supervisor = new Supervisor();
        List<string> firstNames = new List<string> 
        { 
            "Alma", "Anton", "Asger", "Axel", "Dagmar", "Lasse", "Louise", "Lucas", "Mille", "Nikoline", 
            "Peter", "Philip", "Sofia", "Sofie", "Tue", "Vilhelm"
        };
        List<string> surnames = new List<string> 
        { 
            "Andersen", "Bertelsen", "Birch", "Burman", "Cronval", "Dyrholm", "Fuchs", "Gyhrs", "Hviid", 
            "Kjærgaard", "Klausen"
        };
        List<string> domains = new List<string> { "@gmail.com", "@hotmail.com", "@outlook.com", "@outlook.dk"};
        List<string> fullName = new List<string>();
        fullName.Add(firstNames[r.Next(0, firstNames.Count - 1)]);
        while (fullName.Count < 3)
        {
            string word = surnames[r.Next(0, surnames.Count - 1)];
            if (!fullName.Contains(word))
            {
                fullName.Add(word);
            }
        }

        supervisor.Name = string.Join(" ", fullName);

        // Email is build up of firstname + first surname + @hotmail.com
        var emailName = fullName[0] + fullName[1];

        while(existingEmails.Contains(emailName)){
            emailName += r.Next(1, 99);
            System.Console.WriteLine("FOUND A DUPLICATE!");
        }
        System.Console.WriteLine(existingEmails.Count);
        existingEmails.Add(emailName);

        emailName += domains[r.Next(0, domains.Count - 1)];
        supervisor.Email = emailName;
        System.Console.WriteLine(emailName);

        return supervisor;
    }

    /// <summary>
    /// Using the random buzzwords and an additional projectType string, returns a string
    /// </summary>
    private static string GenerateTitle(List<string> buzzwords)
    {
        Random r = new Random();
        List<string> projectTypes = new List<string> 
        { 
            "Activity", "Entrepreneurship", "Program", "Project", "Study", "Thesis"
        };
        
        return string.Join(" ", buzzwords) + " " + projectTypes[r.Next(0, projectTypes.Count - 1)];
    }
    /// <summary>
    /// Select two random buzzwords, returns a List<string>
    /// </summary>
    private static List<string> SelectTwoRandomBuzzwords()
    {
        Random r = new Random();
        List<string> buzzwords = new List<string> 
        { 
            "Accessibility", "Agile and Lean", "Algorithm", "API", "Backend", "BitBucket", "Blazor", 
            "Blockchain", "Bug-fixing", "Bugs", "Byzantine Generals Problem", "C#", "C++", "Cache", 
            "Chess", "Code", "CSS", "Data", "Database Management", "Debugging", "Deployment", "Development", 
            "DNA", "Documentation", "Domain", "Framework", "Frontend", "Git", "Github", "Golang", "gRPC", 
            "HTML", "HTTPS", "Information Architecture", "Internet of things", "Java", "Javascript", "KTP", 
            "Language", "Library", "LINQ", "Maximizing", "Metaverse", "Minification", "Mobilefirst", "MongoDB", 
            "MVC", "MVVM", "MySQL", "NFT", "Operating System", "Performance enhancing", "Photoscanning", "PHP", 
            "Plugin", "Profit Obtaining", "Research", "Responsive Design", "REST", "Rolling-Wave planning", 
            "Smart", "Test Driven Development", "Testing", "UI", "Usability", "UX Design", "Version Control", 
            "Web", "Web", "Web 3", "Wi-Fi"
        };
        List<string> chosenWords = new List<string> {};
        while (chosenWords.Count < 2)
        {
            string word = buzzwords[r.Next(0, buzzwords.Count - 1)];
            if (!chosenWords.Contains(word))
            {
                chosenWords.Add(word);
            }
        }
        
        return chosenWords;
    }
}