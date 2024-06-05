using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;

namespace TMV.Common
{
    public static class FTPCommon
    {
        static ManualResetEvent m_reset = new ManualResetEvent(false);
        private static string SOURCE_FILE = string.Empty;
        static Exception e = null;
        static bool result = false;

        public static bool UploadFile(string source, string dest, string host, int port, string user, string pw)
        {
            using (FtpClient conn = new FtpClient())
            {
                conn.Host = host;
                conn.Port = port;
                SOURCE_FILE = source;

                conn.Credentials = new NetworkCredential(user, pw);
                
                IAsyncResult rs = conn.BeginOpenWrite(dest, new AsyncCallback(BeginOpenWriteCallback), conn);

                m_reset.WaitOne();
                conn.Disconnect();
            }
            return result;
        }

        static void BeginOpenWriteCallback(IAsyncResult ar)
        {
            FtpClient conn = ar.AsyncState as FtpClient;
            Stream istream = null, ostream = null;
            byte[] buf = new byte[8192];
            int read = 0;

            try
            {
                if (conn == null)
                    throw new InvalidOperationException("The FtpControlConnection object is null!");

                ostream = conn.EndOpenWrite(ar);
                istream = new FileStream(SOURCE_FILE, FileMode.Open, FileAccess.Read);

                while ((read = istream.Read(buf, 0, buf.Length)) > 0)
                {
                    ostream.Write(buf, 0, read);
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                FormGlobals.Message_Error(ex );
            }
            finally
            {
                if (istream != null)
                    istream.Close();

                if (ostream != null)
                    ostream.Close();

                m_reset.Set();
            }
        }

    }
}
