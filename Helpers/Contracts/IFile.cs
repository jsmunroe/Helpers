namespace Helpers.Contracts
{
    public interface IFile
    {
        /// <summary>
        /// Whether the file exists.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// File name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Full path to this file.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Parent directory.
        /// </summary>
        IDirectory Directory { get; }
    }
}