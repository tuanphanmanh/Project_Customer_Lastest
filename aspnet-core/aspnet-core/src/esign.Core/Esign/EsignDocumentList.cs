using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esign.Esign
{
    [Table("EsignDocumentList")]
    public class EsignDocumentList : FullAuditedEntity<long>, IEntity<long>
    {
        public long? RequestId { get; set; }
        [StringLength(250)]
        public string DocumentName { get; set; }
        [StringLength(500)]
        public string DocumentPath { get; set; }
        [StringLength(50)]
        public string Md5Hash { get; set; }
        [Required]
        public int DocumentOrder { get; set; }
        [Required]
        public int TotalPage { get; set; }
        [Required]
        public int TotalSize { get; set; }
        public byte[] EncryptedUserPass { get; set; }
        public byte[] SecretKey { get; set; }        
        //public byte[] DocumentSmallPart { get; set; }
        public bool IsAdditionalFile { get; set; }
        public long? AffiliateReferenceId { get; set; }
        [StringLength(50)]
        public string QrRandomCode { get; set; }
        public bool IsDigitalSignatureFile { get; set; }

    }
}
