namespace LibraProgramming.Ecs.Core.Path
{
    internal sealed class NameToken : PathToken
    {
        public string Name
        {
            get;
        }

        public NameToken(string name)
        {
            Name = name;
        }
    }
}