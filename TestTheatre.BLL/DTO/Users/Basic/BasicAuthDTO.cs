 
using System.ComponentModel.DataAnnotations; 

namespace TestTheatre.BLL.DTO.Users.Basic
{
    public abstract class BasicAuthDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
