public class ProjectsControllerTests
{
    [Fact]
    public async Task GetAllAsync_returns_Ok()
    {
        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.GetAllProjectsAsync()).ReturnsAsync(Array.Empty<ProjectDTO>());
        var controller = new ProjectsController(logger.Object, repository.Object);


        // Act
        var actual = await controller.GetAllAsync();

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromIdAsync_Returns_NotFound_Given_Nonexisting_ProjectId()
    {

        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.GetProjectFromIDAsync(101)).ReturnsAsync(default(ProjectDTO)); // "For a reference-type it returns null"
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromIdAsync(101);

        // Assert
        Assert.IsType<NotFoundObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromIdAsync_Returns_BadRequest_Given_Negative_ProjectId()
    {

        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.GetProjectFromIDAsync(-1)).ReturnsAsync(default(ProjectDTO)); // "For a reference-type it returns null"
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromIdAsync(-1);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromIdAsync_Returns_Ok_Given_existing_ProjectId()
    {

        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();

        var project = new ProjectDTO
        (
            1,
            "Blockchain",
            DateTime.ParseExact("26/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            "This is a amazing project working with talented people.",
            new List<string> { "anton@hotmail.com", "nibu@hotmail.com" },
            new List<string> { "andasdton@hotmail.com", "nibabjksdfu@hotmail.com" },
            "createdby@hotmail.com",
            "Lars",
            new List<string> { "Business", "Blockchain"}
        );
        repository.Setup(m => m.GetProjectFromIDAsync(1)).ReturnsAsync(project);
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromIdAsync(1);

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }

    
    [Fact]
    public async Task GetFromNameAsync_Returns_NotFound_Given_Nonexisting_ProjectName()
    {

        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.GetProjectsFromNameAsync("Blockchain")).ReturnsAsync(Array.Empty<ProjectDTO>); // "For a reference-type it returns null"
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromNameAsync("Blockchain");

        // Assert
        Assert.IsType<NotFoundObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromNameAsync_Returns_BadRequest_Given_Empty_ProjectName()
    {

        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.GetProjectsFromNameAsync("Blockchain")).ReturnsAsync(Array.Empty<ProjectDTO>); // "For a reference-type it returns null"
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromNameAsync("");

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromNameAsync_Returns_Ok_Given_Existing_ProjectName()
    {

        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();

        var project = new ProjectDTO
        (
            1,
            "Blockchain",
            DateTime.ParseExact("26/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            "This is a amazing project working with talented people.",
            new List<string> { "anton@hotmail.com", "nibu@hotmail.com" },
            new List<string> { "andasdton@hotmail.com", "nibabjksdfu@hotmail.com" },
            "createdby@hotmail.com", 
            "Lars",
            new List<string> { "Business", "Blockchain"}
        );
        repository.Setup(m => m.GetProjectsFromNameAsync("Blockchain")).ReturnsAsync(new List<ProjectDTO> { project });
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromNameAsync("Blockchain");

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromTagsAsync_Returns_Ok_Given_Existing_Tag()
    {

        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();

        var projects = new List<ProjectDTO>() {
            new ProjectDTO
            (
                1,
                "Blockchain",
                DateTime.ParseExact("26/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                "This is a amazing project working with talented people.",
                new List<string> { "anton@hotmail.com", "nibu@hotmail.com" },
                new List<string> { "andasdton@hotmail.com", "nibabjksdfu@hotmail.com" },
                "createdby@hotmail.com",
                "Lars",
                new List<string> {"Blockchain", "Business"}

            ),
            new ProjectDTO
            (
                2,
                "Metaverse",
                DateTime.ParseExact("26/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                "This is a amazing project working with talented people.",
                new List<string> { "lasse@hotmail.com", "peter@hotmail.com" },
                new List<string> { "hej@med.com", "dig@hotmail.com" },
                "createdby@hotmail.com",
                "Lars",
                new List<string> { "Business", "BlockChain", "Fast"}

            )
        };
        repository.Setup(m => m.GetProjectsFromTagsAsync(new List<string> { "Business", "Blockchain"} )).ReturnsAsync(projects);
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromTagsAsync("Business_Blockchain");

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromTagsAsync_Returns_BadRequest_Given_Empty_Tag_String()
    {

        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();

        repository.Setup(m => m.GetProjectsFromTagsAsync(new List<string> { "" } )).ReturnsAsync(Array.Empty<ProjectDTO>);
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromTagsAsync("");

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromTagsAsync_Returns_NotFound_Given_Nonexistent_Tag()
    {

        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();

        repository.Setup(m => m.GetProjectsFromTagsAsync(new List<string> { "Testing" })).ReturnsAsync(Array.Empty<ProjectDTO>);
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromTagsAsync("Testing");

        // Assert
        Assert.IsType<NotFoundObjectResult>(actual.Result);
    }

     [Fact]
    public async Task CreateProjectAsync_Returns_Created_Given_Valid_Data()
    {
        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();

        var inputProject = new ProjectCreateDTO()
        {
            CreatedByEmail = "AntonBertelsen@hotmail.com",
            Description = "A project made by Lars",
            EndDate = DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            StartDate = DateTime.ParseExact("23/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Name = "Lars Project",
            StudentEmails = new List<string>() {"nibu@itu.dk", "lakl@itu.dk", "tugy@itu.dk"},
            SupervisorsEmails = new List<string>() {"phcr@itu.dk", "palo@itu.dk"},
            Tags = new List<string>() {"UI", "Business"}
        };

        var outputProject = new ProjectDTO
        (
            5, 
            "Lars Project", 
            DateTime.ParseExact("23/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            "A project made by Lars",
            new List<string>() {"nibu@itu.dk", "lakl@itu.dk", "tugy@itu.dk"},
            new List<string>() {"phcr@itu.dk", "palo@itu.dk"},
            "AntonBertelsen@hotmail.com",
            "Anton",
            new List<string>() {"UI", "Business"}
        );

        repository.Setup(m => m.CreateProjectAsync(inputProject)).ReturnsAsync((Status.Created, outputProject));
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.CreateProjectAsync(inputProject);

        // Assert
        Assert.IsType<CreatedResult>(actual.Result);
    }

    [Fact]
    public async Task CreateProjectAsync_Returns_BadRequest_Given_Invalid_Data()
    {
        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();

        var inputProject = new ProjectCreateDTO()
        {
            CreatedByEmail = "",
            Description = "",
            EndDate = DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            StartDate = DateTime.ParseExact("23/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Name = "Lars Project",
            StudentEmails = new List<string>() {"nibu@itu.dk", "lakl@itu.dk", "tugy@itu.dk"},
            SupervisorsEmails = new List<string>() {"phcr@itu.dk", "palo@itu.dk"},
            Tags = new List<string>() {"UI", "Business"}
        };

        var outputProject = new ProjectDTO
        (
            5, 
            "Lars Project", 
            DateTime.ParseExact("23/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            "A project made by Lars",
            new List<string>() {"nibu@itu.dk", "lakl@itu.dk", "tugy@itu.dk"},
            new List<string>() {"phcr@itu.dk", "palo@itu.dk"},
            "AntonBertelsen@hotmail.com",
            "Anton",
            new List<string>() {"UI", "Business"}
        );

        repository.Setup(m => m.CreateProjectAsync(inputProject)).ReturnsAsync((Status.Created, outputProject));
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.CreateProjectAsync(inputProject);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual.Result);
    }
    [Fact]
    public async Task UpdateProjectAsync_Returns_BadRequest_Given_Bad_Input()
    {
        //Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();

        var inputProject = new ProjectUpdateDTO()
        {
            Id = 20,
            CreatedByEmail = "",
            Description = "",
            EndDate = DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            StartDate = DateTime.ParseExact("23/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Name = "Lars Project",
            StudentEmails = new List<string>() {"nibu@itu.dk", "lakl@itu.dk", "tugy@itu.dk"},
            SupervisorsEmails = new List<string>() {"phcr@itu.dk", "palo@itu.dk"},
            Tags = new List<string>() {"UI", "Business"}
        };

        repository.Setup(m => m.UpdateProjectAsync(20, inputProject)).ReturnsAsync(Status.BadRequest);
        var controller = new ProjectsController(logger.Object, repository.Object);

        //Act
        var actual = await controller.UpdateProjectAsync(20, inputProject);

        //Assert
        Assert.IsType<BadRequestObjectResult>(actual.Result);
    }
    [Fact]
    public async Task UpdateProjectAsync_Returns_NotFound_Given_Invalid_ID()
    {
        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();

        var inputProject = new ProjectUpdateDTO()
        {
            Id = 42,
            CreatedByEmail = "anton@bertelsen.com",
            Description = "Project by Anton",
            EndDate = DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            StartDate = DateTime.ParseExact("23/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Name = "Lars Project",
            StudentEmails = new List<string>() {"nibu@itu.dk", "lakl@itu.dk", "tugy@itu.dk"},
            SupervisorsEmails = new List<string>() {"phcr@itu.dk", "palo@itu.dk"},
            Tags = new List<string>() {"UI", "Business"}
        };

        repository.Setup(m => m.UpdateProjectAsync(42, inputProject)).ReturnsAsync(Status.NotFound);
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.UpdateProjectAsync(42, inputProject);

        // Assert
        Assert.IsType<NotFoundObjectResult>(actual.Result);
    }
    [Fact]
    public async Task UpdateProjectAsync_Returns_OK_Given_Valid_Parameters()
    {
        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();

        var inputProject = new ProjectUpdateDTO()
        {
            Id = 42,
            CreatedByEmail = "anton@bertelsen.com",
            Description = "Project by Anton",
            EndDate = DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            StartDate = DateTime.ParseExact("23/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Name = "Lars Project",
            StudentEmails = new List<string>() {"nibu@itu.dk", "lakl@itu.dk", "tugy@itu.dk"},
            SupervisorsEmails = new List<string>() {"phcr@itu.dk", "palo@itu.dk"},
            Tags = new List<string>() {"UI", "Business"}
        };

        repository.Setup(m => m.UpdateProjectAsync(42, inputProject)).ReturnsAsync(Status.Updated);
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.UpdateProjectAsync(42, inputProject);

        // Assert
        Assert.IsType<OkResult>(actual.Result);
    }

    [Fact]
    public async Task DeleteProjectAsync_Returns_NotFound_Given_Nonexisting_Project()
    {
        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();

        repository.Setup(m => m.DeleteProjectAsync(42)).ReturnsAsync(Status.NotFound);
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.DeleteAsync(42);

        // Assert
        Assert.IsType<NotFoundObjectResult>(actual.Result);
    }

    [Fact]
    public async Task DeleteProjectAsync_Returns_Ok_Given_Existing_Project()
    {
        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();

        repository.Setup(m => m.DeleteProjectAsync(42)).ReturnsAsync(Status.Deleted);
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.DeleteAsync(42);

        // Assert
        Assert.IsType<OkResult>(actual.Result);
    }
}