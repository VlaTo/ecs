namespace LibraProgramming.Ecs.Core.Path
{
    internal abstract class PathToken
    {
        /// <summary>
        /// 
        /// </summary>
        public static PathToken Delimiter { get; } = new TerminalToken(PathTerminal.PathDelimiter);

        /// <summary>
        /// 
        /// </summary>
        public static PathToken WildCard { get; } = new TerminalToken(PathTerminal.WildCard);

        /// <summary>
        /// 
        /// </summary>
        public static PathToken EndOfStream { get; } = new TerminalToken(PathTerminal.EndOfStream);

        /// <summary>
        /// 
        /// </summary>
        public static PathToken String(string segment) => new SegmentToken(segment);
    }
}