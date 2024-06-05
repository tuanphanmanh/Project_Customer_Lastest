using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.EXRATE.TOOL
{
    public static class AppConst
    {
        public const string XCHECK_STORE = "MST_CMM_EXCHANGE_RATE_XCHECK";
        public const string CHECK_EXIST_STORE = "MST_CMM_EXCHANGE_RATE_CHECK_EXIST";

        public const string EMAIL_LOGGING = "Login to mail account.";
        public const string EMAIL_LOGGING_SUCCESS = "Logged in successfully";
        public const string EMAIL_LOGGING_FAILURE = "Login failed. ";

        public const string EMAIL_GETTING = "Getting email list";
        public const string EMAIL_GETTING_SUCCESS = "Get email list success";
        public const string EMAIL_GETTING_FAILURE = "Get email list failed. ";

        public const string EMAIL_ATTACHMENT_SAVING = "Saving attachment from email. ";
        public const string EMAIL_ATTACHMENT_SAVING_SUCESS = "Saved attachment successfully";
        public const string EMAIL_ATTACHMENT_SAVING_FAILURE = "Save attachment failed. ";

        public const string PDF_DOWNLOADING = "Downloading PDF";
        public const string PDF_DOWNLOADING_SUCCESS = "PDF downloaded successfully";
        public const string PDF_DOWNLOADING_FAILURE = "Download PDF failed. ";

        public const string PDF_PARSING = "Parsing PDF File";
        public const string PDF_PARSING_SUCCESS = "Parsed PDF successfully";
        public const string PDF_PARSING_FAILURE = "Parse PDF failed. ";


        public const string TABLE_GETTING = "Getting table";
        public const string TABLE_GETTING_SUCCESS = "Get table successfully";
        public const string TABLE_GETTING_FAILURE = "Get table failed. ";


        public const string DB_INSERTING = "Inserting data to db";
        public const string DB_INSERTING_SUCCESS = "Insert data successfully";
        public const string DB_INSERTING_FAILURE = "Insert data failed. ";

        public const string DB_STORE_CALLING = "Calling store";
        public const string DB_STORE_CALLING_SUCCESS = "Call store successfully";
        public const string DB_STORE_CALLING_FAILURE = "Call store failed. ";

        public const string EMAIL_SENDING = "Sending Email ";
        public const string EMAIL_SENDING_SUCCESS = "Sent email successfully";
        public const string EMAIL_SENDING_FAILURE = "Send email failed. ";

        public const string FILE_EXIST = "No new email found. Skipping";
        public const string EXRATE_EXIST = "ExchangeRate datetime and version is already exists in database.";
        public const string NOT_IN_TIME = "The current time is not between the working time.";
    }
}
