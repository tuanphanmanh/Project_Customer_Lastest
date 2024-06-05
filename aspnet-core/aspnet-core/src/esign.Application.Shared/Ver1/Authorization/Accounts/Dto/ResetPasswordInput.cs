using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Abp.Auditing;
using Abp.Runtime.Security;
using Abp.Runtime.Validation;

namespace esign.Authorization.Accounts.Dto.Ver1
{
    public class ResetPasswordInput: IShouldNormalize
    {
        public long UserId { get; set; }

        public string ResetCode { get; set; }
        public DateTime ExpireDate { get; set; }

        [DisableAuditing]
        public string Password { get; set; }
        /// <summary>
        /// Encrypted values for {TenantId}, {UserId}, {ResetCode} and {ExpireDate}
        /// </summary>
        /// //requied
        public string inputCode { get; set; }

        public void Normalize()
        {
            ResolveParameters();
        }

        protected virtual void ResolveParameters()
        {
            if (!string.IsNullOrEmpty(inputCode))
            {
                try
                {
                    string decodedParam = HttpUtility.UrlDecode(inputCode);
                    var parameters = SimpleStringCipher.Instance.Decrypt(decodedParam);
                    var query = HttpUtility.ParseQueryString(parameters);

                    if (query["userId"] != null)
                    {
                        UserId = Convert.ToInt32(query["userId"]);
                    }

                    if (query["resetCode"] != null)
                    {
                        ResetCode = query["resetCode"];
                    }

                    //if (query["expireDate"] == null)
                    //{
                    //    throw new AbpValidationException();
                    //}
                    
                    //ExpireDate = Convert.ToDateTime(query["expireDate"]);

                }
                catch (Exception e)
                {
                    throw new AbpValidationException("Invalid reset password link!");
                }
            }
        }
    }
}
