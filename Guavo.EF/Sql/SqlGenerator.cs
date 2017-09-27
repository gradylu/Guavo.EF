using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guavo.EF
{
    public abstract  class SqlGenerator
    {
        public virtual string CountSQL<T>()
        {
            string tableName = string.Empty;
            var type = typeof(T);
            TableAttribute tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                tableName = tableAttr.Name;
            }
            else
            {
                tableName = type.Name;
            }


            return $"SELECT COUNT(*) FROM {tableName}";
        }

        public virtual string DeleteSQL<T>()
        {
            string tableName = string.Empty;
            var type = typeof(T);

            TableAttribute tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                tableName = tableAttr.Name;
            }
            else
            {
                tableName = type.Name;
            }

            List<string> keyList = new List<string>();
            PropertyInfo[] pInfos = type.GetProperties();
            foreach (var pInfo in pInfos)
            {
                if (pInfo.GetCustomAttribute<KeyAttribute>() != null)
                {
                    keyList.Add(string.Format("{0}=@{0}", pInfo.Name));
                }
            }

            if (keyList.Count == 0)
            {
                var pInfo = pInfos.ToList().Find(item => item.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase));
                if (pInfo != null)
                {
                    keyList.Add(string.Format("{0}=@{0}", pInfo.Name));
                }
            }

            if (keyList.Count == 0)
            {
                throw new KeyNotFoundException($"{tableName}没有主键设置");
            }

            return string.Format($"DELETE FROM {tableName} WHERE {0}", string.Join(" AND ", keyList));

        }

        public virtual string InsertSQL<T>()
        {
            string tableName = string.Empty;
            var type = typeof(T);
            PropertyInfo[] pInfos = type.GetProperties();
            TableAttribute tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                tableName = tableAttr.Name;
            }
            else
            {
                tableName = type.Name;
            }

            var insertSQL = new StringBuilder($"INSERT INTO {tableName} ");
            var fields = new List<string>();
            var values = new List<string>();

            foreach (var pinfo in pInfos)
            {
                fields.Add(pinfo.Name);
                values.Add($"@{pinfo.Name}");
            }

            insertSQL.AppendFormat("({0}) VALUES ({1}); ", string.Join(",", fields), string.Join(",", values));
            return insertSQL.ToString();
        }

        public virtual string Select<T>()
        {
            string tableName = string.Empty;
            var type = typeof(T);
            PropertyInfo[] pInfos = type.GetProperties();
            TableAttribute tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                tableName = tableAttr.Name;
            }
            else
            {
                tableName = type.Name;
            }

            return $"SELECT * FROM {tableName}";

        }

        public virtual string UpdateSQL<T>()
        {
            string tableName = string.Empty;
            var type = typeof(T);
            PropertyInfo[] pInfos = type.GetProperties();
            TableAttribute tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                tableName = tableAttr.Name;
            }
            else
            {
                tableName = type.Name;
            }

            var insertSQL = new StringBuilder($"INSERT INTO {tableName} ");
            var fields = new List<string>();
            var keys = new List<string>();
            foreach (var pinfo in pInfos)
            {
                if (pinfo.GetCustomAttribute<KeyAttribute>() != null)
                {
                    keys.Add($"{pinfo.Name}=@{pinfo.Name}");
                }

                fields.Add($"{pinfo.Name}=@{pinfo.Name}");
            }

            if (keys.Count == 0)
            {
                var pInfo = pInfos.ToList().Find(item => item.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase));
                if (pInfo != null)
                {
                    keys.Add($"{pInfo.Name}=@{pInfo.Name}");
                }
            }

            if (keys.Count == 0)
            {
                throw new KeyNotFoundException($"{tableName}没有主键设置");
            }

            return string.Format($"UPDATE {tableName} SET {0} WHERE {1}", string.Join(",", fields), string.Join(" AND ", keys));
        }
    }
}
