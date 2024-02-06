using Microsoft.AspNetCore.Mvc;

namespace EndPoint.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExternalServiceController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExternalServiceController> _logger;
    private readonly IConfiguration _configuration;
    public ExternalServiceController(IHttpClientFactory httpClientFactory,
        ILogger<ExternalServiceController> logger,
        IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<List<ExternalUserViewModel>>> GetAllUsers()
    {
        string apiUrl = _configuration["ExternalService:Api:UsersUrl"];
        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }
        throw new Exception("Failed Get data");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserBy(int id)
    {
        string apiUrl = _configuration[$"ExternalService:Api:UsersUrl{id}"];

        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

        var statusCode = (int)response.StatusCode;
        var requestMethode = HttpContext.Request.Method;
        var requestUrl = apiUrl;
        var clientIp = HttpContext.Connection.RemoteIpAddress;

        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Get item by id. Status Code: {statusCode}, Request Methode: {requestMethode}, Request URL: {requestUrl}, Client IP: {clientIp}");

            return Ok(content);
        }
        _logger.LogError($"Failed Get data. Status Code: {statusCode}, Request Methode: {requestMethode}, Request URL: {requestUrl}, Client IP: {clientIp}");
        return BadRequest($"Failed to retrieve data from the external service. Status Code : {statusCode}");
    }
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] ExternalUserViewModel data)
    {
        string apiUrl = _configuration["ExternalService:Api:UsersUrl"];

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, data);

        var statusCode = (int)response.StatusCode;
        var requestMethode = HttpContext.Request.Method;
        var requestUrl = apiUrl;
        var clientIp = HttpContext.Connection.RemoteIpAddress;

        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Item Created. Status Code: {statusCode}, Request Methode: {requestMethode}, Request URL: {requestUrl}, Client IP: {clientIp}");

            return Ok(content);
        }
        else
        {
            //_logger.LogError("Failed to post data {StatusCode}", (int)response.StatusCode);
            _logger.LogError($"Failed to post data. Status Code: {statusCode}, Request Methode: {requestMethode}, Request URL: {requestUrl}, Client IP: {clientIp}");
            return BadRequest($"Failed to post data to the external service. StatusCode: {(int)response.StatusCode}");
        }
    }
    [HttpPut]
    public async Task<IActionResult> UpdateUser(ExternalUserViewModel data)
    {
        string apiUrl = _configuration[$"ExternalService:Api:UsersUrl{data.id}"];

        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(apiUrl, data);

        var requestMethode = HttpContext.Request.Method;
        var statusCode = (int)response.StatusCode;
        var requestUrl = apiUrl;
        var clientIp = HttpContext.Connection.RemoteIpAddress;

        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Item Updated. Status Code: {statusCode}, Request Methode: {requestMethode}, Request URL: {requestUrl}, Client IP: {clientIp}");

            return Ok(content);
        }
        _logger.LogError($"Failed Get data. Status Code: {statusCode}, Request Methode: {requestMethode}, Request URL: {requestUrl}, Client IP: {clientIp}, Reason Pharse: {response.ReasonPhrase}");
        return BadRequest($"Failed to retrieve data from the external service. Status Code : {statusCode}");
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        string apiUrl = _configuration[$"ExternalService:Api:UsersUrl{id}"];

        HttpResponseMessage response = await _httpClient.DeleteAsync(apiUrl);

        var statusCode = (int)response.StatusCode;
        var requestMethode = HttpContext.Request.Method;
        var requestUrl = apiUrl;
        var clientIp = HttpContext.Connection.RemoteIpAddress;

        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Item deleted. Status Code: {statusCode}, Request Methode: {requestMethode}, Request URL: {requestUrl}, Client IP: {clientIp}");

            return NoContent();
        }
        _logger.LogError($"Failed Get data. Status Code: {statusCode}, Request Methode: {requestMethode}, Request URL: {requestUrl}, Client IP: {clientIp}");
        return BadRequest($"Failed to retrieve data from the external service. Status Code : {statusCode}");
    }
}

public class ExternalUserViewModel
{
    public int id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string gender { get; set; }
    public bool status { get; set; }
}