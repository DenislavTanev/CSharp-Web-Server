using System;
using System.Collections.Generic;
using System.Text;

namespace SharedTrip.Models.Users
{
    public class UserRegisterInputModel
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
