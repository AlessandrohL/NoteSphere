using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class ApplicationAssemblyReference
    {
        public static Assembly Assembly => Assembly.GetAssembly(typeof(ApplicationAssemblyReference))!;
    }
}
