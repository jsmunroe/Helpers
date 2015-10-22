namespace Helpers.Contracts
{
    public interface IFileCopier<in TFrom, in TTo>
        where TFrom : IFile 
        where TTo : IFile
    {
        void Copy(TFrom a_from, TTo a_to);
    }
}