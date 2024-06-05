using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LSP.Models.TB_M_LOOKUP
{
    public class TB_M_LOOKUPInfo
    {
        #region "public Members"
        public int ID { get; set; }
        public int ROW_NO { get; set; }
        [Required(ErrorMessage = "Field is required")]
        [StringLength(40, ErrorMessage = "Must be under 40 characters")]
        public string DOMAIN_CODE { get; set; }
        [Required(ErrorMessage = "Field is required")]
        [StringLength(40, ErrorMessage = "Must be under 40 characters")]
        public String ITEM_CODE { get; set; }
        [Required(ErrorMessage = "Field is required")]
        [StringLength(2048, ErrorMessage = "Must be under 2048 characters")]
        public String ITEM_VALUE { get; set; }
        public int ITEM_ORDER { get; set; }
        public String DESCRIPTION { get; set; }
        public String IS_USE { get; set; }

        public bool? IS_USE_BOL
        {
            get
            {
                return IS_USE == "Y" ? true : false;
            }
            set
            {
                if (value == true) IS_USE = "Y";
                else IS_USE = "N";             
            }
        }

        public String IS_RESTRICT { get; set; }

        public bool? IS_RESTRICT_BOL
        {
            get
            {
                return IS_RESTRICT == "Y" ? true : false;
            }
            set
            {
                if (value == true) IS_RESTRICT = "Y";
                else IS_RESTRICT = "N";                            
            }
        }
        #endregion

        #region "Constructors"
        public TB_M_LOOKUPInfo()
        {
            ID = 0;
            ROW_NO = 0;
            DOMAIN_CODE = string.Empty;
            ITEM_CODE = string.Empty;
            ITEM_VALUE = string.Empty;
            ITEM_ORDER = 0;
            DESCRIPTION = string.Empty;
            IS_USE = string.Empty;
            IS_RESTRICT = string.Empty;
        }

        public TB_M_LOOKUPInfo(int id, int ROW_NO, string DOMAIN_CODE, string ITEM_CODE, string ITEM_VALUE, int ITEM_ORDER, string DESCRIPTION, string IS_USE, string IS_RESTRICT)
        {
            this.ID = ID;
            this.ROW_NO = ROW_NO;
            this.DOMAIN_CODE = DOMAIN_CODE;
            this.ITEM_CODE = ITEM_CODE;
            this.ITEM_VALUE = ITEM_VALUE;
            this.ITEM_ORDER = ITEM_ORDER;
            this.DESCRIPTION = DESCRIPTION;
            this.IS_USE = IS_USE;
            this.IS_RESTRICT = IS_RESTRICT;
        }
        #endregion
    }
}