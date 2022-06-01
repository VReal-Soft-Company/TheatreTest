using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTheatre.Shared.Data;

namespace TestTheare.DAL.Filters
{
    public class Filter<TEntity> where TEntity : IDbEntityBase
    {
        public string OrderBy { get; set; }
        public bool? OrderByDescending { get; set; }
    }
} 
