using System;
using ClassLibrary4;
using System.Data.OleDb;

namespace ViewModel
{
    public class ReviewsDB : BaseDB
    {
        public ReviewList SelectAll()
        {
            command.Parameters.Clear();
            command.CommandText = "SELECT * FROM Reviews";
            ReviewList list = new ReviewList(base.Select());
            return list;
        }

        public static Review SelectById(int id)
        {
            ReviewsDB db = new ReviewsDB();
            db.command.CommandText = "SELECT * FROM Reviews WHERE ID = @id";
            db.command.Parameters.Add(new OleDbParameter("@id", id));

            List<BaseEntity> list = db.Select();
            return list.Count > 0 ? (Review)list[0] : null;
        }

        public override BaseEntity NewEntity()
        {
            return new Review();
        }

        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Review review = entity as Review;
            if (review != null)
            {
                if (reader["Rating"] != DBNull.Value)
                    review.Rating = Convert.ToInt32(reader["Rating"]);

                if (reader["CommentText"] != DBNull.Value)
                    review.CommentText = reader["CommentText"].ToString();

                if (reader["ReviewDate"] != DBNull.Value)
                    review.ReviewDate = Convert.ToDateTime(reader["ReviewDate"]);

                if (reader["CookID"] != DBNull.Value)
                {
                    int cookId = Convert.ToInt32(reader["CookID"]);
                    review.Cook = CookDB.SelectById(cookId);
                }

                if (reader["RecipeID"] != DBNull.Value)
                {
                    int recipeId = Convert.ToInt32(reader["RecipeID"]);
                    review.Recipe = RecipesDB.SelectById(recipeId);
                }
            }
            base.CreateModel(entity);
            return entity;
        }

        protected override void CreateInsertedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Review review = entity as Review;

            if (review != null)
            {
                string sqlStr = "INSERT INTO Reviews (Rating, CommentText, ReviewDate, CookID, RecipeID) VALUES (@rating, @comment, @rDate, @cookId, @recipeId)";

                cmd.CommandText = sqlStr;

                cmd.Parameters.Add("@rating", OleDbType.Integer).Value =
                    review.Rating;

                cmd.Parameters.Add("@comment", OleDbType.LongVarWChar).Value =
                    (object)review.CommentText ?? DBNull.Value;

                cmd.Parameters.Add("@rDate", OleDbType.Date).Value =
                    review.ReviewDate;

                cmd.Parameters.Add("@cookId", OleDbType.Integer).Value =
                    review.Cook != null ? (object)review.Cook.Id : DBNull.Value;

                cmd.Parameters.Add("@recipeId", OleDbType.Integer).Value =
                    review.Recipe != null ? (object)review.Recipe.Id : DBNull.Value;
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Review review = entity as Review;

            if (review != null)
            {
                string sqlStr = "UPDATE Reviews SET Rating = @rating, CommentText = @comment, ReviewDate = @rDate, CookID = @cookId, RecipeID = @recipeId WHERE ID = @id";

                cmd.CommandText = sqlStr;

                cmd.Parameters.Add("@rating", OleDbType.Integer).Value =
                    review.Rating;

                cmd.Parameters.Add("@comment", OleDbType.LongVarWChar).Value =
                    (object)review.CommentText ?? DBNull.Value;

                cmd.Parameters.Add("@rDate", OleDbType.Date).Value =
                    review.ReviewDate;

                cmd.Parameters.Add("@cookId", OleDbType.Integer).Value =
                    review.Cook != null ? (object)review.Cook.Id : DBNull.Value;

                cmd.Parameters.Add("@recipeId", OleDbType.Integer).Value =
                    review.Recipe != null ? (object)review.Recipe.Id : DBNull.Value;

                cmd.Parameters.Add("@id", OleDbType.Integer).Value =
                    review.Id;
            }
        }

        protected override void CreateDeletedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Review review = entity as Review;
            if (review != null)
            {
                string sqlStr = "DELETE FROM Reviews WHERE ID = @id";
                cmd.CommandText = sqlStr;
                cmd.Parameters.Add(new OleDbParameter("@id", review.Id));
            }
        }
    }
}