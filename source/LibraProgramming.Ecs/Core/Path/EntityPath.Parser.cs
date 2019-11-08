using System;
using System.IO;
using LibraProgramming.Ecs.Core.Extensions;
using LibraProgramming.Ecs.Core.Path.Segments;

namespace LibraProgramming.Ecs.Core.Path
{
    /// <summary>
    /// 
    /// </summary>
    public partial class EntityPath
    {
        /// <summary>
        /// 
        /// </summary>
        private sealed class EntityPathParser
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            public EntityPathSegment Parse(string path)
            {
                var reader = new StringReader(path);

                using (var tokenizer = new EntityPathTokenizer(reader))
                {
                    return ParsePath(tokenizer);
                }
            }

            private EntityPathSegment ParsePath(EntityPathTokenizer tokenizer)
            {
                var token = tokenizer.GetNextToken();

                if (token.IsEndOfStream())
                {
                    return null;
                }

                if (token.IsWildCard())
                {
                    EnsureTail(tokenizer);
                    return new EntityPathWildCardSegment();
                }

                if (token.IsPathDelimiter())
                {
                    return ParseFromRoot(tokenizer);
                }

                if (token.IsName(out var key))
                {
                    var next = ParseDelimiter(tokenizer);
                    return new EntityKeySegment(key, next);
                }

                throw new Exception();
            }

            private EntityPathSegment ParseFromRoot(EntityPathTokenizer tokenizer)
            {
                var token = tokenizer.GetNextToken();

                if (token.IsEndOfStream())
                {
                    return null;
                }

                if (token.IsPathDelimiter())
                {
                    var next = ParseWildCardOrName(tokenizer);
                    return new EntityPathRootSegment(next);
                }

                throw new Exception();
            }

            private EntityPathSegment ParseWildCardOrName(EntityPathTokenizer tokenizer)
            {
                var token = tokenizer.GetNextToken();

                if (token.IsEndOfStream())
                {
                    return null;
                }

                if (token.IsWildCard())
                {
                    return ParseAnyLevelOrTail(tokenizer);
                }

                if (token.IsName(out var key))
                {
                    var next = ParseDelimiter(tokenizer);
                    return new EntityKeySegment(key, next);
                }

                throw new Exception();
            }

            private EntityPathSegment ParseAnyLevelOrTail(EntityPathTokenizer tokenizer)
            {
                EnsureTail(tokenizer);
                return new EntityPathWildCardSegment();
            }

            private EntityPathSegment ParseDelimiter(EntityPathTokenizer tokenizer)
            {
                var token = tokenizer.GetNextToken();

                if (token.IsEndOfStream())
                {
                    return null;
                }

                if (token.IsPathDelimiter())
                {
                    return ParseWildCardOrName(tokenizer);
                }

                throw new Exception();
            }

            private static void EnsureTail(EntityPathTokenizer tokenizer)
            {
                var token = tokenizer.GetNextToken();

                if (false == token.IsEndOfStream())
                {
                    throw new Exception();
                }
            }
        }
    }
}