﻿using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewMoldes.System
{
    public class UserVm
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
