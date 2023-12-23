using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateBlogPostAsync(BlogPost blogPost);
        Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync();
    }
}
