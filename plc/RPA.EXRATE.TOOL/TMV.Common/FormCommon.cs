using System;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Threading;

namespace TMV.Common
{
    public sealed class FormGlobals
    {
        #region "Variables"

        public static string CS_FONT_NAME;
        private const string ERROR_REFERENCES_CONSTRAINT = "ORA-02292: integrity constraint";
        private const string ERROR_TNS_TIME_OUT = "ORA-12170: TNS:Connect timeout occurred";
        private const string ERROR_UNIQUE_CONSTRAINT = "ORA-00001: unique constraint";
        private const string ERROR_VALUE_TOO_LARGE = "ORA-12899: value too large for column";
        public static GetDataError_Invoker GetDataError_Function;
        public static GetField_Invoker GetField_Function;

        public delegate DataTable GetDataError_Invoker(string sErrType, string sErrObject);
        public delegate DataTable GetField_Invoker(string sTable);

        #endregion

        private struct DataErrorObject
        {
            public string ErrorType;
            public string ErrorObjectName;
            public string ErrorMessage;
            public bool GetFromDB;
        }

        #region "Message"
        public static void Message_Error(Exception ex, string sDesc)
        {
            string sMessage;
            if (sDesc != "")
                sMessage = "Description: " + sDesc + "\rMessage: ";
            else
                sMessage = "";

            MessageBox.Show((sMessage + ex.Message.Replace("\r\n", "\r ")) + "\r\rError Trace: " +
                             ex.StackTrace, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void Message_Error(Exception ex)
        {
            string sDesc = ex.Message;
            string sErrMessage = "";
            sErrMessage = GetDataError(sDesc);
            if (sErrMessage != "")
                MessageBox.Show(sErrMessage, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (sDesc.IndexOf("Could not find file") >= 0)
                Message_Information(sDesc);
            else if (sDesc.IndexOf("20000: Period is close or not exists") >= 0)
                Message_Information("Period is closed or not exist. " + Environment.NewLine +
                                    "You have to create period or re-open period by function: Master Data/TMV/Period Control ");
            else if (sDesc.IndexOf("ORA-20000: Data has changed since you retrieved it.") >= 0)
                Message_Information("Data has changed since you retrieved it. " + Environment.NewLine +
                                    "Could you please re-load and re-update. ");
            else if ((sDesc.IndexOf("The process cannot access the file") >= 0) & (sDesc.IndexOf("because it is being used by another process") > 0))
                Message_Information(sDesc);
            else
                Message_Error(ex, "");
        }

        public static bool Message_Confirm(string sMessage, bool bShowRetry)
        {
            if (bShowRetry)
                return (MessageBox.Show(sMessage, Application.ProductName, MessageBoxButtons.RetryCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Retry);

            return (MessageBox.Show(sMessage, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes);
        }

        public static bool Message_Delete(string objType, string sObjName)
        {
            return (MessageBox.Show("Are you sure you want to delete " + objType +
                                    ((sObjName == "") ? "" : " '" + sObjName + "'"), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
        }

        public static void Message_Information(string sMessage)
        {
            MessageBox.Show(sMessage, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
        }

        public static void Message_Warning(string sMessage)
        {
            MessageBox.Show(sMessage, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void Message_Warning_Error(Exception ex)
        {
            MessageBox.Show(ex.Message.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
        }

        public static void Message_Warning_Error(string sMessage)
        {
            MessageBox.Show(sMessage, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
        }

        public static bool Message_YesNo_Cancel(string sMessage)
        {
            return (MessageBox.Show(sMessage, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes);
        }

        public static DialogResult Message_YesNoCancel(string sMessage)
        {
            return MessageBox.Show(sMessage, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
        }

        private static DataErrorObject GetErrorObject(string sMsg)
        {
            DataErrorObject oRet = new DataErrorObject();
            int iBegin = 0;
            int iEnd = 0;
            int iLen = 0;
            string sObjName = "";

            if (sMsg.IndexOf("ORA-02292: integrity constraint") >= 0)
            {
                iBegin = sMsg.IndexOf("(");
                iBegin = sMsg.IndexOf(".", iBegin) + 1;
                iEnd = sMsg.IndexOf(")", iBegin);
                sObjName = sMsg.Substring(iBegin, iEnd - iBegin);
                oRet.ErrorType = "R";
                oRet.ErrorObjectName = sObjName;
                oRet.ErrorMessage = "";
                oRet.GetFromDB = true;
                return oRet;
            }
            if (sMsg.IndexOf("ORA-00001: unique constraint") >= 0)
            {
                iBegin = sMsg.IndexOf("(");
                iBegin = sMsg.IndexOf(".", iBegin) + 1;
                iEnd = sMsg.IndexOf(")", iBegin);
                sObjName = sMsg.Substring(iBegin, iEnd - iBegin);
                oRet.ErrorType = "U";
                oRet.ErrorObjectName = sObjName;
                oRet.ErrorMessage = "";
                oRet.GetFromDB = true;
                return oRet;
            }
            if (sMsg.IndexOf("ORA-12899: value too large for column") >= 0)
            {
                iBegin = sMsg.IndexOf(".");
                iBegin = sMsg.IndexOf(".", (int)(iBegin + 1)) + 2;
                iEnd = sMsg.IndexOf("\" (", iBegin);
                iLen = sMsg.IndexOf(")", iEnd);
                sObjName = sMsg.Substring(iBegin, iEnd - iBegin);
                oRet.ErrorMessage = sMsg.Substring(iEnd + 1, iLen - iEnd);
                oRet.ErrorType = "L";
                oRet.ErrorObjectName = "";
                oRet.ErrorMessage = "Value too large for column '" + sObjName + "' -" + oRet.ErrorMessage;
                oRet.GetFromDB = false;
                return oRet;
            }
            if (sMsg.IndexOf("ORA-12170: TNS:Connect timeout occurred") >= 0)
            {
                oRet.ErrorType = "L";
                oRet.ErrorObjectName = "";
                oRet.ErrorMessage = "Having problem with Network or Oracle connection. Please contact TMV IT";
                oRet.GetFromDB = false;
            }
            return oRet;
        }

        private static string GetDataError(string sMsg)
        {
            string sRet = "";
            DataErrorObject oErr = new DataErrorObject();
            DataTable dt = null;
            oErr = GetErrorObject(sMsg);

            try
            {
                if (!((oErr.ErrorType != null) & (oErr.ErrorObjectName != null)))
                    return sRet;

                if (oErr.GetFromDB)
                {
                    dt = GetDataError_Function(oErr.ErrorType, oErr.ErrorObjectName);
                    if ((dt != null) && (dt.Rows.Count > 0))
                        sRet = dt.Rows[0][0].ToString();

                    return sRet;
                }
                if (oErr.ErrorMessage != "")
                    sRet = oErr.ErrorMessage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return sRet;
        }

        #endregion
    }
}
