namespace MyApp.Infrastructure.Core;
public class Tag
{
    [Key]
    public string Name { get; set; }
    public List<Project> Projects { get; set; } = new List<Project>();
}
