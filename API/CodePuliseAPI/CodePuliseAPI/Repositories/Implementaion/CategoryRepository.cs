using CodePuliseAPI.Data;
using CodePuliseAPI.Models.Domain;
using CodePuliseAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePuliseAPI.Repositories.Implementaion
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext) {
            this.dbContext=dbContext;
        
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;    
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
         var existingCategory= await dbContext.Categories.FirstOrDefaultAsync(x=> x.Id == id);
            if (existingCategory == null) {

                return null;
            }
            dbContext.Categories.Remove(existingCategory);
            await dbContext.SaveChangesAsync();
            return existingCategory;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dbContext.Categories.ToArrayAsync();
        }

        public async Task<Category?> GetById(Guid id)
        {
            return  await dbContext.Categories.FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<Category?> UpadateAsync(Category category)
        {
         var existingCategory= await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
          if(existingCategory!=null)
            {
                dbContext.Entry(existingCategory).CurrentValues.SetValues(category);
                await dbContext.SaveChangesAsync();
                return category;
            }
          return null;
        }
    }
}
