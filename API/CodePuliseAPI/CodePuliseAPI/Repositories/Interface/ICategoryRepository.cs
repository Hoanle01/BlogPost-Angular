using CodePuliseAPI.Models.Domain;

namespace CodePuliseAPI.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task <Category> CreateAsync (Category category);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetById(Guid id);
        Task<Category?> UpadateAsync(Category category);
        Task<Category?> DeleteAsync(Guid id);

    }
}
