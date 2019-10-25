namespace LibraProgramming.Game.Towers.Core
{
    public interface IFileProvider
    {
        IFile GetFile(string path);
    }
}