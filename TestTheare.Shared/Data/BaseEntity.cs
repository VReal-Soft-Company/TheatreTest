 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTheatre.Shared.Data
{
    public abstract class BaseEntity : IDbEntityBase<long>
    {
        [Key]
        public long Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
