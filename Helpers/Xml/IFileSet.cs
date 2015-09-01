using System.Collections.Generic;

namespace Helpers.Xml
{
    public interface IFileSet<TEntity> : ICollection<TEntity>
    {
        /// <summary>
        /// Save the set to its file.
        /// </summary>
        void Save();

        /// <summary>
        /// Revert the set back to its file.
        /// </summary>
        void Revert();
    }
}