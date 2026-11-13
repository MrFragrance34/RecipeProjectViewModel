
using System;
using ClassLibrary4;
using System.Data.OleDb;

namespace ViewModel
{
    public class AdminsDB : UsersDB
    {
        public AdminList SelectAll()
        {
            command.Parameters.Clear();
            command.CommandText = "SELECT Users.ID, Users.DisplayName, Users.Email, Users.PasswordHash, Users.JoinDate, Admins.AdminRole FROM Users INNER JOIN Admins ON Users.ID = Admins.ID";

            AdminList list = new AdminList(base.Select());
            return list;
        }

        public override BaseEntity NewEntity()
        {
            return new Admin();
        }

        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Admin admin = entity as Admin;

            if (admin != null)
            {
                admin.DisplayName = reader["DisplayName"].ToString();
                admin.Email = reader["Email"].ToString();
                admin.PasswordHash = reader["PasswordHash"].ToString();

                if (reader["JoinDate"] != DBNull.Value)
                    admin.JoinDate = Convert.ToDateTime(reader["JoinDate"]);

                if (reader["AdminRole"] != DBNull.Value)
                    admin.AdminRole = reader["AdminRole"].ToString();
            }

            base.CreateModel(entity);
            return entity;
        }

        protected override void CreateInsertedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Admin admin = entity as Admin;

            if (admin != null)
            {
                string sqlStr = "INSERT INTO Admins (ID, AdminRole) VALUES (@id, @role)";
                cmd.CommandText = sqlStr;

                cmd.Parameters.Add(new OleDbParameter("@id", admin.Id));
                cmd.Parameters.Add(new OleDbParameter("@role", admin.AdminRole ?? (object)DBNull.Value));
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Admin admin = entity as Admin;

            if (admin != null)
            {
                string sqlStr = "UPDATE Admins SET AdminRole = @role WHERE ID = @id";
                cmd.CommandText = sqlStr;

                cmd.Parameters.Add(new OleDbParameter("@role", admin.AdminRole ?? (object)DBNull.Value));
                cmd.Parameters.Add(new OleDbParameter("@id", admin.Id));
            }
        }

        protected override void CreateDeletedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Admin admin = entity as Admin;

            if (admin != null)
            {
                string sqlStr = "DELETE FROM Admins WHERE ID = @id";
                cmd.CommandText = sqlStr;

                cmd.Parameters.Add(new OleDbParameter("@id", admin.Id));
            }
        }

        public override void Insert(BaseEntity entity)
        {
            Admin admin = entity as Admin;

            if (admin != null)
            {
                UsersDB usersDB = new UsersDB();
                UserList users = usersDB.SelectAll();

                int maxId = 0;

                foreach (User user in users)
                {
                    if (user.Id > maxId)
                        maxId = user.Id;
                }

                admin.Id = maxId + 1;

                inserted.Add(new ChangeEntity(base.CreateInsertedSQL, entity));
                inserted.Add(new ChangeEntity(this.CreateInsertedSQL, entity));
            }
        }

        public override void Update(BaseEntity entity)
        {
            Admin admin = entity as Admin;

            if (admin != null)
            {
                updated.Add(new ChangeEntity(this.CreateUpdatedSQL, entity));
                updated.Add(new ChangeEntity(base.CreateUpdatedSQL, entity));
            }
        }

        public override void Delete(BaseEntity entity)
        {
            Admin admin = entity as Admin;

            if (admin != null)
            {
                deleted.Add(new ChangeEntity(this.CreateDeletedSQL, entity));
                deleted.Add(new ChangeEntity(base.CreateDeletedSQL, entity));
            }
        }
    }
}
