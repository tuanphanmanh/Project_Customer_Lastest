using System;

namespace TMV.ObjectInfo
{
    public class APP_LogbookInfo
    {
        public string COMPUTER_NAME { get; set; }
        public string FORM_NAME { get; set; }
        public string LOG_ACTION { get; set; }
        public string LOG_DESCRIPTION { get; set; }
        public decimal LOG_ID { get; set; }
        public DateTime LOG_TIME { get; set; }
        public decimal USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string WINDOWS_USER { get; set; }

        public APP_LogbookInfo()
        {
        }

        public APP_LogbookInfo(decimal LOG_ID, string COMPUTER_NAME, string WINDOWS_USER, string LOG_ACTION, string LOG_DESCRIPTION, string FORM_NAME, decimal USER_ID, string USER_NAME, DateTime LOG_TIME)
        {
            this.LOG_ID = LOG_ID;
            this.COMPUTER_NAME = COMPUTER_NAME;
            this.WINDOWS_USER = WINDOWS_USER;
            this.LOG_ACTION = LOG_ACTION;
            this.LOG_DESCRIPTION = LOG_DESCRIPTION;
            this.FORM_NAME = FORM_NAME;
            this.USER_ID = USER_ID;
            this.USER_NAME = USER_NAME;
            this.LOG_TIME = LOG_TIME;
        }
    }
}
