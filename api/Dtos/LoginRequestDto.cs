using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class LoginRequest
        {
            public string UsernameOrEmail { get; set; }
            public string Password { get; set; }
        }
}