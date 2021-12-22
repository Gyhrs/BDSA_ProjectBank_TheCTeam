namespace MyApp.Infrastructure.Model;

public class UserRepository : IUserRepository
{
    private readonly IStudyBankContext _context;

    public UserRepository(IStudyBankContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<UserDTO>> GetAllUsersAsync()
    {
        List<UserDTO> users = new List<UserDTO>();

        List<StudyBankUser> studybankUsers = await _context.Users.ToListAsync();

        foreach (var u in studybankUsers)
        {
            if (u is Student s)
            {
                users.Add(
                    new StudentDTO
                    (
                        s.Email,
                        s.Name,
                        s.Program,
                        s.Project.Id
                    )
                );
            }
            else if (u is Supervisor su)
            {
                users.Add(
                    new SupervisorDTO
                    (
                        su.Email,
                        su.Name,
                        su.Projects.Select(p => p.Id).ToList()
                    )
                );
            }
        }

        return users.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<StudentDTO>> GetStudentsFromProjectIDAsync(int projectId)
    {
        return (await _context.Users.OfType<Student>()
            .Where(s => s.Project.Id == projectId)
            .Select(s => new StudentDTO(s.Email, s.Name, s.Program, s.Project.Id))
            .ToListAsync())
        .AsReadOnly();
    }

    public async Task<IReadOnlyCollection<SupervisorDTO>> GetSupervisorsFromProjectIDAsync(int projectId)
    {
         return (await _context.Users.OfType<Supervisor>()
            .Where(s => s.Projects.Select(p => p.Id).Contains(projectId))
            .Select(s => new SupervisorDTO(s.Email, s.Name, s.Projects.Select(p => p.Id).ToList()))
            .ToListAsync())
        .AsReadOnly();
    }

    public async Task<UserDTO> GetUserFromEmailAsync(string userEmail)
    {
        var user = (await _context.Users.Where(u => u.Email == userEmail).FirstOrDefaultAsync());

        if (user is Student s)
        {
            return new StudentDTO
            (
                s.Email,
                s.Name,
                s.Program,
                s.Project.Id
            );
        }
        else if (user is Supervisor su)
        {
            return new SupervisorDTO(
            su.Email,
            su.Name,
            su.Projects.Select(p => p.Id).ToList()
            );
        }
        return null;
    }
}
