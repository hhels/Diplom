﻿using Diplom.Common.Models;

namespace Diplom.Common.Bodies
{
    public class RegisterBody
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Year { get; set; }
        public string PhoneNumber { get; set; }
        public RussType Russ { get; set; }
    }
}