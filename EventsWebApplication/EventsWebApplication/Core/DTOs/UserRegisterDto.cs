﻿namespace EventsWebApplication.Core.DTOs
{
    public class UserRegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "User";

    }
}
