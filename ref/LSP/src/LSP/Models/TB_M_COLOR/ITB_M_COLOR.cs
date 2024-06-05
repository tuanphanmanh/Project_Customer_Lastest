using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSP.Models.TB_M_COLOR
{
	public interface ITB_M_COLOR
	{
		TB_M_COLORInfo TB_M_COLOR_Get(string id);       
		
		IList<TB_M_COLORInfo> TB_M_COLOR_Gets(string ID);

        IList<TB_M_COLORInfo> TB_M_COLOR_GetsByNotVehicleID(string VEHICLE_M_ID);

        IList<TB_M_COLORInfo> TB_M_COLOR_GetsByVehicleID(string VEHICLE_M_ID);

        IList<TB_M_COLORInfo> TB_M_COLOR_SearchByVehicleID(TB_M_COLORInfo obj);

        IList<TB_M_COLORInfo> TB_M_COLOR_Search(TB_M_COLORInfo obj);
        
		int TB_M_COLOR_Insert(TB_M_COLORInfo obj);
		
		int TB_M_COLOR_Update(TB_M_COLORInfo obj);
		
		int TB_M_COLOR_Delete(string id);
    }
}

