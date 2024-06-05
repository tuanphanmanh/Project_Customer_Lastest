using System.Collections.Generic;

namespace esign.Tenants.Dashboard.Dto.Ver1
{
    public class GetSalesSummaryOutput
    {
        public GetSalesSummaryOutput(List<SalesSummaryData> salesSummary)
        {
            SalesSummary = salesSummary;
        }


        public int TotalSales { get; set; }

        public int Revenue { get; set; }

        public int Expenses { get; set; }

        public int Growth { get; set; }

        public List<SalesSummaryData> SalesSummary { get; set; }

    }
}