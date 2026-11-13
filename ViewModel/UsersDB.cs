using System;
using ClassLibrary4;
using System.Data.OleDb;

namespace ViewModel
{
    public class UsersDB : BaseDB
    {
        public UserList SelectAll()
        {
            command.Parameters.Clear();
            command.CommandText = "SELECT * FROM Users";
            UserList list = new UserList(base.Select());
            return list;
        }

        public override BaseEntity NewEntity()
        {
            return new User();
        }

        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            User user = entity as User;
            if (user != null)
            {
                if (reader["DisplayName"] != DBNull.Value)
                    user.DisplayName = reader["DisplayName"].ToString();
                if (reader["Email"] != DBNull.Value)
                    user.Email = reader["Email"].ToString();
                if (reader["PasswordHash"] != DBNull.Value)
                    user.PasswordHash = reader["PasswordHash"].ToString();
                if (reader["JoinDate"] != DBNull.Value)
                    user.JoinDate = Convert.ToDateTime(reader["JoinDate"]);
            }
            base.CreateModel(entity);
            return entity;
        }

        protected override void CreateInsertedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            User user = entity as User;

            if (user != null)
            {
                if (user is Admin)
                {
                    cmd.CommandText = "INSERT INTO Users (ID, DisplayName, Email, PasswordHash, JoinDate) VALUES (@id, @displayName, @email, @passHash, @joinDate)";
                    cmd.Parameters.Add(new OleDbParameter("@id", user.Id));
                }
                else
                {
                    cmd.CommandText = "INSERT INTO Users (DisplayName, Email, PasswordHash, JoinDate) VALUES (@displayName, @email, @passHash, @joinDate)";
                }

                cmd.Parameters.Add(new OleDbParameter("@displayName", user.DisplayName ?? (object)DBNull.Value));
                cmd.Parameters.Add(new OleDbParameter("@email", user.Email ?? (object)DBNull.Value));
                cmd.Parameters.Add(new OleDbParameter("@passHash", user.PasswordHash ?? (object)DBNull.Value));

                cmd.Parameters.Add("@joinDate", OleDbType.Date).Value = user.JoinDate;
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            User user = entity as User;
            if (user != null)
            {
                string sqlStr = "UPDATE Users SET DisplayName = @displayName, Email = @email, PasswordHash = @passHash, JoinDate = @joinDate WHERE ID = @id";
                cmd.CommandText = sqlStr;
                cmd.Parameters.Add(new OleDbParameter("@displayName", user.DisplayName ?? (object)DBNull.Value));
                cmd.Parameters.Add(new OleDbParameter("@email", user.Email ?? (object)DBNull.Value));
                cmd.Parameters.Add(new OleDbParameter("@passHash", user.PasswordHash ?? (object)DBNull.Value));
                cmd.Parameters.Add("@joinDate", OleDbType.Date).Value = user.JoinDate;
                cmd.Parameters.Add(new OleDbParameter("@id", user.Id));
            }
        }

        protected override void CreateDeletedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            User user = entity as User;
            if (user != null)
            {
                string sqlStr = "DELETE FROM Users WHERE ID = @id";
                cmd.CommandText = sqlStr;
                cmd.Parameters.Add(new OleDbParameter("@id", user.Id));
            }
        }
    }
}