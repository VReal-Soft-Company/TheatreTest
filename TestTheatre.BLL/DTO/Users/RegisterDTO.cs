using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTheatre.BLL.DTO.Users.Basic;

namespace TestTheatre.BLL.DTO
{
    public class RegisterDTO:BasicAuthDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }  
    }
}
