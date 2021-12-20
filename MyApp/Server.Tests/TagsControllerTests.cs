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

public class TagsControllerTests
{
    [Fact]
    public async Task Get_returns_Ok()
    {
        // Arrange
        var logger = new Mock<ILogger<TagsController>>();
        var repository = new Mock<ITagRepository>();
        repository.Setup(m => m.GetAllTagsAsync()).ReturnsAsync(Array.Empty<TagDTO>());
        var controller = new TagsController(logger.Object, repository.Object);
        // Act
        var actual = await controller.GetAll();

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }
}
