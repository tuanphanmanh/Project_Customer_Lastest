using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMV.ObjectInfo
{

    public class MstLgaBarUser
    {
        #region "Property"
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string IsActive { get; set; }
        public int ProcessId { get; set; }
        public string ProcessCode { get; set; }
        #endregion

        #region "Constants"
        public static readonly string ID_COL = "Id";
        public static readonly string USER_ID_COL = "UserId";
        public static readonly string USER_NAME_COL = "UserName";
        public static readonly string IPROCESS_ID_COL = "ProcessId";
        public static readonly string PROCESS_NAME_COL = "ProcessCode";
        #endregion

        #region "Init Class"
        public MstLgaBarUser()
        {
        }
        public MstLgaBarUser(int ID, string USER_ID, string USER_NAME, string IS_ACTIVE, int PROCESS_ID, string PROCESS_NAME)
        {
            this.Id = ID;
            this.UserId = USER_ID;
            this.UserName = USER_NAME;
            this.ProcessId = PROCESS_ID;
            this.ProcessCode = PROCESS_NAME;
            this.IsActive = IS_ACTIVE;

        }
        #endregion
    }
}