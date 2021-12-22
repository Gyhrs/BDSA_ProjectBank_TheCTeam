namespace MyApp.Infrastructure;
public abstract class StudyBankUser
{
    [EmailAddress]
    [Key]
    public string Email { get; set; }

    [StringLength(50)]
    public string Name { get; set; }
}
