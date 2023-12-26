using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IBaseEntity<TKey>
    {
        TKey Id { get; set; }
        DateTime CreatedTime { get; set; }
        DateTime? ModifiedTime { get; set; }
    }
}
