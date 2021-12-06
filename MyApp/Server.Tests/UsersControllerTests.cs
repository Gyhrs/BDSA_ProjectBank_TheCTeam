using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using MyApp.Server.Controllers;
using MyApp.Shared;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

public class UsersControllerTests
{
    [Fact]
    public async Task Get_returns_Ok()
    {
        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.GetAllUsersAsync()).ReturnsAsync(Array.Empty<UserDTO>());
        var controller = new UsersController(logger.Object, repository.Object);


        // Act
        var actual = await controller.GetAll();

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromEmail_Returns_NotFound_Given_Nonexisting_Email()
    {

        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.GetUserFromEmailAsync("anton@berg.hotmail.com")).ReturnsAsync(default(UserDTO));
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromEmail("anton@berg.hotmail.com");

        // Assert
        Assert.IsType<NotFoundObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromEmail_Returns_BadRequest_Given_Invalid_Email()
    {

        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.GetUserFromEmailAsync("kl")).ReturnsAsync(default(UserDTO));
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromEmail("kl");

        // Assert
        Assert.IsType<NotFoundObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromEmail_Returns_Ok_Given_existing_Email()
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
        var actual = await controller.GetFromEmail("Anton@hotmail.com");

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetStudentsFromProjectId_Returns_BadRequest_Given_Negative_Id()
    {

        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.GetStudentsFromProjectIDAsync(-1)).ReturnsAsync(Array.Empty<StudentDTO>);
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetStudentsFromProjectId(-1);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetStudentsFromProjectId_Returns_NotFound_Given_Existing_Id()
    {

        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.GetStudentsFromProjectIDAsync(1)).ReturnsAsync(Array.Empty<StudentDTO>);
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetStudentsFromProjectId(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetStudentsFromProjectId_Returns_Ok_Given_Existing_Id()
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
        var actual = await controller.GetStudentsFromProjectId(1);

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetSupervisorsFromProjectId_Returns_BadRequest_Given_Negative_Id()
    {

        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.GetSupervisorsFromProjectIDAsync(-1)).ReturnsAsync(Array.Empty<SupervisorDTO>);
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetSupervisorsFromProjectId(-1);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetSupervisorsFromProjectId_Returns_NotFound_Given_Existing_Id()
    {

        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.GetSupervisorsFromProjectIDAsync(1)).ReturnsAsync(Array.Empty<SupervisorDTO>);
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetSupervisorsFromProjectId(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetSupervisorsFromProjectId_Returns_Ok_Given_Existing_Id()
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
        var actual = await controller.GetSupervisorsFromProjectId(1);

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }
}
