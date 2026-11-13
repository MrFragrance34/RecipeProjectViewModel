    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace ClassLibrary4
    {
        public class User : BaseEntity
        {
            private string displayName;
            private string email;
            private string passwordHash;
            private DateTime joinDate;

            public string DisplayName { get => displayName; set => displayName = value; }
            public string Email { get => email; set => email = value; }
            public string PasswordHash { get => passwordHash; set => passwordHash = value; }
            public DateTime JoinDate { get => joinDate; set => joinDate = value; }
            public override string ToString()
            {
            return $"ID: {Id}, Name: {DisplayName}, Email: {Email}, Join Date: {JoinDate}";
            }
    }
    }
