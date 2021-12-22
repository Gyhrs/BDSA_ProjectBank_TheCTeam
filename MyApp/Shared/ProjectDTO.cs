namespace MyApp.Shared;
public record ProjectDTO
(
    int Id,
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    string Description,
    List<string>? StudentEmails,
    List<string>? SupervisorsEmails,
    string? CreatedByEmail,
    string? CreatedBy,
    List<string>? Tags
);

public record ProjectCreateDTO
{
    [StringLength(100)]
    public string? Name {get; set;}
    public DateTime StartDate {get; set;}
    public DateTime EndDate {get; set;}
    public string? Description {get; set;}
    public List<string>? StudentEmails {get; set;}
    public List<string>? SupervisorsEmails {get; set;}
    public string? CreatedByEmail {get; set;}
    public List<string>? Tags {get; set;}
}
public record ProjectUpdateDTO : ProjectCreateDTO
{
    public int Id { get; set; }
}