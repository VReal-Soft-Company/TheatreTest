using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace TestTheatre.DAL.Entities
{
    public class UserShows
    {
        public long UserId { get; set; }
        public User User { get; set; }
        [ForeignKey(nameof(UserId))]
        public long ShowId { get; set; }
        [ForeignKey(nameof(ShowId))]
        public Show Show{ get; set; }
        public long CountOfTickets { get; set; }
        public DateTime DateTime { get; set; }
    }
}
