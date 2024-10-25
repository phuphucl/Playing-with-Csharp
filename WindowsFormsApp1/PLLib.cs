using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class PLLib
    {
        internal unsafe class Buffer
        {
            private byte[] _array;
            [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
            private static extern IntPtr Memcpy(void* dest, void* src, int count);
            public Buffer(int size = 1024)
            {
                if (size <= 0) size = 1024;
                _array = new byte[size];
            }
            //               Offset
            //01234567890123456789012345678901234567890123456789
            //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            public bool SetValue(int offset, void* pValue, int size)
            {
                LastError = null;
                if (pValue == null || size <= 0 || offset < 0 || offset + size >= _array.Length)
                {
                    LastError = new Exception("Offset or size or the set value address is invalid");
                    return false;
                }

                fixed (byte* pDest = _array)
                {
                    Memcpy(pDest + offset, (byte*)pValue, size);
                }
                return true;
            }
            //backward compatibility 
            // Rectangle: 20 x, y, w, h, color      
            // Rectangle: 24 x, y, w, h, color, pw

            //byte, sbyte, char, short, ushort, int, uint, long, ulong, decimal, double, float

            // ******** SET FUNCTIONS ********
            public bool SetValue(int offset, int value)
            {
                return SetValue(ref offset, value);
            }
            public bool SetValue(ref int offset, int value)
            {
                LastError = null;
                if (SetValue(offset, &value, sizeof(int)))
                {
                    offset += sizeof(int);
                    return true;
                }
                return false;
            }

            public bool SetValue(int offset, uint value)
            {
                return SetValue(offset, &value, sizeof(uint));
            }

            public bool SetValue(int offset, short value)
            {
                return SetValue(offset, &value, sizeof(short));
            }

            public bool SetValue(int offset, ushort value)
            {
                return SetValue(offset, &value, sizeof(ushort));
            }

            public bool SetValue(int offset, byte value)
            {
                return SetValue(offset, &value, sizeof(byte));
            }

            public bool SetValue(int offset, sbyte value)
            {
                return SetValue(offset, &value, sizeof(sbyte));
            }

            public bool SetValue(int offset, long value)
            {
                return SetValue(offset, &value, sizeof(long));
            }

            public bool SetValue(int offset, ulong value)
            {
                return SetValue(offset, &value, sizeof(ulong));
            }

            public bool SetValue(int offset, double value)
            {
                return SetValue(offset, &value, sizeof(double));
            }

            public bool SetValue(int offset, float value)
            {
                return SetValue(offset, &value, sizeof(float));
            }

            public bool SetValue(int offset, DateTime value)
            {
                long tick = value.Ticks;
                return SetValue(offset, &tick, sizeof(long));
            }
            //ASCII
            public bool SetValue(int offset, char value)
            {
                return SetValue(offset, &value, sizeof(char));
            }
            //0123456789
            //P0h0u0c0
            public bool SetValue(int offset, byte[] array)
            {
                if (array == null) return false;
                fixed (byte* pByte = array)
                {
                    return SetValue(offset, pByte, sizeof(byte) * array.Length);
                }
            }
            //intlongintLengthStringALengthbyte[]string
            public bool SetValue(int offset, string value, bool isUnicode = true)
            {
                int length = value.Length;

                if (isUnicode)
                {
                    fixed (char* ptr = value)
                    {
                        return SetValue(offset, ptr, sizeof(char) * length);
                    }
                }
                else
                {
                    byte[] array = System.Text.Encoding.ASCII.GetBytes(value);
                    return SetValue(offset, array);
                }
            }
            //oooooooooooooooooooooooooooooooo bbbbbbbb ccccccc bbbbbbb oooooooo xxxxxxxxxxxxxxxxxxxxxxxxxxxx
            // ******** GET FUNCTIONS ********
            public bool GetValue(int offset, void* retValue, int size)
            {
                LastError = null;
                if (size > 0 && offset > 0 && offset + size <= _array.Length)
                {
                    fixed (byte* pSrc = _array)
                    {
                        Memcpy(retValue, pSrc + offset, size);
                        return true;
                    }
                }
                LastError = new Exception("offset or size is invalid");
                return false;
            }

            public int GetByte(int offset, byte defaultValue = default)
            {
                GetValue(offset, &defaultValue, sizeof(byte));
                return defaultValue;
            }

            public int GetSByte(int offset, sbyte defaultValue = default)
            {
                GetValue(offset, &defaultValue, sizeof(sbyte));
                return defaultValue;
            }

            public char GetChar(int offset, char defaultValue = default)
            {
                GetValue(offset, &defaultValue, sizeof(char));
                return defaultValue;
            }

            public short GetShort(int offset, short defaultValue = default)
            {
                GetValue(offset, &defaultValue, sizeof(short));
                return defaultValue;
            }

            public ushort GetUShort(int offset, ushort defaultValue = default)
            {
                GetValue(offset, &defaultValue, sizeof(ushort));
                return defaultValue;
            }

            public int GetInt(int offset, int defaultValue = default)
            {
                GetValue(offset, &defaultValue, sizeof(int));
                return defaultValue;
            }

            public uint GetUInt(int offset, uint defaultValue = default)
            {
                GetValue(offset, &defaultValue, sizeof(uint));
                return defaultValue;
            }

            public long GetLong(int offset, long defaultValue = default)
            {
                GetValue(offset, &defaultValue, sizeof(long));
                return defaultValue;
            }

            public ulong GetULong(int offset, ulong defaultValue = default)
            {
                GetValue(offset, &defaultValue, sizeof(ulong));
                return defaultValue;
            }

            public double GetDouble(int offset, double defaultValue = default)
            {
                GetValue(offset, &defaultValue, sizeof(double));
                return defaultValue;
            }

            public float GetFloat(int offset, float defaultValue = default)
            {
                GetValue(offset, &defaultValue, sizeof(float));
                return defaultValue;
            }
            //PLLib

            public DateTime GetDateTime(int offset, DateTime defaultValue = default)
            {
                long ticks = 0;
                if (!GetValue(offset, &ticks, sizeof(long))) return defaultValue;
                DateTime retDateTime = new DateTime(ticks);
                return retDateTime;
            }
            public Exception LastError { get; private set; }
            public byte[] GetByteArray(int offset, int length, byte[] defaultValue = null)
            {
                LastError = null;
                if (length <= 0)
                {
                    LastError = new Exception("Length is zero or negative");
                    return defaultValue;
                }
                byte[] array = new byte[length];
                fixed (byte* ptr = array)
                {
                    if (!GetValue(offset, ptr, length)) return null;
                }
                return array;
            }

            public string GetString(int offset, int length, bool isUnicode, string defaultValue = default)
            {
                LastError = null;
                if (length <= 0)
                {
                    LastError = new Exception("Length is zero or negative");
                    return defaultValue;
                }
                if (isUnicode)
                {
                    char[] chars = new char[length];
                    fixed (char* pChar = chars)
                    {
                        if (!GetValue(offset, pChar, sizeof(char) * length)) return defaultValue;
                        return new string(pChar);
                    }
                }
                else
                {
                    byte[] array = GetByteArray(offset, length);
                    return System.Text.Encoding.ASCII.GetString(array);
                }

            }
        }
    }

    //byte sbyte: 8bit
    //short ushort char: 16bit, 2byte
    //int uint: 32bit, 4byte
    //long ulong: 64bit, 8byte
    //float: 32bit
    //double: 64bit
    //decimal: 128

    //DateTime: 64bit

    public struct MyStruct1
    {
        public byte B1;
        public byte B2;
        public int I;
    }
    public struct MyStruct2
    {
        public int I;
        public byte B1;
        public byte B2;

        public override string ToString()
        {
            return "This is myStruct2";
        }

    }
    public struct MyStruct3
    {
        public byte B1;
        public int I;
        public byte B2;

        public MyStruct3(byte b1, int i, byte b2)
        {
            B1 = b1; 
            I = i; 
            B2 = b2; 
        }
    }
    public class MyClass3
    {
        public byte B1;
        public int I;
        public byte B2;

        public static implicit operator int(MyClass3 c)
        {
            return c.I;
        }

        public static implicit operator MyClass3(int value)
        {
            MyClass3 c = new MyClass3();
            c.I = value;
            return c;
        }
        //ARGB
        public static implicit operator MyClass3(string value) 
        {
            MyClass3 c = new MyClass3();
            string[] s = value.Split(','); 
            if (s.Length > 0 && byte.TryParse(s[0].Trim(), out byte result1)) c.B1 = result1;
            if (s.Length > 1 && int.TryParse(s[1].Trim(), out int result2)) c.I = result2;
            if (s.Length > 2 && byte.TryParse(s[2].Trim(), out byte result3)) c.B2 = result3;
            return c;
        }

        public static MyClass3 operator +(MyClass3 c1, MyClass3 c2)
        {
            MyClass3 c = new MyClass3();
            c.I = c1.I + c2.I;
            c.B1 = (byte) (c1.B1 + c2.B1);
            c.B2 = (byte) (c1.B2 + c2.B2);
            return c;
        }

        public static bool operator == (MyClass3 c1, MyClass3 c2)
        {
            return (c1.I == c2.I);
        }
        public static bool operator !=(MyClass3 c1, MyClass3 c2)
        {
            return (c1.I != c2.I);
        }
    }
}

