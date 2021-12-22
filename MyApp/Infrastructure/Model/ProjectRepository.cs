namespace MyApp.Infrastructure.Model;
public class ProjectRepository : IProjectRepository
{
    private readonly IStudyBankContext _context;

    public ProjectRepository(IStudyBankContext context)
    {
        _context = context;
    }

    /// <summary>
    /// GetAllProjects() returns an async read-only list of ProjectDTO's of all projects in the DB.
    /// </summary> 
    public async Task<IReadOnlyCollection<ProjectDTO>> GetAllProjectsAsync()
    {
        // Await simply allows the application to perform other actions while it waits for a response from the database.
        var projects = await (
                        from p in _context.Projects
                        select new ProjectDTO(
                            p.Id,
                            p.Name,
                            p.StartDate,
                            p.EndDate,
                            p.Description,
                            p.Students != null ? p.Students.Select(s => s.Email).ToList() : null,
                            p.Supervisors != null ? p.Supervisors.Select(s => s.Email).ToList() : null,
                            p.CreatedBy != null ? p.CreatedBy.Email : null,
                            p.CreatedBy != null ? p.CreatedBy.Name : null,
                            p.Tags != null ? p.Tags.Select(t => t.Name).ToList() : null
                        )).ToListAsync();
        return projects.AsReadOnly();
    }

