public class UsersControllerTests
{
    [Fact]
    public async Task GetAsync_returns_Ok()
    {
        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.GetAllUsersAsync()).ReturnsAsync(Array.Empty<UserDTO>());
        var controller = new UsersController(logger.Object, repository.Object);


        // Act
        var actual = await controller.GetAllAsync();

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromEmailAsync_Returns_NotFound_Given_Nonexisting_Email()
    {

        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.GetUserFromEmailAsync("anton@berg.hotmail.com")).ReturnsAsync(default(UserDTO));
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromEmailAsync("anton@berg.hotmail.com");

        // Assert
        Assert.IsType<NotFoundObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromEmailAsync_Returns_BadRequest_Given_Invalid_Email()
    {

        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.GetUserFromEmailAsync("kl")).ReturnsAsync(default(UserDTO));
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromEmailAsync("kl");

        // Assert
        Assert.IsType<NotFoundObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromEmailAsync_Returns_Ok_Given_existing_Email()
    {

        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();

        var user = new StudentDTO
        (
            "Anton@hotmail.com",
            "Anton",
            "SWU",
            1
        );
        repository.Setup(m => m.GetUserFromEmailAsync("Anton@hotmail.com")).ReturnsAsync(user);
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromEmailAsync("Anton@hotmail.com");

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetStudentsFromProjectIdAsync_Returns_BadRequest_Given_Negative_Id()
    {

        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.GetStudentsFromProjectIDAsync(-1)).ReturnsAsync(Array.Empty<StudentDTO>);
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetStudentsFromProjectIdAsync(-1);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetStudentsFromProjectIdAsync_Returns_NotFound_Given_Existing_Id()
    {

        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.GetStudentsFromProjectIDAsync(1)).ReturnsAsync(Array.Empty<StudentDTO>);
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetStudentsFromProjectIdAsync(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetStudentsFromProjectIdAsync_Returns_Ok_Given_Existing_Id()
    {

        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();

        var user = new StudentDTO
        (
            "Anton@hotmail.com",
            "Anton",
            "SWU",
            1
        );

        repository.Setup(m => m.GetStudentsFromProjectIDAsync(1)).ReturnsAsync(new List<StudentDTO> { user });
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetStudentsFromProjectIdAsync(1);

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetSupervisorsFromProjectIdAsync_Returns_BadRequest_Given_Negative_Id()
    {

        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.GetSupervisorsFromProjectIDAsync(-1)).ReturnsAsync(Array.Empty<SupervisorDTO>);
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetSupervisorsFromProjectIdAsync(-1);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetSupervisorsFromProjectIdAsync_Returns_NotFound_Given_Existing_Id()
    {

        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.GetSupervisorsFromProjectIDAsync(1)).ReturnsAsync(Array.Empty<SupervisorDTO>);
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetSupervisorsFromProjectIdAsync(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetSupervisorsFromProjectIdAsync_Returns_Ok_Given_Existing_Id()
    {

        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();

        var user = new SupervisorDTO
        (
            "Anton@hotmail.com",
            "Anton",
            new List<int> { 1, 5, 9}
        );

        repository.Setup(m => m.GetSupervisorsFromProjectIDAsync(1)).ReturnsAsync(new List<SupervisorDTO> { user });
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetSupervisorsFromProjectIdAsync(1);

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }
}
