using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Controls;
using System.Xml.Linq;
using Microsoft.Office.Interop.Outlook;
using Org.BouncyCastle.Asn1.X509;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace RPA.EXRATE.TOOL
{
    public class OutlookClientHelper
    {

        private string _profileName;
        private Microsoft.Office.Interop.Outlook.NameSpace _oNS;
        private Microsoft.Office.Interop.Outlook.Application _oApp;


        public OutlookClientHelper(string profileName)
        {
            _profileName = profileName;
            _oApp = new Microsoft.Office.Interop.Outlook.Application();
            _oNS = _oApp.GetNamespace("mapi");
            _oNS.Logon(profileName, Missing.Value, false, true);
        }

        private MAPIFolder GetFolder(string folderPath, Folders folders)
        {
            string dir = folderPath.Substring(0, folderPath.Substring(4).IndexOf("\\") + 4);
            try
            {
                foreach (MAPIFolder mf in folders)
                {
                    if (!(mf.FullFolderPath.StartsWith(dir))) continue;
                    if (mf.FullFolderPath == folderPath) return mf;
                    else
                    {
                        MAPIFolder temp = GetFolder(folderPath, mf.Folders);
                        if (temp != null)
                            return temp;
                    }
                }
                return null;
            }
            catch { return null; }
        }

            
        public Items GetEmailsByTitle(string title, string folder="Inbox")
        {
            MAPIFolder sourceFolder = _oNS.GetDefaultFolder(OlDefaultFolders.olFolderInbox).Parent.Folders[folder] ?? throw new System.Exception("Folder not exist in current profile.");

            //Get the Items collection in the Inbox folder.
            string filter = "@SQL=\"urn:schemas:httpmail:subject\" LIKE '%" + title + "%'";
            var oItems = sourceFolder.Items.Restrict(filter);
            if (oItems.Count == 0)
            {
                throw new System.Exception("No email item found with title: " + title);
            }
            return oItems;
        }


        public List<string> GetAttachmentsByEmail(Microsoft.Office.Interop.Outlook.MailItem mailItem, string path)
        {
            List<string> result = new List<string>();
            if (mailItem.Attachments.Count > 0)
            {
                Console.WriteLine("Attachments:");

                // Loop through the attachments
                for (int i = 1; i <= mailItem.Attachments.Count; i++)
                {
                    Microsoft.Office.Interop.Outlook.Attachment attachment = mailItem.Attachments[i];

                    // Save the attachment to a specified location

                    var folder = path + DateTime.Now.ToString("MM_dd_yyyy") + "\\";
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    //
                    var fullPath = folder + "ExchangeRate_" + mailItem.ConversationID + ".pdf";
                    attachment.SaveAsFile(fullPath);
                    result.Add(fullPath);
                }
            } else
            {
                throw new System.Exception("This email don\'t have attachment");
            }

            return result;
        }


        public Boolean MoveMailToFolder(Microsoft.Office.Interop.Outlook.MailItem mailItem, string destFolder)
        {
            MAPIFolder destinationFolder = _oNS.GetDefaultFolder(OlDefaultFolders.olFolderInbox).Parent.Folders[destFolder] ?? throw new System.Exception("Folder not exist in current profile.");
            mailItem.Move(destinationFolder);
            return true;
        }

        public Boolean SendEmail(string receiver, string subject, string body, string[] attachmentPaths = null, string cc = null, string mailType="text")
        {
            Microsoft.Office.Interop.Outlook.MailItem mailItem = (Microsoft.Office.Interop.Outlook.MailItem)_oApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            mailItem.To = receiver;
            if (cc != null)
            {
                mailItem.CC = cc;
            }

            mailItem.Subject = subject;

            if (mailType == "html")
            {
                mailItem.HTMLBody = body;
            } else
            {
                mailItem.Body = body;
            }
           
            

            if (attachmentPaths != null)
            {
                foreach (string attachmentPath in attachmentPaths)
                {
                    mailItem.Attachments.Add(attachmentPath);  
                }
            }

            mailItem.Send();
            return true;
        }

        public void disconnect()
        {
            _oNS.Logoff();
        }
    }
}
