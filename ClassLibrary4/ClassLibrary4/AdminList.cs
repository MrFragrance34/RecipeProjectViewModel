using System;
using System.Collections.Generic;

namespace ClassLibrary4
{
    public class AdminList : List<Admin>
    {
        public AdminList() { }

        public AdminList(IEnumerable<Admin> list) : base(list) { }

        public AdminList(IEnumerable<BaseEntity> list) : base(list.Cast<Admin>().ToList()) { }
    }
}