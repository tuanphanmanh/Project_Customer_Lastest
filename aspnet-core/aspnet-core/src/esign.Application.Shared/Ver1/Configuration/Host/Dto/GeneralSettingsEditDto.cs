namespace esign.Configuration.Host.Dto.Ver1
{
    public class GeneralSettingsEditDto
    {
        public string Timezone { get; set; }

        /// <summary>
        /// This value is only used for comparing user's timezone to default timezone
        /// </summary>
        public string TimezoneForComparison { get; set; }
    }
}