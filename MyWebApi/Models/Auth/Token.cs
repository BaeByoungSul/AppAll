﻿namespace Models.Auth;

public class Tokens
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
