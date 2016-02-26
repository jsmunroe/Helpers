using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.IO
{
    /// <summary>
    /// AdvancedStreamReader
    ///  A a_stream reader that provides character position and provides the ability to seek to a character. 
    ///     This is done without reading the steam as a whole, it uses a buffered method of reading the a_stream
    ///     which is inherited from <see cref="StreamReader"/>.    
    /// </summary>
    /// <remarks>
    /// Caveats for use:
    /// 1) The position of the base a_stream will be set to 0 upon construction.
    /// 2) Seeking the base a_stream manually will cause <see cref="CharacterPosition"/> to become invalid.
    /// 3) If a new line character other than <see cref="Environment.NewLine"/> is used, it should be set 
    ///     in the constructor.
    /// 4) This class does not support text streams with more than one new line type.
    /// </remarks>
    public class AdvancedStreamReader : StreamReader
    {
        private int _newLineLength = Environment.NewLine.Length;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IO.StreamReader"/> class for the specified a_stream.
        /// </summary>
        /// <param name="a_stream">The a_stream to be read.</param>
        /// <param name="a_newLine">New line type.</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="a_stream"/> does not support reading.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="a_stream"/> is null.</exception>
        public AdvancedStreamReader(Stream a_stream, NewLineType a_newLine = NewLineType.Environment)
            : base(a_stream)
        {
            SetNewLine(a_newLine);
            ResetBaseStream();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IO.StreamReader"/> class for the specified a_stream, with the specified byte order mark detection option.
        /// </summary>
        /// <param name="a_stream">The a_stream to be read.</param>
        /// <param name="a_detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="a_newLine">New line type.</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="a_stream"/> does not support reading. </exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="a_stream"/> is null. </exception>
        public AdvancedStreamReader(Stream a_stream, bool a_detectEncodingFromByteOrderMarks, NewLineType a_newLine = NewLineType.Environment)
            : base(a_stream, a_detectEncodingFromByteOrderMarks)
        {
            SetNewLine(a_newLine);
            ResetBaseStream();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IO.StreamReader"/> class for the specified a_stream, with the specified character a_encoding.
        /// </summary>
        /// <param name="a_stream">The a_stream to be read.</param>
        /// <param name="a_encoding">The character a_encoding to use.</param>
        /// <param name="a_newLine">New line type.</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="a_stream"/> does not support reading.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="a_stream"/> or <paramref name="a_encoding"/> is null. </exception>
        public AdvancedStreamReader(Stream a_stream, Encoding a_encoding, NewLineType a_newLine = NewLineType.Environment)
            : base(a_stream, a_encoding)
        {
            SetNewLine(a_newLine);
            ResetBaseStream();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IO.StreamReader"/> class for the specified a_stream, with the specified character a_encoding and byte order mark detection option.
        /// </summary>
        /// <param name="a_stream">The a_stream to be read.</param>
        /// <param name="a_encoding">The character a_encoding to use.</param>
        /// <param name="a_detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file. </param>
        /// <param name="a_newLine">New line type.</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="a_stream"/> does not support reading.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="a_stream"/> or <paramref name="a_encoding"/> is null.</exception>
        public AdvancedStreamReader(Stream a_stream, Encoding a_encoding, bool a_detectEncodingFromByteOrderMarks, NewLineType a_newLine = NewLineType.Environment)
            : base(a_stream, a_encoding, a_detectEncodingFromByteOrderMarks)
        {
            SetNewLine(a_newLine);
            ResetBaseStream();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IO.StreamReader"/> class for the specified a_stream, with the specified character a_encoding, byte order mark detection option, and buffer size.
        /// </summary>
        /// <param name="a_stream">The a_stream to be read.</param>
        /// <param name="a_encoding">The character a_encoding to use.</param>
        /// <param name="a_detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="a_bufferSize">The minimum buffer size.</param>
        /// <param name="a_newLine">New line type.</param>
        /// <exception cref="T:System.ArgumentException">The a_stream does not support reading.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="a_stream"/> or <paramref name="a_encoding"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="a_bufferSize"/> is less than or equal to zero.</exception>
        public AdvancedStreamReader(Stream a_stream, Encoding a_encoding, bool a_detectEncodingFromByteOrderMarks, int a_bufferSize, NewLineType a_newLine = NewLineType.Environment)
            : base(a_stream, a_encoding, a_detectEncodingFromByteOrderMarks, a_bufferSize)
        {
            SetNewLine(a_newLine);
            ResetBaseStream();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IO.StreamReader"/> class for the specified a_stream based on the specified character a_encoding, byte order mark detection option, and buffer size, and optionally leaves the a_stream open.
        /// </summary>
        /// <param name="a_stream">The a_stream to read.</param>
        /// <param name="a_encoding">The character a_encoding to use.</param>
        /// <param name="a_detectEncodingFromByteOrderMarks">true to look for byte order marks at the beginning of the file; otherwise, false.</param>
        /// <param name="a_bufferSize">The minimum buffer size.</param>
        /// <param name="a_leaveOpen">true to leave the a_stream open after the <see cref="T:System.IO.StreamReader"/> object is disposed; otherwise, false.</param>
        /// <param name="a_newLine">New line type.</param>
        public AdvancedStreamReader(Stream a_stream, Encoding a_encoding, bool a_detectEncodingFromByteOrderMarks, int a_bufferSize, bool a_leaveOpen, NewLineType a_newLine = NewLineType.Environment)
            : base(a_stream, a_encoding, a_detectEncodingFromByteOrderMarks, a_bufferSize, a_leaveOpen)
        {
            SetNewLine(a_newLine);
            ResetBaseStream();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IO.StreamReader"/> class for the specified file name.
        /// </summary>
        /// <param name="a_path">The complete file a_path to be read.</param>
        /// <param name="a_newLine">New line type.</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="a_path"/> is an empty string ("").</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="a_path"/> is null.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified a_path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.IO.IOException"><paramref name="a_path"/> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
        public AdvancedStreamReader(string a_path, NewLineType a_newLine = NewLineType.Environment)
            : base(a_path)
        {
            SetNewLine(a_newLine);
            ResetBaseStream();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IO.StreamReader"/> class for the specified file name, with the specified byte order mark detection option.
        /// </summary>
        /// <param name="a_path">The complete file a_path to be read.</param>
        /// <param name="a_detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="a_newLine">New line type.</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="a_path"/> is an empty string ("").</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="a_path"/> is null.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified a_path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.IO.IOException"><paramref name="a_path"/> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
        public AdvancedStreamReader(string a_path, bool a_detectEncodingFromByteOrderMarks, NewLineType a_newLine = NewLineType.Environment)
            : base(a_path, a_detectEncodingFromByteOrderMarks)
        {
            SetNewLine(a_newLine);
            ResetBaseStream();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IO.StreamReader"/> class for the specified file name, with the specified character a_encoding.
        /// </summary>
        /// <param name="a_path">The complete file a_path to be read.</param>
        /// <param name="a_encoding">The character a_encoding to use.</param>
        /// <param name="a_newLine">New line type.</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="a_path"/> is an empty string ("").</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="a_path"/> or <paramref name="a_encoding"/> is null.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified a_path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.NotSupportedException"><paramref name="a_path"/> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
        public AdvancedStreamReader(string a_path, Encoding a_encoding, NewLineType a_newLine = NewLineType.Environment)
            : base(a_path, a_encoding)
        {
            SetNewLine(a_newLine);
            ResetBaseStream();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IO.StreamReader"/> class for the specified file name, with the specified character a_encoding and byte order mark detection option.
        /// </summary>
        /// <param name="a_path">The complete file a_path to be read.</param>
        /// <param name="a_encoding">The character a_encoding to use.</param>
        /// <param name="a_detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="a_newLine">New line type.</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="a_path"/> is an empty string ("").</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="a_path"/> or <paramref name="a_encoding"/> is null.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified a_path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.NotSupportedException"><paramref name="a_path"/> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
        public AdvancedStreamReader(string a_path, Encoding a_encoding, bool a_detectEncodingFromByteOrderMarks, NewLineType a_newLine = NewLineType.Environment)
            : base(a_path, a_encoding, a_detectEncodingFromByteOrderMarks)
        {
            SetNewLine(a_newLine);
            ResetBaseStream();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IO.StreamReader"/> class for the specified file name, with the specified character a_encoding, byte order mark detection option, and buffer size.
        /// </summary>
        /// <param name="a_path">The complete file a_path to be read.</param>
        /// <param name="a_encoding">The character a_encoding to use.</param>
        /// <param name="a_detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="a_bufferSize">The minimum buffer size, in number of 16-bit characters.</param>
        /// <param name="a_newLine">New line type.</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="a_path"/> is an empty string ("").</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="a_path"/> or <paramref name="a_encoding"/> is null.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified a_path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.NotSupportedException"><paramref name="a_path"/> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="a_bufferSize"/> is less than or equal to zero. </exception>
        public AdvancedStreamReader(string a_path, Encoding a_encoding, bool a_detectEncodingFromByteOrderMarks, int a_bufferSize, NewLineType a_newLine = NewLineType.Environment)
            : base(a_path, a_encoding, a_detectEncodingFromByteOrderMarks, a_bufferSize)
        {
            SetNewLine(a_newLine);
            ResetBaseStream();
        }

        /// <summary>
        /// Character position.
        /// </summary>
        public long CharacterPosition { get; private set; }

        /// <summary>
        /// Seek to the given character position (<paramref name="a_characterPosition"/>).
        /// </summary>
        /// <param name="a_characterPosition">Character position to which to seek.</param>
        public virtual void SeekCharacter(long a_characterPosition)
        {
            var blocks = a_characterPosition / 1024;
            var left = (int)(a_characterPosition % 1024);

            BaseStream.Seek(0, SeekOrigin.Begin);

            var buffer = new char[1024];
            for (var i = 0; i < blocks; i++)
                Read(buffer, 0, 1024);

            if (left > 0)
                Read(buffer, 0, left);
        }

        /// <summary>
        /// Reads the next character from the input a_stream and advances the character position by one character.
        /// </summary>
        /// <returns>
        /// The next character from the input a_stream represented as an <see cref="T:System.Int32"/> object, or -1 if no more characters are available.
        /// </returns>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override int Read()
        {
            CharacterPosition++;

            return base.Read();
        }

        /// <summary>
        /// Reads a specified maximum of characters from the current a_stream into a buffer, beginning at the specified index.
        /// </summary>
        /// <returns>
        /// The number of characters that have been read, or 0 if at the end of the a_stream and no data was read. The number will be less than or equal to the <paramref name="count"/> parameter, depending on whether the data is available within the a_stream.
        /// </returns>
        /// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index"/> and (<paramref name="index + count - 1"/>) replaced by the characters read from the current source. </param><param name="index">The index of <paramref name="buffer"/> at which to begin writing. </param><param name="count">The maximum number of characters to read. </param><exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index"/> is less than <paramref name="count"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> is null. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> or <paramref name="count"/> is negative. </exception><exception cref="T:System.IO.IOException">An I/O error occurs, such as the a_stream is closed. </exception>
        public override int Read(char[] buffer, int index, int count)
        {
            var readCount = base.Read(buffer, index, count);

            CharacterPosition += readCount;

            return readCount;
        }

        /// <summary>
        /// Reads a specified maximum number of characters from the current a_stream asynchronously and writes the data to a buffer, beginning at the specified index. 
        /// </summary>
        /// <returns>
        /// The number of characters that have been read, or 0 if at the end of the a_stream and no data was read. The number will be less than or equal to the <paramref name="count"/> parameter, depending on whether the data is available within the a_stream.
        /// </returns>
        /// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index"/> and (<paramref name="index"/> + <paramref name="count"/> - 1) replaced by the characters read from the current source.</param><param name="index">The position in <paramref name="buffer"/> at which to begin writing.</param><param name="count">The maximum number of characters to read. If the end of the a_stream is reached before the specified number of characters is written into the buffer, the current method returns.</param><exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> or <paramref name="count"/> is negative.</exception><exception cref="T:System.ArgumentException">The sum of <paramref name="index"/> and <paramref name="count"/> is larger than the buffer length.</exception><exception cref="T:System.ObjectDisposedException">The a_stream has been disposed.</exception><exception cref="T:System.InvalidOperationException">The reader is currently in use by a previous read operation. </exception>
        public override async Task<int> ReadAsync(char[] buffer, int index, int count)
        {
            var readCount = await base.ReadAsync(buffer, index, count);

            CharacterPosition += readCount;

            return readCount;
        }

        /// <summary>
        /// Reads a specified maximum number of characters from the current a_stream and writes the data to a buffer, beginning at the specified index.
        /// </summary>
        /// <returns>
        /// The number of characters that have been read. The number will be less than or equal to <paramref name="count"/>, depending on whether all input characters have been read.
        /// </returns>
        /// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index"/> and (<paramref name="index + count - 1"/>) replaced by the characters read from the current source.</param><param name="index">The position in <paramref name="buffer"/> at which to begin writing.</param><param name="count">The maximum number of characters to read.</param><exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> is null. </exception><exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index"/> is less than <paramref name="count"/>. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> or <paramref name="count"/> is negative. </exception><exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.StreamReader"/> is closed. </exception><exception cref="T:System.IO.IOException">An I/O error occurred. </exception>
        public override int ReadBlock(char[] buffer, int index, int count)
        {
            var readCount = base.ReadBlock(buffer, index, count);

            CharacterPosition += readCount;

            return readCount;
        }

        /// <summary>
        /// Reads a specified maximum number of characters from the current a_stream asynchronously and writes the data to a buffer, beginning at the specified index.
        /// </summary>
        /// <returns>
        /// The number of characters that have been read. The number will be less than or equal to <paramref name="count"/>, depending on whether all input characters have been read.
        /// </returns>
        /// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index"/> and (<paramref name="index"/> + <paramref name="count"/> - 1) replaced by the characters read from the current source.</param><param name="index">The position in <paramref name="buffer"/> at which to begin writing.</param><param name="count">The maximum number of characters to read. If the end of the a_stream is reached before the specified number of characters is written into the buffer, the method returns.</param><exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> or <paramref name="count"/> is negative.</exception><exception cref="T:System.ArgumentException">The sum of <paramref name="index"/> and <paramref name="count"/> is larger than the buffer length.</exception><exception cref="T:System.ObjectDisposedException">The a_stream has been disposed.</exception><exception cref="T:System.InvalidOperationException">The reader is currently in use by a previous read operation. </exception>
        public override async Task<int> ReadBlockAsync(char[] buffer, int index, int count)
        {
            var readCount = await base.ReadBlockAsync(buffer, index, count);

            CharacterPosition += readCount;

            return readCount;
        }

        /// <summary>
        /// Reads a line of characters from the current a_stream and returns the data as a string.
        /// </summary>
        /// <returns>
        /// The next line from the input a_stream, or null if the end of the input a_stream is reached.
        /// </returns>
        /// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string. </exception><exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override string ReadLine()
        {
            var readString = base.ReadLine();

            if (readString == null)
                return null;

            CharacterPosition += readString.Length;

            if (!EndOfStream)
                CharacterPosition += _newLineLength;

            return readString;
        }

        /// <summary>
        /// Reads a line of characters asynchronously from the current a_stream and returns the data as a string.
        /// </summary>
        /// <returns>
        /// The next line from the input a_stream, or null if the end of the input a_stream is reached.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The number of characters in the next line is larger than <see cref="F:System.Int32.MaxValue"/>.</exception><exception cref="T:System.ObjectDisposedException">The a_stream has been disposed.</exception><exception cref="T:System.InvalidOperationException">The reader is currently in use by a previous read operation. </exception>
        public override async Task<string> ReadLineAsync()
        {
            var readString = await base.ReadLineAsync();

            if (readString == null)
                return null;

            CharacterPosition += readString.Length;

            if (!EndOfStream)
                CharacterPosition += _newLineLength;

            return readString;
        }

        /// <summary>
        /// Reads all characters from the current position to the end of the a_stream.
        /// </summary>
        /// <returns>
        /// The rest of the a_stream as a string, from the current position to the end. If the current position is at the end of the a_stream, returns an empty string ("").
        /// </returns>
        /// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string. </exception><exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override string ReadToEnd()
        {
            var readString = base.ReadToEnd();

            CharacterPosition += readString.Length;

            return readString;
        }

        /// <summary>
        /// Reads all characters from the current position to the end of the a_stream asynchronously and returns them as one string.
        /// </summary>
        /// <returns>
        /// The rest of the a_stream as a string, from the current position to the end. If the current position is at the end of the a_stream, returns an empty string ("").
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The number of characters is larger than <see cref="F:System.Int32.MaxValue"/>.</exception><exception cref="T:System.ObjectDisposedException">The a_stream has been disposed.</exception><exception cref="T:System.InvalidOperationException">The reader is currently in use by a previous read operation. </exception>
        public override async Task<string> ReadToEndAsync()
        {
            var readString = await base.ReadToEndAsync();

            CharacterPosition += readString.Length;

            return readString;
        }

        /// <summary>
        /// Reset the base a_stream to the beginning. Used by the constructors.
        /// </summary>
        private void ResetBaseStream()
        {
            BaseStream?.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// Set the new line property to the given new line type (<paramref name="a_newLineType"/>).
        /// </summary>
        /// <param name="a_newLineType">New line type.</param>
        private void SetNewLine(NewLineType a_newLineType)
        {
            switch (a_newLineType)
            {
                case NewLineType.Environment:
                    _newLineLength = Environment.NewLine.Length;
                    break;
                case NewLineType.CrNl:
                    _newLineLength = 2;
                    break;
                case NewLineType.Nl:
                case NewLineType.Cr:
                    _newLineLength = 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(a_newLineType), a_newLineType, null);
            }
        }
    }

    public enum NewLineType
    {
        Environment,    // Get the value from the environment (default).
        Nl,             // New line only ("\n").
        CrNl,           // Carriage return followed by new line. ("\r\n").
        Cr              // Carriage return ("\r").
    }
}