    ///<summary>
    /// GetProjectFromId finds a project in the datbase, with the given ID.
    ///</summary>
    public async Task<ProjectDTO> GetProjectFromIDAsync(int projectId)
    {
        var projects = from p in _context.Projects
                       where p.Id == projectId
                       select new ProjectDTO(
                           p.Id,
                           p.Name,
                           p.StartDate,
                           p.EndDate,
                           p.Description,
                           p.Students != null ? p.Students.Select(s => s.Email).ToList() : null,
                           p.Supervisors != null ? p.Supervisors.Select(s => s.Email).ToList() : null,
                           p.CreatedBy != null ? p.CreatedBy.Email : null,
                           p.CreatedBy != null ? p.CreatedBy.Name : null,
                           p.Tags != null ? p.Tags.Select(t => t.Name).ToList() : null
                       );
        return await projects.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<ProjectDTO>> GetProjectsFromTagsAsync(List<string> searchTags)
    {
        // Finds all tags that contain a searchTag. E.g. if you have searched for "UI" and "AI" we find [UI, AI]
        // which points to respective lists (UI -> [p1, p2, p3], etc.) 
        var tags = await _context.Tags.Where(t => searchTags.Any(ttag => ttag == t.Name))
        .Include(t => t.Projects)
            .ThenInclude(p => p.CreatedBy)
        .Include( t => t.Projects)
            .ThenInclude(p => p.Tags)
        .ToListAsync();

        var projects = new List<ProjectDTO>();

        // If any tags were found, do this. Otherwise return an empty list.
        if (tags.Count > 0)
        {
            // Add the first tag's list of projects to a list.
            List<Project> list = new List<Project>(tags.First().Projects.ToList());

            // Intersect this list with all the other tag's list of projects
            for (int i = 1; i < tags.Count; i++)
            {
                list = list.Intersect(tags.ElementAt(i).Projects).ToList();
            }

            // For all Project -> ProjectDTO in list.
            projects = list.Select(p => new ProjectDTO(
                                p.Id,
                                p.Name,
                                p.StartDate,
                                p.EndDate,
                                p.Description,
                                p.Students != null ? p.Students.Select(s => s.Email).ToList() : null,
                                p.Supervisors != null ? p.Supervisors.Select(s => s.Email).ToList() : null,
                                p.CreatedBy != null ? p.CreatedBy.Email : null,
                                p.CreatedBy != null ? p.CreatedBy.Name : null,
                                p.Tags != null ? p.Tags.Select(t => t.Name).ToList() : null
                            )).ToList();
        }
        return projects.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<ProjectDTO>> GetProjectsFromNameAsync(string name)
    {


        var projects = await (
                        from p in _context.Projects
                        where p.Name.Contains(name)
                        select new ProjectDTO(
                            p.Id,
                            p.Name,
                            p.StartDate,
                            p.EndDate,
                            p.Description,
                            p.Students != null ? p.Students.Select(s => s.Email).ToList() : null,
                            p.Supervisors != null ? p.Supervisors.Select(s => s.Email).ToList() : null,
                            p.CreatedBy != null ? p.CreatedBy.Email : null,
                            p.CreatedBy != null ? p.CreatedBy.Name : null,
                            p.Tags != null ? p.Tags.Select(t => t.Name).ToList() : null
                        )).ToListAsync();


        return projects.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<ProjectDTO>> GetProjectsFromTagsAndNameAsync(List<string> searchTags, string title)
    {
        var tagProjects = await GetProjectsFromTagsAsync(searchTags);

        return tagProjects.Where(t => t.Name.ToLower().Contains(title.ToLower())).ToList().AsReadOnly();
    }
    private async Task<List<Student>> GetStudentsFromList(List<string> userEmails)
    {
        List<Student> users = new List<Student>();
        foreach (var item in userEmails)
        {
            var user = await _context.Users.OfType<Student>().Where(u => u.Email == item).FirstOrDefaultAsync();
            if (user != null)
            {
                users.Add(user);
            }
        }
        return users;
    }


    public async Task<(Status, ProjectDTO)> CreateProjectAsync(ProjectCreateDTO create)
    {
        var conflict = await _context.Projects.Where
        (
            p => p.Description == create.Description &&
            p.Name == create.Name &&
            p.StartDate == create.StartDate &&
            p.EndDate == create.EndDate
            ).Select(p => new ProjectDTO(
                            p.Id,
                            p.Name,
                            p.StartDate,
                            p.EndDate,
                            p.Description,
                            p.Students != null ? p.Students.Select(s => s.Email).ToList() : null,
                            p.Supervisors != null ? p.Supervisors.Select(s => s.Email).ToList() : null,
                            p.CreatedBy != null ? p.CreatedBy.Email : null,
                            p.CreatedBy != null ? p.CreatedBy.Name : null,
                            p.Tags != null ? p.Tags.Select(t => t.Name).ToList() : null
                        )).FirstOrDefaultAsync();

        if (conflict != null)
        {
            return (Status.Conflict, conflict);
        }

        var entity = new Project
        {
            Name = create.Name,
            StartDate = create.StartDate,
            EndDate = create.EndDate,
            Description = create.Description,
            Students = await GetStudentsFromList(create.StudentEmails),
            Supervisors = await GetSupervisorsFromListAsync(create.SupervisorsEmails),
            CreatedBy = await GetUserFromEmailAsync(create.CreatedByEmail),
            Tags = await GetTagsFromStringListAsync(create.Tags)
        };

        _context.Projects.Add(entity);
        await _context.SaveChangesAsync();

        return (Status.Created, new ProjectDTO(
            entity.Id,
            entity.Name,
            entity.StartDate,
            entity.EndDate,
            entity.Description,
            entity.Students.Select(s => s.Email).ToList(),
            entity.Supervisors.Select(s => s.Email).ToList(),
            entity.CreatedBy.Email,
            entity.CreatedBy.Name,
            entity.Tags.Select(t => t.Name).ToList()
        ));
    }

    public async Task<Status> UpdateProjectAsync(int id, ProjectUpdateDTO project)
    {
        var entity = await _context.Projects.Where(p => p.Id == project.Id).FirstOrDefaultAsync();
        if (entity == null)
        {
            return Status.NotFound;
        }
        entity.Name = project.Name;
        entity.CreatedBy = await GetUserFromEmailAsync(project.CreatedByEmail);
        entity.Description = project.Description;
        entity.StartDate = project.StartDate;
        entity.EndDate = project.EndDate;
        entity.Students = await GetStudentsFromList(project.StudentEmails);
        entity.Supervisors = await GetSupervisorsFromListAsync(project.SupervisorsEmails);
        entity.Tags = await GetTagsFromStringListAsync(project.Tags);

        await _context.SaveChangesAsync();

        return Status.Updated;
    }
    
    public async Task<Status> DeleteProjectAsync(int projectId)
    {
        var entity = await _context.Projects.FindAsync(projectId);

        if (entity == null)
        {
            return Status.NotFound;
        }

        _context.Projects.Remove(entity);
        await _context.SaveChangesAsync();
        return Status.Deleted;
    }

    private async Task<List<Supervisor>> GetSupervisorsFromListAsync(List<string> userEmails)
    {
        List<Supervisor> users = new List<Supervisor>();
        foreach (var item in userEmails)
        {
            var user = await _context.Users.OfType<Supervisor>().Where(u => u.Email == item).FirstOrDefaultAsync();
            if (user != null)
            {
                users.Add(user);
            }
        }
        return users;
    }
    private async Task<StudyBankUser> GetUserFromEmailAsync(string userEmail)
    {
        return await _context.Users.Where(u => u.Email == userEmail).FirstOrDefaultAsync();
    }
    private async Task<List<Tag>> GetTagsFromStringListAsync(List<string> tags)
    {
        List<Tag> list = new List<Tag>();
        foreach (var tag in tags)
        {
            var ta = await _context.Tags.Where(t => t.Name == tag).FirstOrDefaultAsync();
            if (ta != null)
            {
                list.Add(ta);
            }
        }
        return list;
    }

}