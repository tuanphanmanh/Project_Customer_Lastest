
using System;
using System.Runtime.Serialization;
//using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
//using NPOI.XSSF.UserModel;

namespace esign
{
   
    public class CommonFunction 
    {
        public CommonFunction() { }

        public int TryParseInt(object _value, int _defaultValue)
        {
            int _val = _defaultValue;
            if (_value == null || _value == "")
            {
                return _defaultValue;
            }
            else
            {
                try
                {
                    _val = int.Parse(_value.ToString());
                }
                catch (Exception e)
                {
                    return _defaultValue;
                }
            }

            return _val;
        }

        public int TryParseInt(ICell _value, int _defaultValue)
        {
            int _val = _defaultValue;
            try
            {
                _val = TryParseInt(_value.NumericCellValue.ToString(), 0);

            }catch (Exception e)
            {
                _val = TryParseInt(_value.ToString(), 0);
            }
             
            return _val;
        }
    }
}
