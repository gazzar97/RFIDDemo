using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RfidApiLib;
namespace CarScreen
{
    public partial class HomePage : Form
    {
        RfidApi Api = new RfidApi();
        public byte EpcReading = 0;
        public int TagCnt = 0;
        public HashSet<string> mySet = new HashSet<string>();
        SQLOperation SO = new SQLOperation();
        bool isCLosed = false;
        public HomePage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Add Tag
            this.Hide();
            Form1 f = new Form1();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Add Car
            this.Hide();
            StudentForm f = new StudentForm();
            f.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Connect To Rfid
            isCLosed = false;
            int status;
            byte v1 = 0;
            byte v2 = 0;
            string s = "";
            status = Api.OpenCommPort("COM3:");
            if (status != 0)
            {
                listView1.Items.Add("Failed To Open RFID Reader!");
                return;

            }
            status = Api.GetFirmwareVersion(ref v1, ref v2);
            if (status != 0)
            {
                listView1.Items.Add("Can not connect with the reader!");
                Api.CloseCommPort();
                return;
            }
            listView1.Items.Add("Connect the reader success!");
            s = string.Format("The reader's firmware version is:V{0:d2}.{1:d2}", v1, v2);
            listView1.Items.Add(s);
            Set_Ant();
            bAntQuery();
            StartReading();
        }

        private void HomePage_Load(object sender, EventArgs e)
        {
           
            FillDataGrid();
        }
        private void StartReading()
        {
            if (EpcReading == 0)
            {
                Api.ClearIdBuf();

                listView1.Items.Add("Start multiply tags identify!");
                TagCnt = 0;
                timer2.Interval = (2 + 1) * 20;
                timer2.Enabled = true;
                
                EpcReading = 1;
                mySet = new HashSet<string>();

            }
            else
            {
                timer2.Enabled = false;
                EpcReading = 0;
                this.BackColor = Color.MidnightBlue;
            }

        }
      
        private void button4_Click(object sender, EventArgs e)
        {
            // disconnect
            this.BackColor = Color.White;
            if (isCLosed == false)
            {
                Api.CloseCommPort();
                isCLosed = true;

            }
        }
        private void Set_Ant()
        {
            byte ant_sel = 0;
            int status;
            ant_sel |= 0x01;
            status = Api.SetAnt(ant_sel);
            if (status != 0)
            {
                listView1.Items.Add("Set ant failed!");
                return;
            }
            listView1.Items.Add("Set ant success!");
        }
        private void bAntQuery()
        {
            byte ant_sel = 0;

            int status;

            status = Api.GetAnt(ref ant_sel);
            if (status != 0)
            {
                listView1.Items.Add("Get Ant settings failed!");
                return;
            }
            listView1.Items.Add("Get Ant settings success!");
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            int status;
            int i, j;
            byte[,] IsoBuf = new byte[100, 12];
            // FileOperation FO = new FileOperation();
            byte tag_cnt = 0;
            byte tagCnt = 0;
            string s = "";
            string s1 = "";
            byte tag_flag = 0;
            string tmp = "";

            status = Api.EpcMultiTagIdentify(ref IsoBuf, ref tag_cnt, ref tag_flag);

            if (tag_flag == 1)
                this.BackColor = Color.White;
            else
                this.BackColor = Color.Gray;
            if (IsoBuf.Length > 0)
            {
                for (i = 0; i <= tagCnt; i++)
                {
                    s1 = string.Format("NO.{0:D}: ", TagCnt);
                    for (j = 0; j < 12; j++)
                    {
                        s = string.Format("{0:X2} ", IsoBuf[i, j]);
                        s1 += s;
                        tmp += s;
                    }
                    string Date = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt");
                    if (mySet.Contains(Date) == false && tmp != "00 00 00 00 00 00 00 00 00 00 00 00 ")
                    {
                        mySet.Add(Date);

                        
                        //SO.StoreTagNumber(tmp);
                        SO.InsertData(tmp, Date);
                        listView1.Items.Add(s1);
                        TagCnt++;
                    }
                    else { tmp = ""; }
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            FillDataGrid();
        }
        private void FillDataGrid()
        {
            dataGridView1.Rows.Clear();
            List<Elements> CarData = new List<Elements>();
            CarData = SO.GetCarData();
            foreach (var elment in CarData)
            {
                TagElments TimeOfCar = SO.GetCarTime(elment.CarID);
                dataGridView1.Rows.Add(elment.CarOwner, elment.CarNumber, TimeOfCar.TimeIn, TimeOfCar.TimeOut, elment.CarID);

            }
        }
    }
}
