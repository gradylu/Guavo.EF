using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guavo.EF
{
    public interface ISqlGenerator
    {
        string InsertSQL<T>();

        string UpdateSQL<T>();

        string DeleteSQL<T>();

        string CountSQL<T>();

        string Select<T>();

    }
}
