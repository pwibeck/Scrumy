using System;
using System.IO;
using System.Security;

namespace PeterWibeck.ScrumyVSPlugin.TFS
{
    public interface IIsolatedStorageFile : IDisposable
    {
        long Quota { [SecuritySafeCritical]
        get; }

        long AvailableFreeSpace { [SecuritySafeCritical]
        get; }

        [SecuritySafeCritical]
        void Remove();

        IIsolatedStorageFileStream CreateStream(string path, FileMode mode);

        void DeleteFile(string filePath);

        bool FileExists(string filePath);
    }
}