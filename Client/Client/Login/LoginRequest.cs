﻿namespace Client.Login
{
    class LoginRequest
    {
        public string Password;

        public LoginRequest (string password)
        {
            Password = password;
        }
    }
}