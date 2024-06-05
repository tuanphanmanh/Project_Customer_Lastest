
using esign.SendEmail.Dto.Ver1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace esign.SendEmail.Ver1
{
    public interface ISendEmail
    {
        Task SendEmail(EmailContentDto input);
    }
}
