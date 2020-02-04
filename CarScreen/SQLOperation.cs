using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace CarScreen
{
   struct Elements
    {
        public string CarNumber, CarID, CarOwner;
    };
    public struct TagData
    {
        public string TagNumber, ActualNumber;

    }
    public struct TagElments 
    {
        public string TimeIn, TimeOut;
    
    };
    class SQLOperation
    {

        private String ConnectionString = "Data Source=DESKTOP-6R3HAJF;Initial Catalog=CarTransportation;Integrated Security=True";
        public void StoreTagNumber(String TagNumber)
        {
            int ActualNumber = GetActualNumber();
            if (ActualNumber != 0)
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();
                String Query = @"insert into TagIdentification (TagNumber,ActualNumber) values (@TagNumber,@ActualNumber)";
                SqlCommand cmd = new SqlCommand(Query, con);
                SqlParameter parmTagNumber = new SqlParameter("@TagNumber", TagNumber);
                cmd.Parameters.Add(parmTagNumber);
                SqlParameter parmActualNumber = new SqlParameter("@ActualNumber", ActualNumber);
                cmd.Parameters.Add(parmActualNumber);
                HashSet<String> TagNumberList = GetAllData();
                
                if (TagNumberList.Contains(TagNumber) == false)
                {
                    cmd.ExecuteNonQuery();
                    SetActualNumber(ActualNumber);
                }
                con.Close();
            }
            else
            {

                SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();
                String Query = @"insert into TagIdentification (TagNumber) values (@TagNumber)";
                SqlCommand cmd = new SqlCommand(Query, con);
                SqlParameter parmTagNumber = new SqlParameter("@TagNumber", TagNumber);
                cmd.Parameters.Add(parmTagNumber);
                HashSet<String> TagNumberList = GetAllData();
                if (TagNumberList.Contains(TagNumber) == false)
                    cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        private int GetActualNumber()
        {
            int ActualNumber = 0;
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            String Query = @"select TOP 1* from StudentTable where HasTagNumber IS NULL";
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ActualNumber = int.Parse(rdr["StudentIdentity"].ToString());
            }
            con.Close();
            return ActualNumber;
        }
        private void SetActualNumber(int ActualNumber)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            String Query = @"update StudentTable set HasTagNumber = 1 where StudentIdentity = @StudentIdentity";
            SqlCommand cmd = new SqlCommand(Query,con);
            SqlParameter parmActualNumber = new SqlParameter("@StudentIdentity",ActualNumber);
            cmd.Parameters.Add(parmActualNumber);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        private HashSet<String> GetAllData()
        {
            HashSet<String> TagNumberList = new HashSet<String>();
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            String Query = @"select TagNumber from TagIdentification";
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                String TagNumber = rdr["TagNumber"].ToString();
                TagNumberList.Add(TagNumber);
            }
            return TagNumberList;
        }
        
        public List<Elements> GetCarData()
        {
             List<Elements> CarData = new List<Elements>(); 
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string Query = @"select * from StudentTable";
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Elements Temp = new Elements();
                Temp.CarNumber = rdr["ClassNumber"].ToString();
                Temp.CarID = rdr["StudentIdentity"].ToString();
                Temp.CarOwner = rdr["StudentName"].ToString();
                CarData.Add(Temp);
            }
            con.Close();
            return CarData;
        }
        private String GetTagNumber(String ActualNumber)
        {
            String TagNumber="";
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            String Query = @"select TagNumber from TagIdentification where ActualNumber=@ActualNumber";
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlParameter parmActualNumber = new SqlParameter("@ActualNumber", ActualNumber);
            cmd.Parameters.Add(parmActualNumber);
            SqlDataReader rdr = cmd.ExecuteReader();
           if (rdr.Read())
            {
                TagNumber = rdr["TagNumber"].ToString();
            }

            con.Close();

            return TagNumber;
        }
        public TagElments GetCarTime(string CarNumber)
        {
            TagElments LastTime=new TagElments();
            String TagNum = GetTagNumber(CarNumber);
            string Query = @"select * from CarMovement where TagNumber=";
            Query += "'" + TagNum + "'";
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataReader rdr = cmd.ExecuteReader();
            List<String> myTagData = new List<String>();
            while (rdr.Read())
            {
                String temp;
                temp = rdr["TimeStatus"].ToString();
                myTagData.Add(temp);

            }
            if (myTagData.Count >0)
            {
                if (myTagData.Count == 1)
                {
                    LastTime.TimeIn = myTagData[0];
                    LastTime.TimeOut = "";

                }
                else
                {
                    LastTime.TimeIn = myTagData[0];
                    LastTime.TimeOut = myTagData[myTagData.Count - 1];
                }

            }
            con.Close();
            return LastTime;
        }

        public void InsertData (String TageNumber,String Date)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            String Query = @"insert into CarMovement (TagNumber,TimeStatus) 
                            values(@TagNumber,@TimeStatus)";
            SqlCommand cmd = new SqlCommand(Query,con);

            SqlParameter parmTagNumber = new SqlParameter("@TagNumber", TageNumber);
            cmd.Parameters.Add(parmTagNumber);

            SqlParameter parmTagDate = new SqlParameter("@TimeStatus", Date);
            cmd.Parameters.Add(parmTagDate);
            cmd.ExecuteNonQuery();
            con.Close();
        }


    }
}
