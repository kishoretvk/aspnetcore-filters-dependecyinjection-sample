using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleWareFiltersExample.Logger
{
    /// <summary>
    /// A <see cref="Stream"/> which wraps around another <see cref="Stream"/> and copies all data to a <see cref="MemoryStream"/>.
    /// Used by the logging framework.
    /// </summary>
    public class ResponseLoggerStream : Stream
    {
        private readonly Stream inner;
        private readonly MemoryStream tracerStream;
        private readonly bool ownsParent;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseLoggerStream"/> class.
        /// </summary>
        /// <param name="inner">
        /// The stream which is being wrapped.
        /// </param>
        /// <param name="ownsParent">
        /// A value indicating whether the <paramref name="inner"/> should be disposed when the
        /// <see cref="RequestLoggerStream"/> is disposed.
        /// </param>
        public ResponseLoggerStream(Stream inner, bool ownsParent)
        {
            if (inner == null)
            {
                throw new ArgumentNullException(nameof(inner));
            }

            if (inner is ResponseLoggerStream)
            {
                throw new InvalidOperationException("nesting of ResponseLoggerStream objects is not allowed");
            }

            this.inner = inner;
            this.tracerStream = new MemoryStream();
            this.ownsParent = ownsParent;
        }

        /// <summary>
        /// Gets a <see cref="MemoryStream"/>, which holds a copy of all data which was written to the inner
        /// stream.
        /// </summary>
        public MemoryStream TracerStream
        {
            get
            {
                return this.tracerStream;
            }
        }

        /// <summary>
        /// Gets the <see cref="Stream"/> around which this <see cref="ResponseLoggerStream"/> wraps.
        /// </summary>
        public Stream Inner
        {
            get { return this.inner; }
        }

        /// <inheritdoc/>
        public override bool CanRead
        {
            get
            {
                return this.inner.CanRead;
            }
        }

        /// <inheritdoc/>
        public override bool CanSeek
        {
            get
            {
                return this.inner.CanSeek;
            }
        }

        /// <inheritdoc/>
        public override bool CanWrite
        {
            get
            {
                return this.inner.CanWrite;
            }
        }

        /// <inheritdoc/>
        public override long Length
        {
            get
            {
                return this.inner.Length;
            }
        }

        /// <inheritdoc/>
        public override long Position
        {
            get
            {
                return this.inner.Position;
            }

            set
            {
                this.inner.Position = value;
            }
        }

        /// <inheritdoc/>
        public override void Flush()
        {
            this.inner.Flush();
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            this.tracerStream.Read(buffer, offset, count);
            return this.inner.Read(buffer, offset, count);
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            this.tracerStream.Seek(offset, origin);
            return this.inner.Seek(offset, origin);
        }

        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            this.tracerStream.SetLength(value);
            this.inner.SetLength(value);
        }

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            this.tracerStream.Write(buffer, offset, count);
            this.inner.Write(buffer, offset, count);
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            this.tracerStream.Dispose();

            if (this.ownsParent)
            {
                this.inner.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
