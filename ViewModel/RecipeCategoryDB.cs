
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using ClassLibrary4;

namespace ViewModel
{
    public class RecipeCategoryDB : BaseDB
    {
        public RecipeCategoryList SelectAll()
        {
            command.Parameters.Clear();
            command.CommandText = "SELECT * FROM Recipe_Categories";

            RecipeCategoryList list = new RecipeCategoryList(base.Select());
            return list;
        }

        public override BaseEntity NewEntity()
        {
            return new RecipeCategory();
        }

        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            RecipeCategory recipeCategory = entity as RecipeCategory;

            if (recipeCategory != null)
            {
                if (reader["RecipeID"] != DBNull.Value)
                {
                    int recipeId = Convert.ToInt32(reader["RecipeID"]);
                    recipeCategory.Recipe = RecipesDB.SelectById(recipeId);
                }

                if (reader["CategoryID"] != DBNull.Value)
                {
                    int categoryId = Convert.ToInt32(reader["CategoryID"]);

                    CategoryDB categoryDB = new CategoryDB();
                    recipeCategory.Category = categoryDB.SelectById(categoryId);
                }
            }

            base.CreateModel(entity);

            return entity;
        }

        protected override void CreateInsertedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            RecipeCategory recipeCategory = entity as RecipeCategory;

            if (recipeCategory != null)
            {
                string sqlStr = "INSERT INTO Recipe_Categories (RecipeID, CategoryID) VALUES (@recipeId, @categoryId)";

                cmd.CommandText = sqlStr;

                cmd.Parameters.Add(new OleDbParameter(
                    "@recipeId",
                    recipeCategory.Recipe != null
                        ? (object)recipeCategory.Recipe.Id
                        : DBNull.Value));

                cmd.Parameters.Add(new OleDbParameter(
                    "@categoryId",
                    recipeCategory.Category != null
                        ? (object)recipeCategory.Category.Id
                        : DBNull.Value));
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            RecipeCategory recipeCategory = entity as RecipeCategory;

            if (recipeCategory != null)
            {
                string sqlStr = "UPDATE Recipe_Categories SET RecipeID = @recipeId, CategoryID = @categoryId WHERE ID = @id";

                cmd.CommandText = sqlStr;

                cmd.Parameters.Add(new OleDbParameter(
                    "@recipeId",
                    recipeCategory.Recipe != null
                        ? (object)recipeCategory.Recipe.Id
                        : DBNull.Value));

                cmd.Parameters.Add(new OleDbParameter(
                    "@categoryId",
                    recipeCategory.Category != null
                        ? (object)recipeCategory.Category.Id
                        : DBNull.Value));

                cmd.Parameters.Add(new OleDbParameter("@id", recipeCategory.Id));
            }
        }

        protected override void CreateDeletedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            RecipeCategory recipeCategory = entity as RecipeCategory;

            if (recipeCategory != null)
            {
                string sqlStr = "DELETE FROM Recipe_Categories WHERE ID = @id";

                cmd.CommandText = sqlStr;

                cmd.Parameters.Add(new OleDbParameter("@id", recipeCategory.Id));
            }
        }
    }
}
