using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary4
{
    public class RecipeList : List<Recipe>
    {
        public RecipeList() { }
        public RecipeList(IEnumerable<Recipe> list) : base(list) { }
        public RecipeList(IEnumerable<BaseEntity> list) : base(list.Cast<Recipe>().ToList()) { }
    }
}
