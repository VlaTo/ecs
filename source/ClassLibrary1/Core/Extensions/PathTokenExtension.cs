using System;

namespace ClassLibrary1.Core.Extensions
{
    internal static class PathTokenExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsEndOfStream(this PathToken token)
        {
            if (null == token)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return token is TerminalToken term && PathTerminal.EndOfStream == term.Terminal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsPathDelimiter(this PathToken token)
        {
            if (null == token)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return token is TerminalToken term && PathTerminal.PathDelimiter == term.Terminal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsWildCard(this PathToken token)
        {
            if (null == token)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return token is TerminalToken term && PathTerminal.WildCard == term.Terminal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="segment"></param>
        /// <returns></returns>
        public static bool IsSegment(this PathToken token, out string segment)
        {
            if (null == token)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (token is SegmentToken segmentToken)
            {
                segment = segmentToken.Segment;
                return true;
            }

            segment = null;

            return false;
        }
    }
}