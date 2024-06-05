using System.Collections.Generic;

namespace esign.Configuration.Dto.Ver1
{
    public class ExternalLoginSettingsDto
    {
        public List<string> EnabledSocialLoginSettings { get; set; } = new List<string>();
    }
}