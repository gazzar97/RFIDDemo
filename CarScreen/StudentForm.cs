using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarScreen
{
    public partial class StudentForm : Form
    {
        StudentOperation so = new StudentOperation();
        public StudentForm()
        {
            InitializeComponent();
        }

        private void StudentForm_Load(object sender, EventArgs e)
        {
            FillStudentID();
        }
        private void FillStudentID()
        {
            
            List<int> tmp = so.DataOfStudent();
            comboBox1.Items.Clear();
            for (int i = 0; i < tmp.Count; i++)
            {
               
                comboBox1.Items.Add(tmp[i]);

            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String StudentName = textBox1.Text.ToString();
            String ClassNumber = textBox2.Text.ToString();
            if (StudentName.Length == 0 || ClassNumber.Length == 0)
            {
                MessageBox.Show("Invalid Data!");
                return;

            }
            else
            { so.StudentData(StudentName, ClassNumber); FillStudentID();
              // Set TagNumber
            
            
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int CarID = int.Parse(comboBox1.Text.ToString());

            string CarOwner = textBox1.Text.ToString();
            string CarNumber = textBox2.Text.ToString();
            so.Update(CarOwner,CarNumber,CarID);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Delete 
            int StudentNumber = int.Parse (comboBox1.Text.ToString());
            comboBox1.Items[comboBox1.SelectedIndex] = string.Empty;
            so.Delete(StudentNumber);
            FillStudentID();
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string TextCombo = comboBox1.Text.ToString();
            if (TextCombo.Length == 0) return;
            int CarID = int.Parse(comboBox1.Text.ToString());
            CarData tmp= so.SelectCarData(CarID);
            textBox1.Text = tmp.CarOwner;
            textBox2.Text = tmp.CarNumber;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            HomePage f = new HomePage();
            f.Show();
        }
    }
}
