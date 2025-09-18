﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UserProfile.Response
{
    public class UserProfileResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
