using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Reflection;
using log4net;

namespace LSP
{
    public class Logging
    {
        public const string OBJECT_LOG_NULL = "Write log parameter: Object is null or empty!";
        public static readonly string dotSign = " :: ";

        /// <summary>
        /// Log level for writing file
        /// </summary>
        /// <remarks></remarks>
        public enum LogLevel
        {
            // Log information such as start/stop app/form
            INFO,
            // Log for debug applicate
            DEBUG,
            // Log for any warning message to user
            WARN,
            // Log error occur when execute application, such as lost db connection
            ERR,
            // Log for un-catched exceptions
            FATAL
        }

        // Declare log object
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLog(LogLevel logLevel, object objEx, string data)
        {
            // Check write log object
            if (objEx == null || objEx.ToString().Length == 0)
            {
                // Write log about null parameter to file
                WriteLogFile(logLevel, OBJECT_LOG_NULL);
            }
            else
            {
                string className = string.Empty;
                string methodName = string.Empty;
                var stackFrame = new StackFrame(1);

                if (stackFrame != null)
                {
                    methodName = stackFrame.GetMethod().Name;
                    className = stackFrame.GetMethod().DeclaringType.Name;
                }

                // Write log message
                WriteLogFile(logLevel, CreateLogContent(className, methodName, objEx.ToString() + '-' + data));
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLog(LogLevel logLevel, string logMessage)
        {
            string className = string.Empty;
            string methodName = string.Empty;
            var stackFrame = new StackFrame(1);

            if (stackFrame != null)
            {
                methodName = stackFrame.GetMethod().Name;
                className = stackFrame.GetMethod().DeclaringType.Name;
            }

            // Write log message
            WriteLogFile(logLevel, CreateLogContent(className, methodName, logMessage));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLog(LogLevel logLevel, string formName, string actionName, string logMessage)
        {
            string className = string.Empty;
            string methodName = string.Empty;
            var stackFrame = new StackFrame(1);

            if (stackFrame != null)
            {
                methodName = stackFrame.GetMethod().Name;
                className = stackFrame.GetMethod().DeclaringType.Name;
            }

            // Write log message
            WriteLogFile(logLevel, CreateLogContent(formName, actionName, logMessage));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLog(LogLevel logLevel, string formName, string actionName, object objEx)
        {
            // Check write log object
            if (objEx == null || objEx.ToString().Length == 0)
            {
                // Write log about null parameter to file
                WriteLogFile(logLevel, OBJECT_LOG_NULL);
                return;
            }

            string className = string.Empty;
            string methodName = string.Empty;
            var stackFrame = new StackFrame(1);

            if (stackFrame != null)
            {
                methodName = stackFrame.GetMethod().Name;
                className = stackFrame.GetMethod().DeclaringType.Name;
            }

            // Write log message
            WriteLogFile(logLevel, CreateLogContent(formName, actionName, objEx.ToString()));
        }

        public static string CreateLogContent(string className, string methodName, string logContent)
        {
            var logMsg = new StringBuilder();

            // Record user information
            logMsg.Append(Environment.MachineName); // PC name
            logMsg.Append(dotSign);
            logMsg.Append(Environment.UserName); // PC user account
            logMsg.Append(dotSign);
            logMsg.Append(className); // Class name
            logMsg.Append(dotSign);
            logMsg.Append(methodName); // Method write log
            logMsg.Append(dotSign);
            logMsg.Append(logContent); // Main content

            return logMsg.ToString();
        }

        /// <summary>
        /// Note: Do not use this method for write log from anywhere outsite Common.Log class
        /// </summary>
        /// <param name="logLevel">log level</param>
        /// <param name="logMsg">content write to file</param>
        public static void WriteLogFile(LogLevel logLevel, string logMsg)
        {
            // Check log type
            switch (logLevel)
            {
                case LogLevel.DEBUG:
                    if (logger.IsDebugEnabled)
                        logger.Debug(logMsg);

                    break;
                case LogLevel.ERR:
                    if (logger.IsErrorEnabled)
                        logger.Error(logMsg);

                    break;
                case LogLevel.FATAL:
                    if (logger.IsFatalEnabled)
                        logger.Fatal(logMsg);

                    break;
                case LogLevel.INFO:
                    if (logger.IsInfoEnabled)
                        logger.Info(logMsg);

                    break;
                case LogLevel.WARN:
                    if (logger.IsWarnEnabled)
                        logger.Warn(logMsg);

                    break;
            }
        }
    }
}