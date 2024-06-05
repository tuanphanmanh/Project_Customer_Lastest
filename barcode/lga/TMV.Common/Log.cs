using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using log4net;
using System.Reflection;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace TMV.Common
{

    public class Log
    {
        #region "Constant"

        #endregion

        public const string OBJECT_LOG_NULL = "Write log parameter: Object is null or empty!";
        public static readonly string hyphenSign = " - ";

        #region "ENUM declaration"

        /// <summary>
        /// Log level for writing file
        /// </summary>
        /// <remarks></remarks>
        public enum LogLevel
        {
            INFO,
            // Log information such as start/stop app/form
            DEBUG,
            // Log for debug applicate
            WARN,
            // Log for any warning message to user
            ERR,
            // Log error occur when execute application, such as lost db connection
            FATAL
            // Log for un-catched exceptions,
        }

        #endregion

        #region "Class variables"

        // Declare log object
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region "File logging methods"

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
            logMsg.Append(hyphenSign);
            logMsg.Append(Environment.UserName); // PC user account
            logMsg.Append(hyphenSign);
            logMsg.Append(Globals.LoginFullName); // Program user account
            logMsg.Append(hyphenSign);
            logMsg.Append(className); // Class name
            logMsg.Append(hyphenSign);
            logMsg.Append(methodName); // Method write log
            logMsg.Append(hyphenSign);
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
            log4net.ThreadContext.Properties["UserId"] = Globals.LoginUserID;
            // Check log type
            switch (logLevel)
            {
                case LogLevel.DEBUG:
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug(logMsg);
                    }
                    break;
                case LogLevel.ERR:
                    if (logger.IsErrorEnabled)
                    {
                        logger.Error(logMsg);
                    }
                    break;
                case LogLevel.FATAL:
                    if (logger.IsFatalEnabled)
                    {
                        logger.Fatal(logMsg);
                    }
                    break;
                case LogLevel.INFO:
                    if (logger.IsInfoEnabled)
                    {
                        logger.Info(logMsg);
                    }
                    break;
                case LogLevel.WARN:
                    if (logger.IsWarnEnabled)
                    {
                        logger.Warn(logMsg);
                    }
                    break;
            }
        }

        #endregion
    }
}
