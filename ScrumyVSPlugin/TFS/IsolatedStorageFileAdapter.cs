using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace PeterWibeck.ScrumyVSPlugin.TFS
{
    public class IsolatedStorageFileAdapter : IIsolatedStorageFile
    {
        #region StoreType enum

        public enum StoreType
        {
            Application,
            Assembly
        }

        #endregion

        private readonly IsolatedStorageFile wrappedSubject;

        public IsolatedStorageFileAdapter()
        {
            wrappedSubject = IsolatedStorageFile.GetMachineStoreForAssembly();
        }

        public IsolatedStorageFileAdapter(StoreType storeType)
        {
            switch (storeType)
            {
                case StoreType.Application:
                    wrappedSubject = IsolatedStorageFile.GetUserStoreForApplication();
                    break;
                case StoreType.Assembly:
                    wrappedSubject = IsolatedStorageFile.GetMachineStoreForAssembly();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("storeType");
            }
        }

        #region IIsolatedStorageFile Members

        public void Dispose()
        {
            wrappedSubject.Dispose();
        }

        public long Quota
        {
            get { return wrappedSubject.Quota; }
        }

        public long AvailableFreeSpace
        {
            get { return wrappedSubject.AvailableFreeSpace; }
        }

        public void Remove()
        {
            wrappedSubject.Remove();
        }

        public IIsolatedStorageFileStream CreateStream(string path, FileMode mode)
        {
            return new IsolatedStorageFileStreamAdapter(path, mode,
                                                        wrappedSubject);
        }

        public void DeleteFile(string filePath)
        {
            wrappedSubject.DeleteFile(filePath);
        }

        public bool FileExists(string filePath)
        {
            return wrappedSubject.FileExists(filePath);
        }

        #endregion

        public string[] GetFileNames(string searchPattern)
        {
            return wrappedSubject.GetFileNames(searchPattern);
        }
    }
}