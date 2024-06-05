//using Abp.Domain.Entities;
//using Abp.Domain.Entities.Auditing;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace esign.Esign
//{
//    [Table("EsignDocumentListTemp")]
//    public class EsignDocumentListTemp : FullAuditedEntity<long>, IEntity<long>
//    {
//        [StringLength(250)]
//        public string DocumentName { get; set; }
//        [StringLength(500)]
//        public string DocumentPath { get; set; }
//        [StringLength(50)]
//        public string Md5Hash { get; set; }
//        [Required]
//        public int TotalPage { get; set; }
//        [Required]
//        public int TotalSize { get; set; }
//        public byte[] EncryptedUserPass { get; set; }
//        public byte[] SecretKey { get; set; }
//        public bool? IsUserPassInput { get; set; }
//        //public byte[] DocumentSmallPart { get; set; }        
//    }
//}
    