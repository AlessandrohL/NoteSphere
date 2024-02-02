using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Errors
{
    public static class NotebookErrorValidations
    {
        public const string TitleNullOrEmpty = "The notebook title cannot be empty or null.";
        public const string TitleTooShort = "The notebook title must be at least 1 character long.";
        public const string DescriptionMaxLength = "The notebook description must not exceed 70 characters.";
        public const string NotInTrash = "The notebook is not in the trash.";
    }
}
