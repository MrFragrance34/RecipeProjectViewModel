using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary4
{
    public class Admin : User
    {
        private string adminRole;

        public string AdminRole { get => adminRole; set => adminRole = value; }
        public override string ToString()
        {
            return $"{base.ToString()}, Admin Role: {AdminRole}";
        }
    }
}
