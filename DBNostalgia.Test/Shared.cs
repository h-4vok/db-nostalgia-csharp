using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DBNostalgia.Test
{
    public static class Shared
    {
        public static string ConnectionString => ConfigurationManager.ConnectionStrings["local"].ConnectionString;
        public static Func<IDbConnection> BuildConnectionClosure => () => new SqlConnection(Shared.ConnectionString);
    }
}
