namespace MyApp.Infrastructure.Core;

public class Project
{
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public String? Description { get; set; }
    public List<Student>? Students { get; set; }
    public List<Supervisor>? Supervisors { get; set; }
    public StudyBankUser? CreatedBy { get; set; }
    public List<Tag> Tags { get; set; } = new List<Tag>();
}
