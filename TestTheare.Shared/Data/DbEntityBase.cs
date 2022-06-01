using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTheatre.Shared.Data
{
    public interface IDbEntityBase
    {
    }

    public interface IDbEntityBase<TKey> : IDbEntityBase
    { 
        TKey Id { get; }
    }
}
