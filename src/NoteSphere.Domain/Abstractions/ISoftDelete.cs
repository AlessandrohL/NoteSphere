using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
        DateTime? DeleteAt { get; set; }
        void Delete();
        void Restore();
    }
}
