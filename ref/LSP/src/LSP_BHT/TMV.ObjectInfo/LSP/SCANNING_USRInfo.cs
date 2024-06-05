using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMV.ObjectInfo
{

    public class SCANNING_USRInfo
    {
        #region "Property"
        public int ID { get; set; }
        public string USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string IS_ACTIVE { get; set; }
        public int PROCESS_ID { get; set; }
        public string PROCESS_NAME { get; set; }
        #endregion

        #region "Constants"
        public static readonly string ID_COL = "ID";
        public static readonly string USER_ID_COL = "USER_ID";
        public static readonly string USER_NAME_COL = "USER_NAME";
        public static readonly string IPROCESS_ID_COL = "PROCESS_ID";
        public static readonly string PROCESS_NAME_COL = "PROCESS_NAME";
        #endregion

        #region "Init Class"
        public SCANNING_USRInfo()
        {
        }
        public SCANNING_USRInfo(int ID, string USER_ID, string USER_NAME, string IS_ACTIVE, int PROCESS_ID, string PROCESS_NAME)
        {
            this.ID = ID;
            this.USER_ID = USER_ID;
            this.USER_NAME = USER_NAME;
            this.IS_ACTIVE = IS_ACTIVE;
            this.PROCESS_ID = PROCESS_ID;
            this.PROCESS_NAME = PROCESS_NAME;

        }
        #endregion
    }
}