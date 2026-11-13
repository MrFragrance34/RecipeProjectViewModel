using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary4
{
    public class RecipeCategory : BaseEntity
    {
        private Recipe recipe;
        private Category category;

        public Recipe Recipe { get => recipe; set => recipe = value; }
        public Category Category { get => category; set => category = value; }
        public override string ToString()
        {
            return $"ID: {Id}, Recipe ID: {Recipe?.Id}, Category ID: {Category?.Id}";
        }
    }

}
