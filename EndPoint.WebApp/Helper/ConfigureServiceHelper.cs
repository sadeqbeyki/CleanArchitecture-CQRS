﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Identity.Persistance.Identity;
using Identity.Persistance;
using Identity.Application.DTOs.AppSettings;
using Identity.Application.Common.Enums;
using Identity.Application.Common.Const;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EndPoint.WebApp.Helper
{
    public static class ConfigureServiceHelper
    {
        /// <summary>
        /// Enable CORS
        /// </summary>
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

        /// <summary>
        /// Adding Auth scheme & JWT configuration
        /// </summary>
        public static void AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            SymmetricSecurityKey signingKey = new(Encoding.ASCII.GetBytes(configuration["JwtIssuerOptions:SecretKey"]));

            services.AddAuthentication(options =>
            {
                //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                // Configure cookie options as needed
                options.Cookie.Name = "YourCookieName"; // Set a unique name for your cookie
                options.ExpireTimeSpan = TimeSpan.FromHours(1); // Set the expiration time
                options.SlidingExpiration = true; // Enable sliding expiration
            })
            .AddJwtBearer(configureOptions =>
            {
                configureOptions.RequireHttpsMetadata = false;
                configureOptions.SaveToken = true;
                configureOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtIssuerOptions:Issuer"],
                    ValidAudience = configuration["JwtIssuerOptions:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = signingKey,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        /// <summary>
        /// Adding Support for Identity
        /// </summary>
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

        /// <summary>
        /// Generate AppSettings
        /// </summary>
        public static void AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            AppSettings appSettings = configuration.Get<AppSettings>();
            services.TryAddSingleton(appSettings);
        }

        /// <summary>
        /// Add Localization & configure it
        /// </summary>
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

        public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
        {
            foreach (var error in result.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}
