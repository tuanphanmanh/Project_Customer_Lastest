using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace LSP.Models.TB_M_CALENDAR
{
    public class TB_M_CALENDARInfo
    {
        #region "Public Members"
        public int ID { get; set; }
        public int ROW_NO { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public DateTime? WORKING_DATE { get; set; }
        public string WORKING_DATE_Str_DDMMYYYY 
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", WORKING_DATE);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }

        public string WORKING_DATE_Str_MMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:MM/yyyy}", WORKING_DATE);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }

        public string WORKING_TYPE { get; set; }
        public string WORKING_STATUS { get; set; }
        public string IS_ACTIVE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string CREATED_DATE_Str_DDMMYYYY 
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", CREATED_DATE);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public string UPDATED_DATE_Str_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", UPDATED_DATE);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        #endregion

        #region "Constructors"
        public TB_M_CALENDARInfo()
        {
            ID = 0;
            ROW_NO = 0;
            SUPPLIER_CODE = string.Empty;
            WORKING_DATE = null;
            WORKING_TYPE = string.Empty;
            WORKING_STATUS = string.Empty;
            IS_ACTIVE = string.Empty;
            CREATED_BY = string.Empty;
            CREATED_DATE = null;
            UPDATED_BY = string.Empty;
            UPDATED_DATE = null;
        }

        public TB_M_CALENDARInfo(int ID, int ROW_NO, string SUPPLIER_CODE, DateTime? WORKING_DATE, string WORKING_TYPE, string WORKING_STATUS, string IS_ACTIVE, string CREATED_BY, DateTime? CREATED_DATE, string UPDATED_BY, DateTime? UPDATED_DATE)
        {
            this.ID = ID;
            this.ROW_NO = ROW_NO;
            this.SUPPLIER_CODE = SUPPLIER_CODE;
            this.WORKING_DATE = WORKING_DATE;
            this.WORKING_TYPE = WORKING_TYPE;
            this.WORKING_STATUS = WORKING_STATUS;
            this.IS_ACTIVE = IS_ACTIVE;
            this.CREATED_BY = CREATED_BY;
            this.CREATED_DATE = CREATED_DATE;
            this.UPDATED_BY = UPDATED_BY;
            this.UPDATED_DATE = UPDATED_DATE;
        }
        #endregion
    }    
}