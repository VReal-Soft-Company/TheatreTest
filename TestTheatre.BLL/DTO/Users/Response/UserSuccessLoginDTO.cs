using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTheatre.BLL.DTO.Users.Response
{
    public class UserSuccessLoginDTO
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
