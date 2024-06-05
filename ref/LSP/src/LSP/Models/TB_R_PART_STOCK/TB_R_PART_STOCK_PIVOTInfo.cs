using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models.TB_R_PART_STOCK
{
    public class TB_R_PART_STOCK_PIVOTInfo
    {
        #region "Public Members"
        public int ID { get; set; }
        public int ROW_NO { get; set; }
        public string SUPPLIER_CODE { get; set; }        
        public string PART_NO { get; set; }       
        public string COLOR_SFX { get; set; }
        public string PART_NAME { get; set; }        
           
        public string DAY_1 { get; set; }
        public string DAY_2 { get; set; }
        public string DAY_3 { get; set; }
        public string DAY_4 { get; set; }
        public string DAY_5 { get; set; }
        public string DAY_6 { get; set; }
        public string DAY_7 { get; set; }
        public string DAY_8 { get; set; }
        public string DAY_9 { get; set; }
        public string DAY_10 { get; set; }
        public string DAY_11 { get; set; }
        public string DAY_12 { get; set; }
        public string DAY_13 { get; set; }
        public string DAY_14 { get; set; }
        public string DAY_15 { get; set; }
        public string DAY_16 { get; set; }
        public string DAY_17 { get; set; }
        public string DAY_18 { get; set; }
        public string DAY_19 { get; set; }
        public string DAY_20 { get; set; }
        public string DAY_21 { get; set; }
        public string DAY_22 { get; set; }
        public string DAY_23 { get; set; }
        public string DAY_24 { get; set; }
        public string DAY_25 { get; set; }
        public string DAY_26 { get; set; }
        public string DAY_27 { get; set; }
        public string DAY_28 { get; set; }
        public string DAY_29 { get; set; }
        public string DAY_30 { get; set; }
        public string DAY_31 { get; set; }

        public int DAY_1_INT 
        {
            get{try{return Convert.ToInt32(DAY_1);}catch (Exception ex){return 0;}}
        }

        public int DAY_2_INT
        {
            get { try { return Convert.ToInt32(DAY_2); } catch (Exception ex) { return 0; } }
        }

        public int DAY_3_INT
        {
            get { try { return Convert.ToInt32(DAY_3); } catch (Exception ex) { return 0; } }
        }

        public int DAY_4_INT
        {
            get { try { return Convert.ToInt32(DAY_4); } catch (Exception ex) { return 0; } }
        }

        public int DAY_5_INT
        {
            get { try { return Convert.ToInt32(DAY_5); } catch (Exception ex) { return 0; } }
        }

        public int DAY_6_INT
        {
            get { try { return Convert.ToInt32(DAY_6); } catch (Exception ex) { return 0; } }
        }

        public int DAY_7_INT
        {
            get { try { return Convert.ToInt32(DAY_7); } catch (Exception ex) { return 0; } }
        }

        public int DAY_8_INT
        {
            get { try { return Convert.ToInt32(DAY_8); } catch (Exception ex) { return 0; } }
        }

        public int DAY_9_INT
        {
            get { try { return Convert.ToInt32(DAY_9); } catch (Exception ex) { return 0; } }
        }

        public int DAY_10_INT
        {
            get { try { return Convert.ToInt32(DAY_10); } catch (Exception ex) { return 0; } }
        }

        public int DAY_11_INT
        {
            get { try { return Convert.ToInt32(DAY_11); } catch (Exception ex) { return 0; } }
        }

        public int DAY_12_INT
        {
            get { try { return Convert.ToInt32(DAY_12); } catch (Exception ex) { return 0; } }
        }

        public int DAY_13_INT
        {
            get { try { return Convert.ToInt32(DAY_13); } catch (Exception ex) { return 0; } }
        }

        public int DAY_14_INT
        {
            get { try { return Convert.ToInt32(DAY_14); } catch (Exception ex) { return 0; } }
        }

        public int DAY_15_INT
        {
            get { try { return Convert.ToInt32(DAY_15); } catch (Exception ex) { return 0; } }
        }
        public int DAY_16_INT
        {
            get { try { return Convert.ToInt32(DAY_16); } catch (Exception ex) { return 0; } }
        }
        public int DAY_17_INT
        {
            get { try { return Convert.ToInt32(DAY_17); } catch (Exception ex) { return 0; } }
        }

        public int DAY_18_INT
        {
            get { try { return Convert.ToInt32(DAY_18); } catch (Exception ex) { return 0; } }
        }
        public int DAY_19_INT
        {
            get { try { return Convert.ToInt32(DAY_19); } catch (Exception ex) { return 0; } }
        }

        public int DAY_20_INT
        {
            get { try { return Convert.ToInt32(DAY_20); } catch (Exception ex) { return 0; } }
        }

        public int DAY_21_INT
        {
            get { try { return Convert.ToInt32(DAY_21); } catch (Exception ex) { return 0; } }
        }

        public int DAY_22_INT
        {
            get { try { return Convert.ToInt32(DAY_22); } catch (Exception ex) { return 0; } }
        }

        public int DAY_23_INT
        {
            get { try { return Convert.ToInt32(DAY_23); } catch (Exception ex) { return 0; } }
        }

        public int DAY_24_INT
        {
            get { try { return Convert.ToInt32(DAY_24); } catch (Exception ex) { return 0; } }
        }

        public int DAY_25_INT
        {
            get { try { return Convert.ToInt32(DAY_25); } catch (Exception ex) { return 0; } }
        }

        public int DAY_26_INT
        {
            get { try { return Convert.ToInt32(DAY_26); } catch (Exception ex) { return 0; } }
        }

        public int DAY_27_INT
        {
            get { try { return Convert.ToInt32(DAY_27); } catch (Exception ex) { return 0; } }
        }

        public int DAY_28_INT
        {
            get { try { return Convert.ToInt32(DAY_28); } catch (Exception ex) { return 0; } }
        }

        public int DAY_29_INT
        {
            get { try { return Convert.ToInt32(DAY_29); } catch (Exception ex) { return 0; } }
        }
        public int DAY_30_INT
        {
            get { try { return Convert.ToInt32(DAY_30); } catch (Exception ex) { return 0; } }
        }

        public int DAY_31_INT
        {
            get { try { return Convert.ToInt32(DAY_31); } catch (Exception ex) { return 0; } }
        }

        public int STD_MIN_STOCK { get; set; }
        public int STD_MAX_STOCK { get; set; }

        public DateTime? STOCK_MONTH { get; set; }
        public string STOCK_MONTH_Str_DDMMYYYY 
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", STOCK_MONTH);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        public string STOCK_MONTH_Str_MMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:MM/yyyy}", STOCK_MONTH);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
                
        #endregion

         
    }
}