using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMV.EXRATE.TOOL
{
    public static class AppConst
    {
        public const string XCHECK_STORE = "MST_CMM_EXCHANGE_RATE_XCHECK";
        public const string CHECK_EXIST_STORE = "MST_CMM_EXCHANGE_RATE_CHECK_EXIST";

        public const string MAIL_SENDER = "citivnfxcounterratenoreply@citi.com";

        public const string EMAIL_LOGGING = "Login to mail account.";
        public const string EMAIL_LOGGING_SUCCESS = "Login to email successfully";
        public const string EMAIL_LOGGING_FAILURE = "Login failed. ";

        public const string EMAIL_GETTING = "Getting email list";
        public const string EMAIL_GETTING_SUCCESS = "Get email list success";
        public const string EMAIL_GETTING_FAILTURE = "Get email list failed. ";

        public const string EMAIL_ATTACHMENT_SAVING = "Saving attachment from email. ";
        public const string EMAIL_ATTACHMENT_SAVING_SUCESS = "Save attachment successfully";
        public const string EMAIL_ATTACHMENT_SAVING_FAILURE = "Save attachment failed. ";

        public const string PDF_PARSING = "Parsing PDF Attachment";
        public const string PDF_PARSING_SUCCESS = "Parsing PDF successfully";
        public const string PDF_PARSING_FAILTURE = "Parsing PDF failed. ";


        public const string TABLE_GETTING = "Getting table";
        public const string TABLE_GETTING_SUCCESS = "Get table successfully";
        public const string TABLE_GETTING_FAILTURE = "Get table failed. ";


        public const string DB_INSERTING = "Inserting data to db";
        public const string DB_INSERTING_SUCCESS = "Insert data successfully";
        public const string DB_INSERTING_FAILTURE = "Insert data failed. ";

        public const string DB_STORE_CALLING = "Calling store";
        public const string DB_STORE_CALLING_SUCCESS = "Call store successfully";
        public const string DB_STORE_CALLING_FAILTURE = "Call store failed. ";

        public const string EMAIL_SENDING = "Sending Email ";
        public const string EMAIL_SENDING_SUCCESS = "Sent email successfully";
        public const string EMAIL_SENDING_FAILTURE = "Send email failed";


        public const string EXRATE_EXIST = "ExchangeRate datetime and version is already exists in database.";
        public const string FILE_EXIST = "No new email found. Skipping";

        public const string ADMIN_EMAIL_SENDING_TITLE = "[TMV.EXRATE.TOOL] Ran successfully";
    }
}
