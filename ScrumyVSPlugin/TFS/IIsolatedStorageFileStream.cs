using System;
using System.IO;
using System.Security;

namespace PeterWibeck.ScrumyVSPlugin.TFS
{
    public interface IIsolatedStorageFileStream : IDisposable
    {
        bool CanRead { get; }
        bool CanWrite { get; }
        bool CanSeek { get; }
        long Length { get; }
        long Position { get; set; }
        Stream BaseStream { get; }

        int Read(byte[] buffer, int offset, int count);
        int ReadByte();

        [SecuritySafeCritical]
        void Write(byte[] buffer, int offset, int count);

        [SecuritySafeCritical]
        void WriteByte(byte value);
    }
}