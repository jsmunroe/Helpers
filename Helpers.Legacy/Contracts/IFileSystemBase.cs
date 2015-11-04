using System;
using Helpers.IO;

namespace Helpers.Contracts
{
    public interface IFileSystemBase
    {
        /// <summary>
        /// Whether the directory exists.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Directory name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Full path to this directory.
        /// </summary>
        PathBuilder Path { get; }

        /// <summary>
        /// Time of creation (UTC).
        /// </summary>
        DateTime CreatedTimeUtc { get; }

        /// <summary>
        /// Time of last modification (UTC).
        /// </summary>
        DateTime LastModifiedTimeUtc { get; }


    }
}