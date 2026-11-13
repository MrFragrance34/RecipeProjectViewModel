using System;
using ClassLibrary4;
using System.Data.OleDb;

namespace ViewModel
{
    public class RecipesDB : BaseDB
    {
        public RecipeList SelectAll()
        {
            command.Parameters.Clear();
            command.CommandText = "SELECT * FROM Recipes";
            RecipeList list = new RecipeList(base.Select());
            return list;
        }

        public static Recipe SelectById(int id)
        {
            RecipesDB db = new RecipesDB();
            db.command.CommandText = "SELECT * FROM Recipes WHERE ID = @id";
            db.command.Parameters.Add(new OleDbParameter("@id", id));

            List<BaseEntity> list = db.Select();
            return list.Count > 0 ? (Recipe)list[0] : null;
        }

        public override BaseEntity NewEntity()
        {
            return new Recipe();
        }

        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Recipe recipe = entity as Recipe;
            if (recipe != null)
            {
                recipe.Title = reader["Title"].ToString();
                recipe.Description = reader["Description"].ToString();
                recipe.Instructions = reader["Instructions"].ToString();

                if (reader["PrepTimeMins"] != DBNull.Value)
                    recipe.PrepTimeMins = Convert.ToInt32(reader["PrepTimeMins"]);

                if (reader["ImagePath"] != DBNull.Value)
                    recipe.ImagePath = reader["ImagePath"].ToString();

                if (reader["RecipeDate"] != DBNull.Value)
                    recipe.RecipeDate = Convert.ToDateTime(reader["RecipeDate"]);

                if (reader["CookID"] != DBNull.Value)
                {
                    int cookId = Convert.ToInt32(reader["CookID"]);
                    recipe.Cook = CookDB.SelectById(cookId);
                }
            }
            base.CreateModel(entity);
            return entity;
        }

        protected override void CreateInsertedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Recipe recipe = entity as Recipe;

            if (recipe != null)
            {
                string sqlStr = "INSERT INTO Recipes (Title, Description, Instructions, PrepTimeMins, ImagePath, RecipeDate, CookID) VALUES (@title, @desc, @inst, @prep, @img, @rDate, @cookId)";

                cmd.CommandText = sqlStr;

                cmd.Parameters.Add("@title", OleDbType.VarWChar).Value =
                    (object)recipe.Title ?? DBNull.Value;

                cmd.Parameters.Add("@desc", OleDbType.LongVarWChar).Value =
                    (object)recipe.Description ?? DBNull.Value;

                cmd.Parameters.Add("@inst", OleDbType.LongVarWChar).Value =
                    (object)recipe.Instructions ?? DBNull.Value;

                cmd.Parameters.Add("@prep", OleDbType.Integer).Value =
                    recipe.PrepTimeMins;

                cmd.Parameters.Add("@img", OleDbType.VarWChar).Value =
                    (object)recipe.ImagePath ?? DBNull.Value;

                cmd.Parameters.Add("@rDate", OleDbType.Date).Value =
                    recipe.RecipeDate;

                cmd.Parameters.Add("@cookId", OleDbType.Integer).Value =
                    recipe.Cook.Id;
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Recipe recipe = entity as Recipe;
            if (recipe != null)
            {
                string sqlStr = "UPDATE Recipes SET Title = @title, Description = @desc, Instructions = @inst, PrepTimeMins = @prep, ImagePath = @img, RecipeDate = @rDate, CookID = @cookId WHERE ID = @id";

                cmd.CommandText = sqlStr;

                cmd.Parameters.Add("@title", OleDbType.VarWChar).Value =
                    (object)recipe.Title ?? DBNull.Value;

                cmd.Parameters.Add("@desc", OleDbType.LongVarWChar).Value =
                    (object)recipe.Description ?? DBNull.Value;

                cmd.Parameters.Add("@inst", OleDbType.LongVarWChar).Value =
                    (object)recipe.Instructions ?? DBNull.Value;

                cmd.Parameters.Add("@prep", OleDbType.Integer).Value =
                    recipe.PrepTimeMins;

                cmd.Parameters.Add("@img", OleDbType.VarWChar).Value =
                    (object)recipe.ImagePath ?? DBNull.Value;

                cmd.Parameters.Add("@rDate", OleDbType.Date).Value =
                    recipe.RecipeDate;

                cmd.Parameters.Add("@cookId", OleDbType.Integer).Value =
                    recipe.Cook.Id;

                cmd.Parameters.Add("@id", OleDbType.Integer).Value =
                    recipe.Id;
            }
        }

        protected override void CreateDeletedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Recipe recipe = entity as Recipe;
            if (recipe != null)
            {
                string sqlStr = "DELETE FROM Recipes WHERE ID = @id";
                cmd.CommandText = sqlStr;
                cmd.Parameters.Add(new OleDbParameter("@id", recipe.Id));
            }
        }
    }
}