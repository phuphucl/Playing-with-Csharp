using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal unsafe static class Extension
    {
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr Memcpy(void* dest, void* src, int count);
        public unsafe static byte[] FromInt(int number)
        {
            byte[] res = new byte[sizeof(int)];
            //byte* pBuffer = (byte*) &number;
            //res[0] = pBuffer[0];
            //res[1] = pBuffer[1];
            //res[2] = pBuffer[2];
            //res[3] = pBuffer[3];
            fixed (byte* pDest = res)
            {
                Memcpy(pDest, (void*)&number, sizeof(int));
            }
            return res;
        }
        public unsafe static int ToInt(byte[] array)
        {
            //int res = 0;
            //res = array[0] | array[1] << 8 | array[2] << 16 | array[3] << 24;
            //return res;
            int res = 0;
            fixed (byte* pSrc = array)
            {
                Memcpy(&res, pSrc, sizeof(int));
            }
             return res;
        }

        public static int ToInt(this string text, int defaultValue = -99999)
        {
            if (int.TryParse(text, out int result))  return result;
            return defaultValue;
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
    }
}
