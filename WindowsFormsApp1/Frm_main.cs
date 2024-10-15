using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Frm_main : Form
    {
        private bool _bProcess = false;

        public unsafe Frm_main()
        {
            InitializeComponent();
            //                                      x
            int number = 0x23F21789;// xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx


            // Test Extension
            byte[] res = Extension.FromInt(number);
            int test = Extension.ToInt(res);

            int* pNumber = &number;
            int value = *pNumber;
            byte* pBuffer = (byte*)pNumber;
            //pBuffer++;
            //byte, sbyte, char, short, ushort, int, uint, long, ulong, decimal, double, float
            //Primary type

            string name = "Phuc Le";

            fixed(char* pName = name)
            {
                char* p = pName;
                p++;
                int* pInt = (int*) pName;
            //    pInt--;
                int len = *pInt;// '\0' 'A' 'B' '\n' '\b'
            }
            //byte[], int[]
            //
            int n = name.ToInt();
            n = "12345".ToInt();

            // Test Buffer
            //Buffer buffer = new Buffer(1024);
            //buffer.SetValue(5, "Phuc");
            //string test1 = buffer.GetString(5, 4);

            //buffer.SetValue(5, "Phuc", false);
            //string test2 = buffer.GetString(5, 4);

            // Test PLLib
            PLLib.Buffer myBuffer = new PLLib.Buffer();
            myBuffer.SetValue(10, 50);
            myBuffer.GetInt(10);
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void BtnFindPrime_Click(object sender, EventArgs e)
        {
            string res = "";
           // int toNumber = 0;
            // bool isNumber = Convert.ToInt32();
            bool isNumber = int.TryParse(TxtNumber.Text, out int toNumber);
            if (!isNumber)
            {
                MessageBox.Show("The input is not a number!");
                return;
            }

            _bProcess = true;

            List<int> lstInt = FindPrime(2, toNumber);

            if (lstInt.Count > 0)
            {
                foreach (int i in lstInt)
                {
                    res += ", " + i;
                }
                label1.Text = res.Substring(2);
            }
            else { label1.Text = "NOT FOUND"; }

            _bProcess = false;

        }

        private bool IsPrime(int n)
        {
            if (n < 2)
            {
                return false;
            }
            else if (n == 2)
            {
                return true;
            }
            else if (n % 2 == 0)
            {
                return false;
            }

            int half = n / 2;

            for (int i = 3; i <= half; i++)
            {
                if (n % i == 0)
                {
                    return false;
                }
                Application.DoEvents();
                if (!_bProcess)
                {
                    return false;
                }
            }
            return true;
        }
        
        private List<int> FindPrime(int from, int to)
        {
            List<int> lstPrime = new List<int>();

            if (from < 3)
            {
                from = 1;
            }
            else if (from % 2 == 0)
            {
                from++;
            }

            int tick = Environment.TickCount;

            for (int i = from; i <= to; i += 2)
            {
                if (IsPrime(i))
                {
                    lstPrime.Add(i);

                }
                Application.DoEvents();
                if (!_bProcess)
                {
                    return lstPrime;
                }
            }
            //Parallel.For(from, to, number =>
            //{
            //    if (IsPrime(number))
            //    {
            //        lstPrime.Add(number);
            //    }
            //});

            tick = Environment.TickCount - tick;
            Debug.Print("Run time is " + tick);
             return lstPrime;


        }


        private void BtnCancel_Click(object sender, EventArgs e)
        {
            _bProcess = false;
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            label1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //myProgressBar1.LockUpdate = true;
            //myProgressBar1.Minimum = 0;
            //myProgressBar1.Maximum = 100;
            //myProgressBar1.Value = 0;
            //myProgressBar1.LockUpdate = false;
            //for (int i = 0; i <= 100; i++)
            //{
            //    progressBar1.Value = i;
            //    myProgressBar1.Value = i;
            //    label2.Text = "" + i;
            //    System.Threading.Thread.Sleep(100);
            //}
        }

        private void myProgressBar1_Load(object sender, EventArgs e)
        {

        }
    }
}
