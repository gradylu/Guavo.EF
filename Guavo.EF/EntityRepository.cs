using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Configuration;
using System.Data.Common;

namespace Guavo.EF
{
    public class EntityRepository
    {


        protected IDbConnection DbConnection { get; private set; }

        protected ConnectionStringSettings ConnectionStringSetting { get; private set; }

        protected ISqlGenerator SqlGenerator { get; private set; }

        public EntityRepository(string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                if(ConfigurationManager.ConnectionStrings.Count > 0)
                {
                    this.ConnectionStringSetting = ConfigurationManager.ConnectionStrings[0];
                }
            }
            else
            {
                this.ConnectionStringSetting = ConfigurationManager.ConnectionStrings[name];
            }

            DbProviderFactory f = DbProviderFactories.GetFactory(ConnectionStringSetting.ProviderName);
            this.DbConnection = f.CreateConnection();
            DbConnection.ConnectionString = this.ConnectionStringSetting.ConnectionString;

            this.SqlGenerator = SqlGeneratorFactory.GetSqlGenerator(this.ConnectionStringSetting.ProviderName);

        }

        public void Open()
        {
            this.DbConnection.Open();
        }

        public void Close()
        {
            this.DbConnection.Close();
        }

        public IDbTransaction BeginTransaction()
        {
            return DbConnection.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return DbConnection.BeginTransaction(il);
        }

        #region insert,update,delete

        public int Insert<T>(T entity, IDbTransaction transaction = null)
        {        
            return this.DbConnection.Execute(SqlGenerator.InsertSQL<T>(), entity, transaction, null, CommandType.Text);
        }

        public int Insert<T>(List<T> entities, IDbTransaction transaction = null)
        {
            return this.DbConnection.Execute(SqlGenerator.InsertSQL<T>(), entities, transaction, null, CommandType.Text);
        }

        public int Update<T>(T entity, IDbTransaction transaction = null)
        {
            return this.DbConnection.Execute(SqlGenerator.UpdateSQL<T>(), entity, transaction, null, CommandType.Text);
        }

        public int Update<T>(List<T> entities, IDbTransaction transaction = null)
        {
            return this.DbConnection.Execute(SqlGenerator.UpdateSQL<T>(), entities, transaction, null, CommandType.Text);
        }

        public int Delete<T>(T entity, IDbTransaction transaction = null)
        {
            return this.DbConnection.Execute(SqlGenerator.DeleteSQL<T>(), entity, transaction, null, CommandType.Text);
        }

        public int Delete<T>(List<T> entities, IDbTransaction transaction = null)
        {
            return this.DbConnection.Execute(SqlGenerator.DeleteSQL<T>(), entities, transaction, null, CommandType.Text);
        }

        #endregion insert,update,delete

        #region query

      

        #endregion query

    }
}
