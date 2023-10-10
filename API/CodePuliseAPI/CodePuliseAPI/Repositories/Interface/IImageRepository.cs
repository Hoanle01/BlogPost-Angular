using CodePuliseAPI.Models.Domain;

namespace CodePuliseAPI.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> upload(IFormFile file, BlogImage blogImage);

        Task<IEnumerable<BlogImage>> GetAll();
            
     }
}
