using System.IO;
using System.IO.IsolatedStorage;

namespace PeterWibeck.ScrumyVSPlugin.TFS
{
    public class IsolatedStorageFileStreamAdapter : IsolatedStorageFileStream, IIsolatedStorageFileStream
    {
        public IsolatedStorageFileStreamAdapter(string path, FileMode mode,
                                                IsolatedStorageFile isf)
            : base(path, mode, isf)
        {
        }

        public IsolatedStorageFileStreamAdapter(string path, FileMode mode,
                                                FileAccess access, IsolatedStorageFile isf)
            : base(path, mode, access, isf)
        {
        }

        public IsolatedStorageFileStreamAdapter(string path, FileMode mode,
                                                FileAccess access, FileShare share, IsolatedStorageFile isf)
            : base(path, mode, access, share, isf)
        {
        }

        #region IIsolatedStorageFileStream Members

        public Stream BaseStream
        {
            get { return this; }
        }

        #endregion
    }
}