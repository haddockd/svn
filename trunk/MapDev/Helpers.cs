﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace SharpOcarina
{
    public static class Helpers
    {
        public static uint ShiftL(uint v, int s, int w)
        {
            return (uint)(((uint)v & (((uint)0x01 << w) - 1)) << s);
        }

        public static uint ShiftR(uint v, int s, int w)
        {
            return (uint)(((uint)v >> s) & (((int)0x01 << w) - 1));
        }

        public static ushort Read16(List<byte> Data, int Offset)
        {
            return (ushort)(Data[Offset] << 8 | Data[Offset + 1]);
        }

        public static short Read16S(List<byte> Data, int Offset)
        {
            return (short)(Data[Offset] << 8 | Data[Offset + 1]);
        }

        public static void Append16(ref List<byte> Data, ushort Value)
        {
            AppendXX(ref Data, Value, 1);
        }

        public static void Append32(ref List<byte> Data, uint Value)
        {
            AppendXX(ref Data, Value, 3);
        }

        public static void Append48(ref List<byte> Data, ulong Value)
        {
            Data.Add((byte)(Value >> 16));
            Append16(ref Data, (ushort)(Value & 0xFFFF));
        }

        public static void Append64(ref List<byte> Data, ulong Value)
        {
            AppendXX(ref Data, Value, 7);
        }

        private static void AppendXX(ref List<byte> Data, ulong Value, int Shifts)
        {
            for (int i = Shifts; i >= 0; --i)
            {
                byte DataByte = (byte)((Value >> (i * 8)) & 0xFF);
                Data.Add(DataByte);
            }
        }

        public static void Insert16(ref List<byte> Data, int Offset, ushort Value)
        {
            InsertXX(ref Data, Offset, Value, 1);
        }

        public static void Insert32(ref List<byte> Data, int Offset, uint Value)
        {
            InsertXX(ref Data, Offset, Value, 3);
        }

        public static void Insert64(ref List<byte> Data, int Offset, ulong Value)
        {
            InsertXX(ref Data, Offset, Value, 7);
        }

        private static void InsertXX(ref List<byte> Data, int Offset, ulong Value, int Shifts)
        {
            for (int i = Shifts; i >= 0; --i)
            {
                byte DataByte = (byte)((Value >> (i * 8)) & 0xFF);
                Data.Insert(Offset++, DataByte);
            }
        }

        public static void Overwrite16(ref List<byte> Data, int Offset, ushort Value)
        {
            OverwriteXX(ref Data, Offset, Value, 1);
        }

        public static void Overwrite32(ref List<byte> Data, int Offset, uint Value)
        {
            OverwriteXX(ref Data, Offset, Value, 3);
        }

        public static void Overwrite64(ref List<byte> Data, int Offset, ulong Value)
        {
            OverwriteXX(ref Data, Offset, Value, 7);
        }

        private static void OverwriteXX(ref List<byte> Data, int Offset, ulong Value, int Shifts)
        {
            if (Offset >= Data.Count)
            {
                AppendXX(ref Data, Value, Shifts);
            }
            else
            {
                for (int i = Shifts; i >= 0; --i)
                {
                    byte DataByte = (byte)((Value >> (i * 8)) & 0xFF);
                    Data.RemoveAt(Offset);
                    Data.Insert(Offset++, DataByte);
                }
            }
        }

        public static void GenericInject(string Filename, int Offset, byte[] Data)
        {
            GenericInject(Filename, Offset, Data, Data.Length);
        }

        public static void GenericInject(string Filename, int Offset, byte[] Data, int Length)
        {
            BinaryWriter BW = new BinaryWriter(File.OpenWrite(Filename));
            BW.Seek(Offset, SeekOrigin.Begin);
            BW.Write(new byte[Length], 0, Length);
            BW.Seek(Offset, SeekOrigin.Begin);
            BW.Write(Data);
            BW.Close();
        }

        public static double Log2(double Number)
        {
            return Math.Log(Number) / Math.Log(2);
        }

        public static string MakeValidFileName(string name)
        {
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidReStr = string.Format(@"[{0}]+", invalidChars);
            return Regex.Replace(name, invalidReStr, "_");
        }
    }
}
