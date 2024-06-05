namespace esign.Notifications.Dto.Ver1
{
    public class SetNotificationAsReadOutput
    {
        public bool Success { get; set; }

        public SetNotificationAsReadOutput(bool success)
        {
            Success = success;
        }
    }
}
