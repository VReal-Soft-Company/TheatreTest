using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TestTheatre.Shared.Data;

namespace TestTheatre.DAL.Entities
{
    public class User : BaseEntity
    {
      
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public string Email { get; set; }
        public string Role { get; set; } 
        [NotMapped]
        public string Password { get; set; }
        [JsonIgnore]
        public byte[] PasswordHash { get; set; }
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }
        public ICollection<UserShows> UserShows { get; set; }
    }
} 