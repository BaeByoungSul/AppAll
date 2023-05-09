using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClientLib
{
    // CustomStream Event Args
    public class MyProgressChangedEventArgs : EventArgs
    {
        public long BytesRead;
        public long Length;

        public MyProgressChangedEventArgs(long bytesRead, long length)
        {
            BytesRead = bytesRead;
            Length = length;
        }
    }

    /// <summary>
    /// 파일 전송 진행율을 보기 위해서 만듬
    /// </summary>
    public class CustomStream : Stream
    {
        private readonly Stream _file;
        private readonly long _length;
        private long _bytesRead;

        public event EventHandler<MyProgressChangedEventArgs> ProgressChanged;

        public CustomStream(Stream fileStream, long fileLength)
        {
            _file = fileStream;
            //_length = _file.Length;
            _length = fileLength;
            _bytesRead = 0;
            if (ProgressChanged != null)
            {
                ProgressChanged(this, new MyProgressChangedEventArgs(_bytesRead, _length));
            }
        }


        public override bool CanRead { get { return true; } }

        public override bool CanSeek { get { return false; } }

        public override bool CanWrite { get { return false; } }

        public override void Flush() { throw new Exception("This stream does not support writing."); }

        public override long Length { get { throw new Exception("This stream does not support the Length property."); } }

        public override long Position
        {
            get { return _bytesRead; }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }


        public override int Read(byte[] buffer, int offset, int count)
        {
            int result = _file.Read(buffer, offset, count);

            _bytesRead += result;

            // Console.WriteLine("bytes read {0}", _bytesRead);
            if (ProgressChanged != null)
            {
                ProgressChanged(this, new MyProgressChangedEventArgs(_bytesRead, _length));
            }
            return result;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

    } // END CustomStream

}
