namespace esign.Configuration.Dto.Ver1
{
    public class ThemeHeaderSettingsDto
    {
        public bool DesktopFixedHeader => true;

        public bool MobileFixedHeader => true;

        public string MinimizeDesktopHeaderType { get; set; }
    }
}
