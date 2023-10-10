using CodePuliseAPI.Data;
using CodePuliseAPI.Models.Domain;
using CodePuliseAPI.Models.DTO;
using CodePuliseAPI.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePuliseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository,ICategoryRepository categoryRepository)
        {
            this.blogPostRepository=blogPostRepository;
            this.categoryRepository=categoryRepository;
        }
        [HttpPost]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
        {
            //Convert DTO to Domain
            var blogPost = new BlogPost
            {
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                PulishedDate = request.PulishedDate,
                ShortDescription = request.ShortDescription,
                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Categories=new List<Category>() 
            };

            foreach(var categoryGuid in request.Categories) {
                
                var existingCategory=await categoryRepository.GetById(categoryGuid);
                if(existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            
            }
           blogPost= await blogPostRepository.CreateAsync(blogPost);
            //convert Domain Model back to DTO
            var reponse = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PulishedDate = blogPost.PulishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories=blogPost.Categories.Select(x=>new CategoryDto { 
                    Id=x.Id,
                    Name=x.Name,
                    UrlHandle=x.UrlHandle

                }).ToList(),
            };
            return Ok(reponse);
        }

        [HttpGet]
        
        public async Task<IActionResult> GetAlllBlogPosts()
        {
            var blogPosts=await blogPostRepository.GetAllAsync();
            //convert Domain model to DTO
            var response=new List<BlogPostDto>();
            foreach(var blogPost in blogPosts)
            {
                response.Add(new BlogPostDto {
                    Id=blogPost.Id,
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    FeaturedImageUrl=blogPost.FeaturedImageUrl,
                    IsVisible = blogPost.IsVisible,
                    PulishedDate=blogPost.PulishedDate,
                    ShortDescription=blogPost.ShortDescription,
                    Title = blogPost.Title,
                    UrlHandle = blogPost.UrlHandle,
                    
                    Categories = blogPost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle

                    }).ToList(),
                });
            }
            return Ok(response);
        }



        [HttpGet]
        [Route("{urlHandle}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] string urlHandle)
        {
            //Get blogpost details from repository
          var blogPost=  await blogPostRepository.GetByUrlHandAsync(urlHandle);
            if (blogPost is null)
            {
                return NotFound();
            }
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PulishedDate = blogPost.PulishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle

                }).ToList(),

            };
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetBlogPostsById([FromRoute]Guid id) {
        //get the blogpost from Repo
        var blogPost=await blogPostRepository.GetByIdAsync(id);
            if(blogPost is null)
            {
                return NotFound();
            }
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PulishedDate = blogPost.PulishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle

                }).ToList(),

            };
            return Ok(response);
            }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id,UpdateBlogPostRequestDto request)
        {
            //Convert DTO to Domain Model
            var blogPost= new BlogPost
            {
                Id=id,
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                PulishedDate = request.PulishedDate,
                ShortDescription = request.ShortDescription,
                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };
            //Foreach
            foreach(var categoryGuid in request.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);
                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }
            //call Repository to Update BogPost Model
           var updatedBlogPost= await blogPostRepository.UpdateAsync(blogPost);
            if (updatedBlogPost == null)
            {
                return NotFound();
            }
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PulishedDate = blogPost.PulishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle

                }).ToList(),
            };
            return Ok(response);
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
           var deleteBlogPost= await blogPostRepository.DeleteAsync(id);
            if(deleteBlogPost == null) { return NotFound();
            };
            var response=new BlogPostDto { 
                Id=deleteBlogPost.Id,
                Author = deleteBlogPost.Author,
                Content = deleteBlogPost.Content,
                FeaturedImageUrl=deleteBlogPost.FeaturedImageUrl,
                IsVisible = deleteBlogPost.IsVisible,
                PulishedDate=deleteBlogPost.PulishedDate,
                ShortDescription=deleteBlogPost.ShortDescription,
                Title = deleteBlogPost.Title,
                UrlHandle = deleteBlogPost.UrlHandle,
            
            
            };
            return Ok(response);
        }

    }
}
