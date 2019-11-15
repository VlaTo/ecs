namespace LibraProgramming.Ecs.Core.Path
{
    internal enum PathTerminal
    {
        EndOfStream,
        PathDelimiter,
        WildCard,
        UpLevel
    }

    internal sealed class TerminalToken : PathToken
    {
        public PathTerminal Terminal
        {
            get;
        }

        public TerminalToken(PathTerminal terminal)
        {
            Terminal = terminal;
        }
    }
}