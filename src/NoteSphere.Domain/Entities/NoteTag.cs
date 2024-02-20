using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class NoteTag
    {
        public Guid NoteId { get; set; }
        public Note? Note { get; set; }

        public int TagId { get; set; }
        public Tag? Tag { get; set; }
    
        
    }
}
