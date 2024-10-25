using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{

    public partial class Frm_main : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        private bool _bProcess = false;

        public unsafe Frm_main()
        {
            //          pM1
            //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            //          12003ABC                     MyStruct1
            //          3ABC1200                     MyStruct2 
            //          10003ABC2000                 MyStruct3  B1 I B2 
            InitializeComponent();//C C++ sizeof(int) == operating system


            Text = "ProgressBar Demo " + (sizeof(IntPtr) == 8 ? "64 bit" : "32 bit");

            int sz1 = sizeof(MyStruct2);
            MyStruct3[] array = new MyStruct3[2];//Fragmentation
            array[0].B1 = 1;
            array[0].B2 = 2;
            array[0].I = 0x0C0B0A03;

            array[1].B1 = 4;
            array[1].B2 = 5;
            array[1].I = 0x0F0E0D06;

            MyClass3 c = new MyClass3();
            c.B1 = 9; c.I = 8976; c.B2 = 67;

            IntPtr ptr = c.GetPointer();
            byte* pM = (byte*)ptr;

            //MyStruct3 myStr = new MyStruct3();
            //MyStruct3* strPtr = &myStr;

            MyClass3[] arrC3 = new MyClass3[5]; //create 5 class c
            MyClass3 c0 = new MyClass3(); c0.I = 0;
            MyClass3 c1 = new MyClass3(); c1.I = 1;
            MyClass3 c2 = new MyClass3(); c2.I = 2;
            MyClass3 c3 = new MyClass3(); c3.I = 3;
            MyClass3 c4 = new MyClass3(); c4.I = 4;
            arrC3[0] = c0;
            arrC3[1] = c1;
            arrC3[2] = c2;
            arrC3[3] = c3;
            arrC3[4] = c4;

            MyClass3* pC3 = (MyClass3*)arrC3[0].GetPointer();
            

            //for (int i = 0; i < arrC3.Length; i++)
            //{
            //    fixed (MyClass3* pC3 = &arrC3[i])
            //    {
            //        Debug.Print("I[" + i + "] = " + pC3->I);
            //    }
            //}

            var t = array;

            int sz2 = sizeof(MyStruct2);
            int sz3 = sizeof(MyStruct3);

            //int t = (int)myProgressBar2.CallMethod("Abc", args);

            //object[] args = { 12, 45 };
            //myProgressBar2.CallMethod<int>("SetProperty", args);

            //                                      x
            int number = 0x23F21789;// xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

           //myProgressBar1.Get


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

        private void myProgressBar2_Load(object sender, EventArgs e)
        {

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left )
            {
                if (ModifierKeys == Keys.Control)
                {
                    ReleaseCapture();
                    SendMessage(panel1.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }

        }
    }
}
