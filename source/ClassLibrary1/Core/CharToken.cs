﻿namespace ClassLibrary1.Core
{
    internal enum PathTerminal
    {
        EndOfStream,
        PathDelimiter,
        WildCard
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