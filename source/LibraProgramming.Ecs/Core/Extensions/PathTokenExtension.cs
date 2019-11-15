using System;
using LibraProgramming.Ecs.Core.Path;

namespace LibraProgramming.Ecs.Core.Extensions
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
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsName(this PathToken token, out string name)
        {
            if (null == token)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (token is NameToken nameToken)
            {
                name = nameToken.Name;
                return true;
            }

            name = null;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsUpLevel(this PathToken token)
        {
            if (null == token)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return token is TerminalToken term && PathTerminal.UpLevel == term.Terminal;
        }
    }
}