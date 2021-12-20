namespace MyApp.Shared;

public interface ITagRepository
{
    Task<IReadOnlyCollection<TagDTO>> GetAllTagsAsync();
}