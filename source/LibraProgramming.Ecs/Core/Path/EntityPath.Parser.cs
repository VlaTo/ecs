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
                    return new EntityKeyWildCard();
                }

                if (token.IsPathDelimiter())
                {
                    //var next = ParseDelimiter(tokenizer);
                    return ParseFromRoot(tokenizer);
                }

                if (token.IsName(out var key))
                {
                    var next = ParseDelimiter(tokenizer);
                    return new EntityKey(key, next);
                }

                if (token.IsUpLevel())
                {
                    var next = ParseDelimiter(tokenizer);
                    return new UpLevel(next);
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
                    return new PathRoot(next);
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
                    return ParsePathWildCardOrDelimiter(tokenizer);
                }

                if (token.IsName(out var key))
                {
                    var next = ParseDelimiter(tokenizer);
                    return new EntityKey(key, next);
                }

                if (token.IsUpLevel())
                {
                    var next = ParseDelimiter(tokenizer);
                    return new UpLevel(next);
                }

                throw new Exception();
            }

            private EntityPathSegment ParsePathWildCardOrDelimiter(EntityPathTokenizer tokenizer)
            {
                var token = tokenizer.GetNextToken();

                if (token.IsEndOfStream())
                {
                    EnsureTail(tokenizer);

                    return new EntityKeyWildCard();
                }

                if (token.IsWildCard())
                {
                    var next = ParseDelimiter(tokenizer);
                    return new PathWildCard(next);
                }

                throw new Exception();
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