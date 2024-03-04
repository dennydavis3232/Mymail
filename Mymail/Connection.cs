using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Mymail
{
    class Connection
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CON"].ConnectionString);
        SqlCommand cmd = new SqlCommand();

        public DataTable ExecuteGetDataTable(string strCMDtext)
        {
            DataTable Dt = null; SqlDataAdapter Da = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CON"].ConnectionString))
            {
                conn.Open();

                cmd = new SqlCommand(strCMDtext, conn);
                cmd.CommandType = CommandType.Text;
                Dt = new DataTable();
                Da = new SqlDataAdapter(cmd);
                Da.Fill(Dt);
                Da.Dispose();

            }
            return Dt;
        }

    }
}