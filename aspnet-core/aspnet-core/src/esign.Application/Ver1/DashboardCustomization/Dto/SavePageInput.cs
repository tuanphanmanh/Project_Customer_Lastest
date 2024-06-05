using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esign.DashboardCustomization.Dto.Ver1
{
    public class SavePageInput
    {
        public string DashboardName { get; set; }

        public string Application { get; set; }

        public List<Page> Pages { get; set; }
    }
}
