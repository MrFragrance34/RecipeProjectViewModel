using System;
using ClassLibrary4;
using System.Data.OleDb;

namespace ViewModel
{
    public class CookDB : BaseDB
    {
        public CookList SelectAll()
        {
            command.Parameters.Clear();
            command.CommandText = "SELECT * FROM Cooks";
            CookList list = new CookList(base.Select());
            return list;
        }

        public static Cook SelectById(int id)
        {
            CookDB db = new CookDB();
            db.command.CommandText = "SELECT * FROM Cooks WHERE ID = @id";
            db.command.Parameters.Add(new OleDbParameter("@id", id));

            List<BaseEntity> list = db.Select();
            return list.Count > 0 ? (Cook)list[0] : null;
        }

        public override BaseEntity NewEntity()
        {
            return new Cook();
        }

        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Cook cook = entity as Cook;
            if (cook != null)
            {
                cook.Bio = reader["Bio"].ToString();
                if (reader["ProfilePicPath"] != DBNull.Value)
                    cook.ProfilePicPath = reader["ProfilePicPath"].ToString();
            }
            base.CreateModel(entity);
            return entity;
        }

        protected override void CreateInsertedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Cook cook = entity as Cook;
            if (cook != null)
            {
                // אם ה-ID לא הוגדר מבחוץ, נחשב אותו אוטומטית לפי מקסימום בטבלה
                if (cook.Id == 0)
                {
                    CookDB db = new CookDB();
                    CookList list = db.SelectAll();
                    int maxId = 0;
                    foreach (Cook c in list)
                    {
                        if (c.Id > maxId)
                            maxId = c.Id;
                    }
                    cook.Id = maxId + 1;
                }

                string sqlStr = "INSERT INTO Cooks (ID, Bio, ProfilePicPath) VALUES (@id, @bio, @pic)";
                cmd.CommandText = sqlStr;
                cmd.Parameters.Add(new OleDbParameter("@id", cook.Id));
                cmd.Parameters.Add(new OleDbParameter("@bio", cook.Bio ?? (object)DBNull.Value));
                cmd.Parameters.Add(new OleDbParameter("@pic", cook.ProfilePicPath ?? (object)DBNull.Value));
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Cook cook = entity as Cook;
            if (cook != null)
            {
                string sqlStr = "UPDATE Cooks SET Bio = @bio, ProfilePicPath = @pic WHERE ID = @id";
                cmd.CommandText = sqlStr;
                cmd.Parameters.Add(new OleDbParameter("@bio", cook.Bio ?? (object)DBNull.Value));
                cmd.Parameters.Add(new OleDbParameter("@pic", cook.ProfilePicPath ?? (object)DBNull.Value));
                cmd.Parameters.Add(new OleDbParameter("@id", cook.Id));
            }
        }

        protected override void CreateDeletedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Cook cook = entity as Cook;
            if (cook != null)
            {
                string sqlStr = "DELETE FROM Cooks WHERE ID = @id";
                cmd.CommandText = sqlStr;
                cmd.Parameters.Add(new OleDbParameter("@id", cook.Id));
            }
        }
    }
}