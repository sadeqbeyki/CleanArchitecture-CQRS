using Blog.Application.Interfaces;
using Blog.Domain.Entities.ArticleAgg;
using Microsoft.AspNetCore.Mvc;

namespace Blog.EndPoint.api.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticleController : ControllerBase
{
    private readonly IMongoRepository<Article> _articleRepository;

    public ArticleController(IMongoRepository<Article> peopleRepository)
    {
        _articleRepository = peopleRepository;
    }

    [HttpPost("createArticle")]
    public async Task AddArticle(string title, string shortDescription)
    {
        Article article = new()
        {
            Title = "Economic",
            ShortDescription = "nemidanam"
        };

        await _articleRepository.InsertOneAsync(article);
    }

    [HttpGet("getArticleData")]
    public IEnumerable<string> GetArticleData()
    {
        var article = _articleRepository.FilterBy(
            filter => filter.Title != "test",
            projection => projection.Title
        );
        return article;
    }
}
