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
