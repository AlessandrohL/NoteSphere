using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeleteAt { get; set; }


        public void Delete()
        {
            IsDeleted = true;
            DeleteAt = DateTime.UtcNow;
            SetModified();
        }
        public void Restore()
        {
            IsDeleted = default;
            DeleteAt = null;
            SetModified();
        }
        public void SetModified() => ModifiedAt = DateTime.UtcNow;
    }
}
