using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runed.UI
{
    public static class TypeExt
    {
        public static bool Compare(this Type a, Type b)
        {
            return a.ToString() == b.ToString();
        }
    }S
}
