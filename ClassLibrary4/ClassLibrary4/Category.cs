using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary4
{
    public class Category : BaseEntity
    {
        private string categoryName;

        public string CategoryName { get => categoryName; set => categoryName = value; }
        public override string ToString()
        {
            return $"ID: {Id}, Category Name: {CategoryName}";
        }
    }
}
