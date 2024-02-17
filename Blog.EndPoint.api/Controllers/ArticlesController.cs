using Blog.Application.Interfaces;
using Blog.Domain.Entities.ArticleAgg;
using Microsoft.AspNetCore.Mvc;

namespace Blog.EndPoint.api.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticleController : ControllerBase
{
    private readonly IMongoRepository<Article> _peopleRepository;

    public ArticleController(IMongoRepository<Article> peopleRepository)
    {
        _peopleRepository = peopleRepository;
    }

    [HttpPost("createArticle")]
    public async Task AddArticle(string title, string shortDescription)
    {
        var person = new Article()
        {
            Title="Economic",
            ShortDescription="nemidanam"
        };

        await _peopleRepository.InsertOneAsync(person);
    }

    [HttpGet("getArticleData")]
    public IEnumerable<string> GetPeopleData()
    {
        var people = _peopleRepository.FilterBy(
            filter => filter.Title != "test",
            projection => projection.Title
        );
        return people;
    }
}
