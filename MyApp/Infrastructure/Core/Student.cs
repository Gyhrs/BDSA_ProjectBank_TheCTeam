namespace MyApp.Infrastructure.Core;

public class Student : StudyBankUser
{
    [StringLength(50)]
    public string Program { get; set; }

    public Project? Project { get; set; }
}


