using System;
using System.Collections.Generic;
using System.Text;

namespace esign.Common.Dto.Ver1
{
    public class SignDigitalDTO
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public byte[] FileSignature { get; set; }
        public long x { get; set; }
        public long y { get; set; }
        public long w { get; set; }
        public long h { get; set; }
        public long PageNo { get; set; }

        public DateTime ApprovedDate { get; set; }
    }
}
