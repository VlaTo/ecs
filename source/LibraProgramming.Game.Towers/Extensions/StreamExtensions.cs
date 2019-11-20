using System;
using System.IO;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace LibraProgramming.Game.Towers.Extensions
{
    internal static class StreamExtensions
    {
        /*public static IRandomAccessStream AsRandomAccessStream(this Stream stream)
        {
            if (null == stream)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return new MemoryRandomAccessStream(stream);
        }*/

        private sealed class MemoryRandomAccessStream : IRandomAccessStream
        {
            private readonly Stream stream;

            public bool CanRead => stream.CanRead;

            public bool CanWrite => stream.CanWrite;

            public ulong Position => (ulong) stream.Position;

            public ulong Size
            {
                get => (ulong) stream.Length;
                set => stream.SetLength((long) value);
            }

            public MemoryRandomAccessStream(Stream stream)
            {
                this.stream = stream;
            }

            public void Dispose()
            {
                stream.Dispose();
            }

            public IAsyncOperationWithProgress<IBuffer, uint> ReadAsync(IBuffer buffer, uint count, InputStreamOptions options)
            {
                var inputStream = GetInputStreamAt(0UL);
                return inputStream.ReadAsync(buffer, count, options);
            }

            public IAsyncOperationWithProgress<uint, uint> WriteAsync(IBuffer buffer)
            {
                var outputStream = GetOutputStreamAt(0UL);
                return outputStream.WriteAsync(buffer);
            }

            public IAsyncOperation<bool> FlushAsync()
            {
                var outputStream = GetOutputStreamAt(0UL);
                return outputStream.FlushAsync();
            }

            public IInputStream GetInputStreamAt(ulong position)
            {
                if (false == stream.CanSeek)
                {
                    throw new InvalidOperationException();
                }

                var positionRequired = (long)position;
                var newPosition = stream.Seek(positionRequired, SeekOrigin.Begin);

                if (newPosition != positionRequired)
                {
                    throw new Exception();
                }

                return stream.AsInputStream();
            }

            public IOutputStream GetOutputStreamAt(ulong position)
            {
                if (false == stream.CanSeek)
                {
                    throw new InvalidOperationException();
                }

                var positionRequired = (long)position;
                var newPosition = stream.Seek(positionRequired, SeekOrigin.Begin);

                if (newPosition != positionRequired)
                {
                    throw new Exception();
                }

                return stream.AsOutputStream();
            }

            public void Seek(ulong position)
            {
                if (false == stream.CanSeek)
                {
                    throw new InvalidOperationException();
                }

                var positionRequired = (long) position;
                var newPosition = stream.Seek(positionRequired, SeekOrigin.Begin);

                if (newPosition != positionRequired)
                {
                    throw new Exception();
                }
            }

            public IRandomAccessStream CloneStream()
            {
                throw new NotSupportedException();
            }
        }
    }
}