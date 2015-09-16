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
        /// Parent directory.
        /// </summary>
        IDirectory Directory { get; }
    }
}