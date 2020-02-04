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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String UserName = textBox1.Text.ToString();
            String Password = textBox2.Text.ToString();
            if (UserName.Length != 0 && Password.Length != 0)
            {
                LoginOperation SqlOp = new LoginOperation();
                bool bol = SqlOp.LoginCheck(UserName, Password);
                if (bol)
                {
                    //True
                    this.Hide();
                    HomePage f = new HomePage();
                    f.Show();
                }
                else
                {
                    //False
                    label3.Visible = true;
                    label3.Text = "User Name or Password are Incorrect!";
                }
            }
            else
            {
                label3.Visible = true;
                label3.Text = "User Name or Password are Missed!";


            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            label3.Visible = false;

        }
    }
}
