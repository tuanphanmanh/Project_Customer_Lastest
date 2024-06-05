using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace LSP.Models.TB_R_PART_STOCK
{
    public interface ITB_R_PART_STOCK
    {
        TB_R_PART_STOCKInfo TB_R_PART_STOCK_Get(string id);

        IList<TB_R_PART_STOCKInfo> TB_R_PART_STOCK_Gets(string ID);

        IList<TB_R_PART_STOCKInfo> TB_R_PART_STOCK_Search(TB_R_PART_STOCKInfo obj);

        int TB_R_PART_STOCK_Insert(TB_R_PART_STOCKInfo obj);

        int TB_R_PART_STOCK_Update(TB_R_PART_STOCKInfo obj);

        int TB_R_PART_STOCK_Delete(string id);

        IList<TB_R_PART_STOCK_PIVOTInfo> TB_R_PART_STOCK_GET_PIVOT_MONTH(TB_R_PART_STOCK_PIVOTInfo obj);

        int TB_R_PART_STOCK_UPLOAD(DataTable _PART_RUNDOWN);
        int TB_R_PART_STOCK_MERGE(string CREATED_BY, DateTime CREATED_DATE);

        IList<TB_R_PART_STOCKInfo> TB_R_PART_STOCK_DetailsIO_Search(TB_R_PART_STOCKInfo obj);
        
    }
}