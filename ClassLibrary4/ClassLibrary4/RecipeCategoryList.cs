using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary4
{
    public class RecipeCategoryList : List<RecipeCategory>
    {
        public RecipeCategoryList() { }

        public RecipeCategoryList(IEnumerable<RecipeCategory> list) : base(list) { }

        public RecipeCategoryList(IEnumerable<BaseEntity> list)
            : base(list.Cast<RecipeCategory>().ToList()) { }
    }
}