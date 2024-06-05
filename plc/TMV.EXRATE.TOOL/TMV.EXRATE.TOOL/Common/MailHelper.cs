using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Xml.Linq;
using Microsoft.Office.Interop.Outlook;
using Org.BouncyCastle.Asn1.X509;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace TMV.EXRATE.TOOL
{
    public class MailHelper
    {

        private string _profileName;
        private Microsoft.Office.Interop.Outlook.NameSpace _oNS;
        private Microsoft.Office.Interop.Outlook.Application _oApp;


        public MailHelper(string profileName)
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

            
        public List<MailItem> GetEmailsByTitle(string title, string folder="Inbox")
        {
            MAPIFolder sourceFolder = _oNS.GetDefaultFolder(OlDefaultFolders.olFolderInbox).Parent.Folders[folder] ?? throw new System.Exception("Folder not exist in current profile.");

            //Get the Items collection in the Inbox folder.
            string filter = "@SQL=\"urn:schemas:httpmail:subject\" LIKE '%" + title + "%'";
            var oItems = sourceFolder.Items.Restrict(filter);
            if (oItems.Count == 0)
            {
                throw new System.Exception("No email item found with title: " + title);
            }

            var filterItems = new List<MailItem>();
            foreach (MailItem item in oItems)
            {
                if (IsValidEmail(item))
                {
                    filterItems.Add(item);
                }
            }

            if (filterItems.Count == 0)
            {
                throw new System.Exception("No email item found with title: " + title);
            }

            return filterItems;
        }

        public bool IsValidEmail(MailItem item)
        {
            bool result = false;
            string pattern = @"From: .+<(.*@.*)>";
            string mailBody = item.Body;

            MatchCollection matches = Regex.Matches(mailBody, pattern);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    if (groups.Count > 1)
                    {
                        if (groups[1].Value != AppConst.MAIL_SENDER)
                        {
                            result = false;
                        } else
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }

            return true;
            //return result;
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
                    if (!attachment.FileName.EndsWith(".pdf")) continue;
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

        public bool MoveMailToFolder(Microsoft.Office.Interop.Outlook.MailItem mailItem, string destFolder)
        {
            MAPIFolder destinationFolder = _oNS.GetDefaultFolder(OlDefaultFolders.olFolderInbox).Parent.Folders[destFolder] ?? throw new System.Exception("Folder not exist in current profile.");
            mailItem.Move(destinationFolder);
            return true;
        }

        public bool SendEmail(string receiver, string subject, string body, string[] attachmentPaths = null, string cc = null, string mailType="text")
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

        public bool SendEmailWithEmailAttachment(string receiver, string subject, string body, MailItem emailAttachment, string cc = null, string mailType = "text")
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
            }
            else
            {
                mailItem.Body = body;
            }

            AddMessageAsAttachment(mailItem, emailAttachment);

            mailItem.Send();
            return true;
        }
        private void AddMessageAsAttachment(Microsoft.Office.Interop.Outlook.MailItem mailContainer, Microsoft.Office.Interop.Outlook.MailItem mailToAttach)
        {
            Microsoft.Office.Interop.Outlook.Attachments attachments = null;
            Microsoft.Office.Interop.Outlook.Attachment attachment = null;
            try
            {
                attachments = mailContainer.Attachments;
                attachment = attachments.Add(mailToAttach,
                   Microsoft.Office.Interop.Outlook.OlAttachmentType.olEmbeddeditem, 1, "The attached e-mail");
                mailContainer.Save();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (attachment != null) Marshal.ReleaseComObject(attachment);
                if (attachments != null) Marshal.ReleaseComObject(attachments);
            }
        }

        public void Disconnect()
        {
            _oNS.Logoff();
        }
    }
}
