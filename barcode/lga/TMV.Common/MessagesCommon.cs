using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace TMV.Common
{
    public static class MessagesCommon
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Message_Error_Dialog(Exception ex)
        {
            // Display custom common error message to user
            Message_Error(ex, ex.Message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Message_WriteLog(Exception ex)
        {
            // Display custom common error message to user
            Log.WriteLog(Log.LogLevel.ERR, ex, "");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Message_WriteLog_Data(Exception ex, string data)
        {
            // Display custom common error message to user
            Log.WriteLog(Log.LogLevel.ERR, ex, data);
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Message_Info(string msgDisplay)
        {
            // Display custom common error message to user
            MessageBox.Show(
                msgDisplay,
                Application.ProductName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Message_Warning(string msgDisplay)
        {
            // Display custom common error message to user
            MessageBox.Show(
                msgDisplay,
                Application.ProductName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1);
        }
        /// <summary>
        /// Write log error and display custom message
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="msgDisplay"></param>
        public static void Message_Error(Exception ex, string msgDisplay)
        {
            // Write noi dung exception ra log file
            Log.WriteLog(Log.LogLevel.ERR, ex, "");

            // Display error message to user
            MessageBox.Show(
                msgDisplay,
                Application.ProductName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
        }

        public static DialogResult Message_Confirm(string msgDisplay)
        {
            return MessageBox.Show(
                msgDisplay,
                Application.ProductName,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1);
        }

        public static void Message_Information(string msgDisplay)
        {
            MessageBox.Show(
                msgDisplay,
                Application.ProductName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

    }
}
