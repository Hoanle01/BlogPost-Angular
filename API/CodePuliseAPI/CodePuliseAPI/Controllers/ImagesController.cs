using CodePuliseAPI.Models.Domain;
using CodePuliseAPI.Models.DTO;
using CodePuliseAPI.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePuliseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        [HttpGet]
        public  async Task<IActionResult> GetAllImages()
        {
            //call image repository to get all images
            var images = await imageRepository.GetAll();
            //covert domain model to DTO
            var response=new List<BlogImageDto>();
            foreach (var item in images)
            {
                response.Add(new BlogImageDto
                {
                    Id = item.Id,
                    FileExtension = item.FileExtension,
                    Title = item.Title,
                    FileName=item.FlieName,
                    DateCreated = item.DateCreated,
                    Url = item.Url,


                });
                
            }
            return Ok(response);

        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName,
            [FromForm] string title)
        {
           
            
             ValidateFileUpload(file);
        if(ModelState.IsValid)
            {
                //file upload
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FlieName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now,
                };
               blogImage= await imageRepository.upload(file, blogImage);

                //convert domain model to DTO

                var response = new BlogImageDto
                {
                    Id = blogImage.Id,
                    FileName = blogImage.FlieName,
                    Title = blogImage.Title,
                    FileExtension = blogImage.FileExtension,
                    Url = blogImage.Url
                };
      
                return Ok(response);
            }
        return BadRequest(ModelState);
            
            
           
           
        }
        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            };
            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "file size cannot be more than 10MB");
            };
        }
    }
}
