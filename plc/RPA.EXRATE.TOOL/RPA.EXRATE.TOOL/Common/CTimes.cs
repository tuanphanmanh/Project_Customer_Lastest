using System;

namespace RPA.EXRATE.TOOL
{
    public class CTimes
    {
        public static decimal ConvertDatetimeToDecimal(decimal sYear, decimal sMonth, decimal sDay, decimal sHour, decimal sMinute, decimal sSecon)
        {
            string hour = "";
            string minute = "";
            string secon = "";
            string smonth = "";
            string sday = "";

            if (sHour < 10)
                hour = "0" + sHour;
            else
                hour = sHour.ToString();

            if (sMinute < 10)
                minute = "0" + sMinute;
            else
                minute = sMinute.ToString();

            if (sSecon < 10)
                secon = "0" + sSecon;
            else
                secon = sSecon.ToString();

            if (sMonth < 10)
                smonth = "0" + sMonth.ToString();
            else
                smonth = sMonth.ToString();

            if (sDay < 10)
                sday = "0" + sDay.ToString();
            else
                sday = sDay.ToString();

            return decimal.Parse(sYear.ToString() + smonth + sday + hour + minute + secon);
        }

        public static decimal ConvertTimeToDec(decimal sHour, decimal sMinute, decimal sSecon)
        {
            string hour = "";
            string minute = "";
            string secon = "";

            if (sHour < 10)
                hour = "0" + sHour;
            else
                hour = sHour.ToString();

            if (sMinute < 10)
                minute = "0" + sMinute;
            else
                minute = sMinute.ToString();

            if (sSecon < 10)
                secon = "0" + sSecon;
            else
                secon = sSecon.ToString();

            return decimal.Parse(hour + minute + secon);
        }
    }
}
