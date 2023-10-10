using CodePuliseAPI.Data;
using CodePuliseAPI.Models.Domain;
using CodePuliseAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePuliseAPI.Repositories.Implementaion
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext dbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment,IHttpContextAccessor httpContextAccessor,ApplicationDbContext dbContext)

        {
            this.webHostEnvironment= webHostEnvironment;
            this.httpContextAccessor= httpContextAccessor;
            this.dbContext= dbContext;


        }

        public async Task<IEnumerable<BlogImage>> GetAll()
        {
          return  await dbContext.BlogImages.ToListAsync();
        }

        public async Task<BlogImage> upload(IFormFile file, BlogImage blogImage)
        {

            //1 Upload the Image to API/Images
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FlieName}{blogImage.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);
            //2-update the database
            var httpRequest = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FlieName}{blogImage.FileExtension}";
            blogImage.Url = urlPath;

            await dbContext.BlogImages.AddAsync(blogImage);
            await dbContext.SaveChangesAsync();
            return blogImage;
               
         }
    }
}
