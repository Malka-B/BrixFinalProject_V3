﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Data.Entites
{
    public class CustomerEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Passowrd { get; set; }
    }
}