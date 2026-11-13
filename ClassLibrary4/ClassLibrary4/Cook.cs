using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary4
{
    public class Cook : BaseEntity
    {
        private string bio;
        private string profilePicPath;

        public string Bio { get => bio; set => bio = value; }
        public string ProfilePicPath { get => profilePicPath; set => profilePicPath = value; }
        public override string ToString()
        {
            return $"ID: {Id}, Bio: {Bio}, Profile Picture: {ProfilePicPath}";
        }
    }
}
