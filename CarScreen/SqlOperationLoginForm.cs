using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using System.Data.SqlClient;


namespace StudentAttendance
{
   public class SqlOperationLoginForm
    {
       public String ConnectionString = "Data Source=DESKTOP-6R3HAJF;Initial Catalog=CarTransportation;Integrated Security=True";
       public bool LoginCheck(String UserName,String Password)
        {
            bool IsRight = false;
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string Query = "select * from dbo.AdminTable";
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataReader rdr = cmd.ExecuteReader();
            String ThisUserName="", ThisPassword="";
            while (rdr.Read())
            {
                ThisUserName = rdr["UserName"].ToString();
                ThisPassword = rdr["Password"].ToString();
            }
            if (ThisUserName == UserName && ThisPassword == Password) IsRight = true;
            return IsRight;
        }
    }
}
