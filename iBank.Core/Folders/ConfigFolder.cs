using PCLExt.FileStorage;
using PCLExt.FileStorage.Folders;

using System.Diagnostics;

namespace iBank.Core.Folders
{
    [DebuggerStepThrough]
    public class ConfigFolder : BaseFolder
    {
        public ConfigFolder() : base(GetFolder()) { }
        private static IFolder GetFolder()
        {
            var documents = new DocumentsRootFolder();
            return documents?.Exists != true ? new LocalRootFolder() : documents.CreateFolder("iBank", CreationCollisionOption.OpenIfExists);
        }
    }
}