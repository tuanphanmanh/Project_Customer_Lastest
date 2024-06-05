using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace esign.Business.Dto.Ver1
{


    public class VersionDetailAppDto
    {
        [StringLength(500)]
        public string VersionName { get; set; }
        [StringLength(500)]
        public string OperatingSystem { get; set; }
        [StringLength(1000)]
        public string UrlConfig { get; set; }
        public bool? IsForceUpdate { get; set; }
    }
    public class VersionAppDto
    {
   
        public VersionDetailAppDto Android { get; set; }
        public VersionDetailAppDto Ios { get; set; }

    }
}
