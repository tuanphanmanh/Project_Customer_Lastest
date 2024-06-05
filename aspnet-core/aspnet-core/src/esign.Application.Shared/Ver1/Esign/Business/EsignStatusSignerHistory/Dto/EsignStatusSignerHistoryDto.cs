using System;

namespace esign.Master.Dto.Ver1
{
    public class EsignStatusSignerHistoryGetByRequestIdDto
    {
        // Id của Signer
        public virtual long Id { get; set; }

        public virtual int? SigningOrder { get; set; }

        public virtual DateTime SignedDate { get; set; }

        public virtual string Note { get; set; }

        public virtual string StatusCode { get; set; }

    }

}


