using PortfolioBlazor.DTOs;

namespace PortfolioBlazor.Services
{   
    public class BlogService
    {
        private readonly HttpClient _httpClient;

        public BlogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BlogDTO>> GetBlogsAsync()
        {
            var blogs = await _httpClient.GetFromJsonAsync<List<BlogDTO>>("https://localhost:44356/blogs");
            return blogs ?? new List<BlogDTO>();
        }
    }

}
