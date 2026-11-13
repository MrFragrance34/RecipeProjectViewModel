using ClassLibrary4;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public delegate void CreateSql(BaseEntity entity, OleDbCommand command);

    public class ChangeEntity
    {
        private BaseEntity entity;
        private CreateSql createSql;
        public ChangeEntity(CreateSql createSql, BaseEntity entity)
        {
            this.createSql = createSql;
            this.entity = entity;
        }
        public BaseEntity Entity { get => entity; set => entity = value; }
        public CreateSql CreateSql { get => createSql; set => createSql = value; }
    }

    public abstract class BaseDB
    {
        protected static string connectionString = BuildConnectionString();

        protected OleDbConnection connection;
        protected OleDbCommand command;
        protected OleDbDataReader reader;

        public static List<ChangeEntity> inserted = new List<ChangeEntity>();
        public static List<ChangeEntity> updated = new List<ChangeEntity>();
        public static List<ChangeEntity> deleted = new List<ChangeEntity>();

        private static string BuildConnectionString()
        {
            const string databaseName = "DatabaseYonaRecipe.accdb";

            DirectoryInfo directory = new DirectoryInfo(AppContext.BaseDirectory);

            while (directory != null)
            {
                string projectPath = Path.Combine(
                    directory.FullName,
                    "ViewModel",
                    "ViewModel",
                    databaseName);

                if (File.Exists(projectPath))
                {
                    return $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={projectPath};Persist Security Info=False;";
                }

                directory = directory.Parent;
            }

            throw new FileNotFoundException(
                "Could not find " + databaseName);
        }

        public BaseDB()
        {
            connection = new OleDbConnection(connectionString);
            command = new OleDbCommand();
            command.Connection = connection;
        }

        public abstract BaseEntity NewEntity();

        protected List<BaseEntity> Select()
        {
            List<BaseEntity> list = new List<BaseEntity>();
            try
            {
                command.Connection = connection;
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    BaseEntity entity = NewEntity();
                    list.Add(CreateModel(entity));
                }
            }
            catch (Exception e)
            {
                throw new Exception(
                    e.Message + "\nSQL:" + command.CommandText);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }

                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return list;
        }

        protected virtual BaseEntity CreateModel(BaseEntity entity)
        {
            entity.Id = (int)reader["id"];
            return entity;
        }


        protected abstract void CreateInsertedSQL(BaseEntity entity, OleDbCommand cmd);
        protected abstract void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd);
        protected abstract void CreateDeletedSQL(BaseEntity entity, OleDbCommand cmd);

            public virtual void Insert(BaseEntity entity)
            {
                BaseEntity reqEntity = this.NewEntity();
                if (entity != null && entity.GetType() == reqEntity.GetType())
                {
                    inserted.Add(new ChangeEntity(this.CreateInsertedSQL, entity));
                }
            }

        public virtual void Update(BaseEntity entity)
        {
            BaseEntity reqEntity = this.NewEntity();
            if (entity != null && entity.GetType() == reqEntity.GetType())
            {
                updated.Add(new ChangeEntity(this.CreateUpdatedSQL, entity));
            }
        }

        public virtual void Delete(BaseEntity entity)
        {
            BaseEntity reqEntity = this.NewEntity();
            if (entity != null && entity.GetType() == reqEntity.GetType())
            {
                deleted.Add(new ChangeEntity(this.CreateDeletedSQL, entity));
            }
        }

        public int SaveChanges()
        {
            int sum = 0;
            try
            {
                command.Connection = connection;
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                foreach (ChangeEntity item in inserted)
                {
                    command.Parameters.Clear();
                    item.CreateSql(item.Entity, command);
                    sum += command.ExecuteNonQuery();
                }
                inserted.Clear();

                foreach (ChangeEntity item in updated)
                {
                    command.Parameters.Clear();
                    item.CreateSql(item.Entity, command);
                    sum += command.ExecuteNonQuery();
                }
                updated.Clear();

                foreach (ChangeEntity item in deleted)
                {
                    command.Parameters.Clear();
                    item.CreateSql(item.Entity, command);
                    sum += command.ExecuteNonQuery();
                }
                deleted.Clear();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "\nSQL:" + command.CommandText);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return sum;
        }
    }
}