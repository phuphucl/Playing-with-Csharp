using System;
using System.Collections.Generic;
using System.Linq;
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

}

