using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace CarScreen
{
    public struct CarData
    {
       public string CarOwner, CarNumber;
       public  int CarID;
    };
    public class StudentOperation
    {
        public String ConnectionString = "Data Source=DESKTOP-6R3HAJF;Initial Catalog=CarTransportation;Integrated Security=True";
        public void StudentData(string StudentName, string ClassNumber)
        {

            SqlConnection con = new SqlConnection(ConnectionString);

            con.Open();
            string InsertSql = @"insert into StudentTable (ClassNumber,StudentName)
                                values (@ClassNumber,@StudentName)";
            SqlCommand cmd = new SqlCommand(InsertSql, con);
            SqlParameter parameterClassNumber = new SqlParameter("@ClassNumber", ClassNumber);
            cmd.Parameters.Add(parameterClassNumber);

            SqlParameter parameterStudentName = new SqlParameter("@StudentName", StudentName);
            cmd.Parameters.Add(parameterStudentName);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Data Has Addded");
            SetTagNumber();

        }
        public void Update(string StudentName, string ClassNumber, int StudentNumber)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string Update = ("UPDATE StudentTable set ClassNumber=@ClassNumber , StudentName=@StudentName where StudentIdentity=@StudentIdentity");
            SqlCommand cmd = new SqlCommand(Update, con);


            SqlParameter parameterClass = new SqlParameter("@ClassNumber", ClassNumber);
            cmd.Parameters.Add(parameterClass);


            SqlParameter parameterName = new SqlParameter("@StudentName", StudentName);
            cmd.Parameters.Add(parameterName);


            SqlParameter parameterID = new SqlParameter("@StudentIdentity", StudentNumber);
            cmd.Parameters.Add(parameterID);

            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Record Update Successfuly");
        }
        public void Delete(int StudentNumber)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string Delete = ("DELETE FROM StudentTable WHERE StudentIdentity=@StudentIdentity ");
            SqlCommand cmd = new SqlCommand(Delete, con);

            SqlParameter parameterStudentName = new SqlParameter("@StudentIdentity", StudentNumber);
            cmd.Parameters.Add(parameterStudentName);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("The Record Is Deleted ");
        }
        public List<int> DataOfStudent()
        {
            List<int> StudentNumberList = new List<int>();
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string Select = ("select StudentIdentity  from StudentTable");
            SqlCommand cmd = new SqlCommand(Select, con);

            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int tmp = int.Parse(rdr["StudentIdentity"].ToString());
                StudentNumberList.Add(tmp);

            }
            con.Close();
            return StudentNumberList;
        }
        public CarData SelectCarData(int CarID)
        {
            CarData tmp = new CarData();

            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            String Query = @"select * from StudentTable where StudentIdentity=@StudentIdentity";
            SqlCommand cmd = new SqlCommand(Query,con);
            SqlParameter parmCarID = new SqlParameter("@StudentIdentity",CarID);
            cmd.Parameters.Add(parmCarID);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                tmp.CarOwner = rdr["StudentName"].ToString(); //Student Name == CarOwner
                tmp.CarNumber = rdr["ClassNumber"].ToString();//Class Number == CarNumber
                tmp.CarID = int.Parse(rdr["StudentIdentity"].ToString());
            }
            con.Close();
            return tmp;
        }

        public void SetTagNumber()
        {
            int ActualNumber = GetActualNumberCar();
            int TagID = GetTagNumber();
            if (ActualNumber != 0)
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();
                String Query = @"update TagIdentification set ActualNumber=@ActualNumber where IdNumber = @IdNumber";
                SqlCommand cmd = new SqlCommand(Query,con);
                SqlParameter parmActualNumber = new SqlParameter("@ActualNumber",ActualNumber);
                cmd.Parameters.Add(parmActualNumber);

                SqlParameter parmTagID = new SqlParameter("@IdNumber", TagID);
                cmd.Parameters.Add(parmTagID);
                cmd.ExecuteNonQuery();
                con.Close();
                SetHasTagNumber(ActualNumber);

            }


        }
        private void SetHasTagNumber(int ID)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            String Query = @"update StudentTable set HasTagNumber=@HasTagNumber where StudentIdentity=@StudentIdentity";
            SqlCommand cmd = new SqlCommand(Query, con);

            SqlParameter parmID = new SqlParameter("@StudentIdentity", ID);
            cmd.Parameters.Add(parmID);

            SqlParameter parmAC = new SqlParameter("@HasTagNumber", 1);
            cmd.Parameters.Add(parmAC);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        private int GetTagNumber()
        {
            // return TagNumber
            int TagID = 0;
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            String Query = @"select Top 1* from TagIdentification where ActualNumber is Null";
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                TagID = int.Parse(rdr["IdNumber"].ToString());
            }
            con.Close();
            return TagID;
        }

        private int GetActualNumberCar()
        {

            //return ActualNumber
            int ActualNumber = 0;

            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            String Query = @"select Top 1* from StudentTable where HasTagNumber is Null";
            SqlCommand cmd = new SqlCommand(Query,con);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                ActualNumber = int.Parse(rdr["StudentIdentity"].ToString());
            }

            con.Close();
            return ActualNumber;
        }

    }

}
