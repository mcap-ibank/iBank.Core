using PCLExt.FileStorage;
using PCLExt.FileStorage.Folders;

namespace iBank.Core.Folders
{
    public class ConfigFolder : BaseFolder
    {
        public ConfigFolder() : base(GetFolder()) { }
        private static IFolder GetFolder()
        {
            var appfolder = new ApplicationRootFolder();
            return appfolder?.Exists != true ? new LocalRootFolder() : (IFolder) appfolder;
        }
    }
}
