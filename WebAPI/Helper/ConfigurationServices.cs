using Identity.Persistance.Identity;
using Identity.Persistance;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Identity.Application.DTOs.AppSettings;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Identity.Application.Common.Const;
using Identity.Application.Common.Enums;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace WebAPI.Helper;

public static class ConfigurationServices
{
    public static void AddCustomIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            // Password configs
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequiredUniqueChars = 0;


            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 10;

            // ApplicationUser settings
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@.-_";
        });
    }
    public static void AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(configureOptions =>
        {
            //configureOptions.Events = new JwtBearerEvents
            //{
            //    OnMessageReceived = context =>
            //    {
            //        context.Token = context.Request.Cookies["Token"];
            //        return Task.CompletedTask;
            //    },
            //};
            configureOptions.SaveToken = true;
            configureOptions.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JwtIssuerOptions:Issuer"],
                ValidAudience = configuration["JwtIssuerOptions:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtIssuerOptions:SecretKey"])),
                ClockSkew = TimeSpan.FromSeconds(30),
                RequireExpirationTime = true,
            };
        });
    }
    public static void AddAppSettings(this IServiceCollection services, IConfiguration configuration)
    {
        AppSettings appSettings = configuration.Get<AppSettings>();
        services.TryAddSingleton(appSettings);
    }
    public static void AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            var apiInfo = new OpenApiInfo
            {
                Version = configuration["SwaggerDetails:ApiVersion"],
                Title = configuration["SwaggerDetails:Title"],
                Description = configuration["SwaggerDetails:Description"],
                Contact = new OpenApiContact
                {
                    Name = configuration["SwaggerDetails:Contact:Name"],
                    Email = configuration["SwaggerDetails:Contact:Email"],
                    Url = new Uri(configuration["SwaggerDetails:Contact:Url"]),
                }
            };
            options.SwaggerDoc("v1", apiInfo);

            var securityScheme = new OpenApiSecurityScheme()
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            };

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                      new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "bearerAuth"
                            }
                        },
                        new string[] {}
                }
            };

            options.SwaggerDoc(configuration["SwaggerDetails:ApiVersion"], apiInfo);
            options.IgnoreObsoleteActions();
            options.IgnoreObsoleteProperties();
            options.AddSecurityDefinition("bearerAuth", securityScheme);
            options.AddSecurityRequirement(securityRequirement);
        });
    }

    public static void AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
        });
    }
    public static void AddCustomLocalization(this IServiceCollection services)
    {
        services.AddLocalization();

        services.Configure<RequestLocalizationOptions>(opts =>
        {
            var supportedCultures = new List<CultureInfo>();
            foreach (var culture in Enum.GetValues(typeof(SupportedCulture)))
            {
                supportedCultures.Add(new CultureInfo(culture.ToString()));
            }

            var defaultCulture = AppConstants.DEFAULT_CULTURE.ToString();
            opts.DefaultRequestCulture = new RequestCulture(defaultCulture, defaultCulture);
            opts.SupportedCultures = supportedCultures;
            opts.SupportedUICultures = supportedCultures;
        });
    }
}
