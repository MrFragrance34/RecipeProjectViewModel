using System;
using ClassLibrary4;
using System.Data.OleDb;

namespace ViewModel
{
    public class CategoryDB : BaseDB
    {
        public CategoryList SelectAll()
        {
            command.Parameters.Clear();
            command.CommandText = "SELECT * FROM Categories";
            CategoryList list = new CategoryList(base.Select());
            return list;
        }
        public Category SelectById(int id)
        {
            command.Parameters.Clear();
            command.CommandText = "SELECT * FROM Categories WHERE ID = @id";
            command.Parameters.Add(new OleDbParameter("@id", id));

            List<BaseEntity> list = base.Select();
            return list.Count > 0 ? (Category)list[0] : null;
        }

        public override BaseEntity NewEntity()
        {
            return new Category();
        }

        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Category category = entity as Category;
            if (category != null)
            {
                if (reader["CategoryName"] != DBNull.Value)
                    category.CategoryName = reader["CategoryName"].ToString();
            }
            base.CreateModel(entity);
            return entity;
        }

        protected override void CreateInsertedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Category category = entity as Category;
            if (category != null)
            {
                string sqlStr = "INSERT INTO Categories (CategoryName) VALUES (@name)";
                cmd.CommandText = sqlStr;
                cmd.Parameters.Add(new OleDbParameter("@name", category.CategoryName ?? (object)DBNull.Value));
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Category category = entity as Category;
            if (category != null)
            {
                string sqlStr = "UPDATE Categories SET CategoryName = @name WHERE ID = @id";
                cmd.CommandText = sqlStr;
                cmd.Parameters.Add(new OleDbParameter("@name", category.CategoryName ?? (object)DBNull.Value));
                cmd.Parameters.Add(new OleDbParameter("@id", category.Id));
            }
        }

        protected override void CreateDeletedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Category category = entity as Category;
            if (category != null)
            {
                string sqlStr = "DELETE FROM Categories WHERE ID = @id";
                cmd.CommandText = sqlStr;
                cmd.Parameters.Add(new OleDbParameter("@id", category.Id));
            }
        }
    }
}   