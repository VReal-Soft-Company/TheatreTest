using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTheare.Shared.Data.Pagination
{
    public class PagedResponse<T>
    {
        public PageInfo PageInfo { get; set; }

        public ICollection<T> Records { get; set; }
    }
}
