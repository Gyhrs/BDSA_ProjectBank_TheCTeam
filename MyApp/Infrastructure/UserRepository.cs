using Microsoft.EntityFrameworkCore;
using MyApp.Shared;
using System.Linq;

namespace MyApp.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly IStudyBankContext _context;

    public UserRepository(IStudyBankContext context)
    {
        _context = context;
    }

    public Task<IReadOnlyCollection<UserDTO>> GetAllUsersAsync()
    {
        //Await simply allows the application to perform other actions while it waits for a response from the database.
        throw new NotImplementedException();
        
    }

    public Task<UserDTO> GetUserFromEmailAsync(string userEmail)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<UserDTO>> GetUsersFromProjectIDAsync(int projectId)
    {
        throw new NotImplementedException();
    }
}
