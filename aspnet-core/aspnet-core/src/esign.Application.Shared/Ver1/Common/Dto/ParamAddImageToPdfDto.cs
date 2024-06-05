using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace esign.Common.Dto.Ver1
{
    public class ParamAddImageToPdfDto
    {
        public long FileDocumentId { get; set; }
        public string FileDocumentPath { get; set; }
        public string FileDocumentName { get; set; }
        public string Md5Hash { get; set; }
        public byte[] EncryptedUserPass { get; set; }
        public string IsUserPassInput { get; set; }
        public long? TotalSize { get; set; }
        public byte[] SecretKey { get; set; }
        public List<SignatureImageAndPositionDto> signatureImageAndPositionsApprove { get; set; }
        public List<SignatureImageAndPositionDto> signatureImageAndPositionsNotApprove { get; set; }
    }
}
