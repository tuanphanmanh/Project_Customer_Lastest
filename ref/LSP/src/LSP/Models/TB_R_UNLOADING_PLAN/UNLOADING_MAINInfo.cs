using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_R_UNLOADING_PLAN
{ 
    public class UNLOADING_MAINInfo
	{
		#region "Public Members"
		public long ID { get; set; }
        public string ROW_NO { get; set; }
		public string DOCK { get; set; }
		public string TRUCK { get; set; }
		public String SUPPLIERS { get; set; }
        //public string WORKING_DATE { get; set; }
        //public string SHIFT { get; set; }
		public short SEQUENCE_NO { get; set; }
        public string ANDON_NO { get; set; }

		public DateTime? PLAN_START_UP_DATETIME { get; set; }
        public string PLAN_START_UP_DATETIME_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm}", PLAN_START_UP_DATETIME);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }

		public DateTime? PLAN_FINISH_UP_DATETIME { get; set; }
        public string PLAN_FINISH_UP_DATETIME_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm}", PLAN_FINISH_UP_DATETIME);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
		 
		public DateTime? ACTUAL_START_UP_DATETIME { get; set; }
		public DateTime? ACTUAL_FINISH_UP_DATETIME { get; set; }
		 
		public DateTime? REVISED_PLAN_START_UP_DATETIME { get; set; }
        public string REVISED_PLAN_START_UP_DATETIME_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm}", REVISED_PLAN_START_UP_DATETIME);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }

		public DateTime? REVISED_PLAN_FINISH_UP_DATETIME { get; set; }
        public string REVISED_PLAN_FINISH_UP_DATETIME_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm}", REVISED_PLAN_FINISH_UP_DATETIME);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
		 
		public int ACTUAL_START_UP_DELAY { get; set; }
		public int ACTUAL_FINISH_UP_DELAY { get; set; }
		public string STATUS { get; set; }
		public String ISSUES { get; set; }
		public String CAUSE { get; set; }
		public String COUTERMEASURE { get; set; }
		public String PIC_RECORDER { get; set; }
		public String PIC_ACTION { get; set; }
		public DateTime? ACTION_DUEDATE { get; set; }
		 
		public short RESULT { get; set; }  

        public float NOW_DIFF { get; set; }
        public float PLAN_START_DIFF { get; set; }
        public float PLAN_FINISH_DIFF { get; set; }
        public float ACTUAL_START_DIFF { get; set; }
        public float ACTUAL_FINISH_DIFF { get; set; }
        public float REVISED_START_DIFF { get; set; }
        public float REVISED_FINISH_DIFF { get; set; }
        public string IS_CURRENT { get; set; }

        public string IS_WARNING_DELAY { get; set; }

		#endregion

		 
    }
}


