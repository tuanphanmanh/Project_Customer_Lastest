using Abp.Dependency;

namespace esign
{
    public class AppFolders : IAppFolders, ISingletonDependency
    {
        public string SampleProfileImagesFolder { get; set; }

        public string WebLogsFolder { get; set; }
    }
}