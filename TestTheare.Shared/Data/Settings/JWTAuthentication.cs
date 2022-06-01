using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTheare.Shared.Data.Settings
{
    public class JWTAuthentication
    {
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }
        public string JwtKey { get; set; }
        public string JwtHours { get; set; }
    }
}
