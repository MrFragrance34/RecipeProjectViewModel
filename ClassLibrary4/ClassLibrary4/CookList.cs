using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary4
{
    public class CookList : List<Cook>
    {
        public CookList() { }
        public CookList(IEnumerable<Cook> list) : base(list) { }
        public CookList(IEnumerable<BaseEntity> list) : base(list.Cast<Cook>().ToList()) { }
    }
}
