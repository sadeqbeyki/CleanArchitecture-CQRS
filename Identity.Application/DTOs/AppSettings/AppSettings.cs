
namespace Identity.Application.DTOs.AppSettings;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
    public Logging Logging { get; set; }
    public JwtIssuerOptions JwtIssuerOptions { get; set; }
    public SwaggerDetails SwaggerDetails { get; set; }
    public SendgridOptions SendGridOptions { get; set; }
    public WebsiteDetails WebsiteDetails { get; set; }
}


