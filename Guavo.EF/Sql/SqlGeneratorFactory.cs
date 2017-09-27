using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guavo.EF
{
    public class SqlGeneratorFactory
    {

        public static ISqlGenerator GetSqlGenerator(string providerName)
        {
            ISqlGenerator sqlGenerator = null;
            switch (providerName) {
                case "MySql.Data.MySqlClient":
                    sqlGenerator = new MySqlGenerator();
                    break;
                case "System.Data.SqlClient":
                    sqlGenerator = new MSSqlGenerator();
                    break;
                case "System.Data.OracleClient":
                    sqlGenerator = new OracelSqlGenerator();
                    break;
            }

            return sqlGenerator;
        
        }

    }
}
