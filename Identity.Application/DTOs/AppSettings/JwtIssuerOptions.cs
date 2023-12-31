﻿namespace Identity.Application.DTOs.AppSettings;

public class JwtIssuerOptions
{
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string SecretKey { get; set; }
}
