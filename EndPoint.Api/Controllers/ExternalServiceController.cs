using EndPoint.Api.Helper;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EndPoint.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExternalServiceController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExternalServiceController> _logger;
    public ExternalServiceController(IHttpClientFactory httpClientFactory,
        ILogger<ExternalServiceController> logger)
    {
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
    }
    #region old     
    //[HttpGet]
    //public async Task<ActionResult<List<ExternalViewModel>>> GetAllFromExternalService()
    //{
    //    string apiUrl = "https://jsonplaceholder.typicode.com/posts";

    //    HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
    //    var statusCode = (int)response.StatusCode;
    //    var requestMethode = HttpContext.Request.Method;
    //    var requestUrl = apiUrl;
    //    var clientIp = HttpContext.Connection.RemoteIpAddress;

    //    if (response.IsSuccessStatusCode)
    //    {
    //        string content = await response.Content.ReadAsStringAsync();

    //        //var exampleUser = new WebApplication2User { Id = "1001", UserName = "Adam", SecurityStamp = DateTime.Now.ToString() };
    //          _logger.LogInformation($"Get all item. Status Code: {statusCode}, Request Methode: {requestMethode}, Request URL: {requestUrl}, Client IP: {clientIp}");

    //        _logger.LogInformation($"Get all item. Status Code: {statusCode}, Request Methode: {requestMethode}, Request URL: {requestUrl}, Client IP: {clientIp}");
    //        return Ok(content);
    //    }
    //    _logger.LogError($"Failed Get data. Status Code: {statusCode}, Request Methode: {requestMethode}, Request URL: {requestUrl}, Client IP: {clientIp}");
    //    return BadRequest($"Failed to retrieve data from the external service. Status Code : {statusCode}");
    //}

    //[HttpGet("{id}")]
    //public async Task<IActionResult> GetFromExternalService(int id)
    //{
    //    string apiUrl = $"https://jsonplaceholder.typicode.com/todos/{id}";

    //    HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

    //    var statusCode = (int)response.StatusCode;
    //    var requestMethode = HttpContext.Request.Method;
    //    var requestUrl = apiUrl;
    //    var clientIp = HttpContext.Connection.RemoteIpAddress;

    //    if (response.IsSuccessStatusCode)
    //    {
    //        string content = await response.Content.ReadAsStringAsync();
    //        _logger.LogInformation($"Get item by id. Status Code: {statusCode}, Request Methode: {requestMethode}, Request URL: {requestUrl}, Client IP: {clientIp}");

    //        return Ok(content);
    //    }
    //    _logger.LogError($"Failed Get data. Status Code: {statusCode}, Request Methode: {requestMethode}, Request URL: {requestUrl}, Client IP: {clientIp}");
    //    return BadRequest($"Failed to retrieve data from the external service. Status Code : {statusCode}");
    //}

    //[HttpPost]
    //public async Task<IActionResult> PostToExternalService([FromBody] ExternalViewModel data)
    //{
    //    string apiUrl = "https://jsonplaceholder.typicode.com/posts";

    //    HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, data);

    //    var statusCode = (int)response.StatusCode;
    //    var requestMethode = HttpContext.Request.Method;
    //    var requestUrl = apiUrl;
    //    var clientIp = HttpContext.Connection.RemoteIpAddress;

    //    if (response.IsSuccessStatusCode)
    //    {
    //        string content = await response.Content.ReadAsStringAsync();
    //        _logger.LogInformation($"Success post data. Status Code: {statusCode}, Request Methode: {requestMethode}, Request URL: {requestUrl}, Client IP: {clientIp}");

    //        return Ok(content);
    //    }
    //    else
    //    {
    //        //_logger.LogError("Failed to post data {StatusCode}", (int)response.StatusCode);
    //        _logger.LogError($"Failed to post data. Status Code: {statusCode}, Request Methode: {requestMethode}, Request URL: {requestUrl}, Client IP: {clientIp}");
    //        return BadRequest($"Failed to post data to the external service. StatusCode: {(int)response.StatusCode}");
    //    }
    //}
    #endregion

    [HttpGet]
    public async Task<ActionResult<List<EXUserViewModel>>> GetAllUsers()
    {
        string apiUrl = "https://gorest.co.in/public/v2/users";

        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }
        throw new Exception("Failed Get data");
        //_logger.LogError($"Failed Get data. Status Code: {statusCode}, Request Methode: {requestMethode}, Request URL: {requestUrl}, Client IP: {clientIp}");
        //return BadRequest($"Failed to retrieve data from the external service. Status Code : {statusCode}");
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserBy(int id)
    {
        string apiUrl = $"https://gorest.co.in/public/v2/users/{id}";

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
    public async Task<IActionResult> CreateUser([FromBody] EXUserViewModel data)
    {
        string apiUrl = "https://jsonplaceholder.typicode.com/posts";

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
    public async Task<IActionResult> UpdateUser(EXUserViewModel data)
    {
        string apiUrl = $"https://gorest.co.in/public/v2/users/{data.id}";

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
        string apiUrl = $"https://gorest.co.in/public/v2/users/{id}";

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

public class ExternalViewModel
{
    public int userId { get; set; }
    public int id { get; set; }
    public string title { get; set; }
    public string body { get; set; }
    public bool completed { get; set; }
}

public class EXUserViewModel
{
    public int id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string gender { get; set; }
    public bool status { get; set; }
}