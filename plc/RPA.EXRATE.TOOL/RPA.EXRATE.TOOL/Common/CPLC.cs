using System;
using System.Text;
using System.Linq;

namespace RPA.EXRATE.TOOL
{
    public class CPLC
    {
        public static string DataPLC { get; set; }

        public static void CheckFCS(ref string cmd, ref string cksm)
        {
            int cks = 0;
            for (int i = 1; i < cmd.Length + 1; i++)
            {
                cks = cks ^ (Encoding.Default.GetBytes(cmd.Substring(i - 1, 1))[0]);
            }
            cksm = cks.ToString("X");
            if (cksm.Length < 2)
                cksm = "0" + cksm;

            //string cksm;
            //string cmd;
            //cmd = "@" + "00" + "WR03000000"; Clear
            //cmd = "@" + "00" + "WR03000001"; 
            //cmd = "@" + "00" + "WR03000003";
            //cmd = "@" + "00" + "WR03000007";
            //cmd = "@" + "00" + "WR0300000F";
            //cmd = "@" + "00" + "WR0300001F";
            //cmd = "@" + "00" + "WR0300003F";
            //cmd = "@" + "00" + "RR02000003"; Send
            //CheckFCS(ref cmd, ref cksm);
            //command_Renamed.Text = cmd + cksm + "*" + "\r";
            //MSComm1.Output = command_Renamed.Text;
        }

        public static void EndCode(ref string EC, ref string ECText)
        {
            switch (EC)
            {
                case "IC":
                    ECText = "Unkown Command";
                    break;
                case "00":
                    ECText = "Normal Completion";
                    break;
                case "13":
                    ECText = "FCS Error";
                    break;
                case "14":
                    ECText = "Format Error";
                    break;
                case "18":
                    ECText = "Frame Length Error";
                    break;
                case "01":
                    ECText = "Not Executable in Run Mode";
                    break;
                case "02":
                    ECText = "Not Executable in Monitor Mode";
                    break;
                case "23":
                    ECText = "User Memory Protected";
                    break;
                case "15":
                    ECText = "Entry Number Data Error";
                    break;
            }
        }

        public static string ConvertHexadecimalToBinary(string HexVal)
        {
            int i, Length;
            string hex2bin = "";
            Length = HexVal.Length;
            for (i = 0; i <= Length - 1; i++)
            {
                string j = HexToNo(HexVal.Substring(Length - i - 1, 1)).ToString();
                hex2bin = ConvertDecimalToBinary(ref j) + hex2bin;
            }
            return hex2bin;
        }

        public static string ConvertDecimalToBinary(ref string Value)
        {
            int[] BinVal = new int[1];
            int i = 0;
            int ret = 0;
            double temp;
            string Str = "";

            double iVal = Convert.ToDouble(Value);
            do
            {
                temp = iVal / 2.0;
                ret = Convert.ToString(temp).IndexOf('.') + 1;
                if (ret > 0)
                    temp = Convert.ToDouble(Convert.ToString(temp).Substring(0, ret - 1));

                ret = Convert.ToInt32(iVal % 2);
                Array.Resize(ref BinVal, i + 1);
                BinVal[i] = ret;
                i = i + 1;
                iVal = temp;
            }
            while (temp > 0.0);

            for (int j = BinVal.GetUpperBound(0); j >= 0; j -= 1)
            {
                Str = Str + Convert.ToString((int)BinVal[j]);
            }

            switch (Str.Length % 4)
            {
                case 1:
                    Str = "000" + Str;
                    break;
                case 2:
                    Str = "00" + Str;
                    break;
                case 3:
                    Str = "0" + Str;
                    break;
            }
            return Str;
        }

        public static int HexToNo(string i)
        {
            int hex = 0;
            switch (i)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    hex = Convert.ToInt16(i);
                    break;
                case "A":
                case "a":
                    hex = 10;
                    break;
                case "B":
                case "b":
                    hex = 11;
                    break;
                case "C":
                case "c":
                    hex = 12;
                    break;
                case "D":
                case "d":
                    hex = 13;
                    break;
                case "E":
                case "e":
                    hex = 14;
                    break;
                case "F":
                case "f":
                    hex = 15;
                    break;
            }
            return hex;
        }

        public static string Right(string str, int Length)
        {
            if (Length < 0)
                throw new ArgumentException();

            if ((Length == 0) || (str == null))
                return "";

            int length = str.Length;
            if (Length >= length)
                return str;

            return str.Substring(length - Length, Length);
        }
    }
}
