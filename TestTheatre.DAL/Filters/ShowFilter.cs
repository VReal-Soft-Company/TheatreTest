using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTheatre.DAL.Entities;

namespace TestTheare.DAL.Filters
{
    public class ShowFilter:Filter<Show>
    {
        public string Name { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
