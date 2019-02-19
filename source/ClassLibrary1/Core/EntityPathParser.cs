using System;
using System.IO;
using ClassLibrary1.Core.Extensions;

namespace ClassLibrary1.Core
{
    internal sealed class EntityPathParser
    {
        public EntityPathStringSegment Parse(string path)
        {
            var reader = new StringReader(path);

            using (var tokenizer = new EntityPathStringTokenizer(reader))
            {
                return ParseFromBegin(tokenizer);
            }
        }

        private EntityPathStringSegment ParseFromBegin(EntityPathStringTokenizer tokenizer)
        {
            var token = tokenizer.GetNextToken();

            if (token.IsEndOfStream())
            {
                return null;
            }

            if (token.IsWildCard())
            {
                EnsureTail(tokenizer);
                return new WildCardEntityPathStringSegment();
            }

            if (token.IsPathDelimiter())
            {
                //var next = ParseWildCardOrString(tokenizer);
                //return new RootEntityPathStringSegment(next);
                return ParseFromRoot(tokenizer);
            }

            if (token.IsSegment(out var segment))
            {
                var next = ParseDelimiter(tokenizer);
                return new StringEntityPathStringSegment(segment, next);
            }

            throw new Exception();
        }

        private EntityPathStringSegment ParseFromRoot(EntityPathStringTokenizer tokenizer)
        {
            var token = tokenizer.GetNextToken();

            if (token.IsEndOfStream())
            {
                return null;
            }

            if (token.IsPathDelimiter())
            {
                var next = ParseWildCardOrString(tokenizer);
                return new RootEntityPathStringSegment(next);
            }

            throw new Exception();
        }

        private EntityPathStringSegment ParseWildCardOrString(EntityPathStringTokenizer tokenizer)
        {
            var token = tokenizer.GetNextToken();

            if (token.IsEndOfStream())
            {
                return null;
            }

            if (token.IsWildCard())
            {
                EnsureTail(tokenizer);
                return new WildCardEntityPathStringSegment();
            }

            if (token.IsSegment(out var segment))
            {
                var next = ParseDelimiter(tokenizer);
                return new StringEntityPathStringSegment(segment, next);
            }

            throw new Exception();
        }

        private EntityPathStringSegment ParseDelimiter(EntityPathStringTokenizer tokenizer)
        {
            var token = tokenizer.GetNextToken();

            if (token.IsEndOfStream())
            {
                return null;
            }

            if (token.IsPathDelimiter())
            {
                return ParseWildCardOrString(tokenizer);
            }

            throw new Exception();
        }

        private static void EnsureTail(EntityPathStringTokenizer tokenizer)
        {
            var token = tokenizer.GetNextToken();

            if (false == token.IsEndOfStream())
            {
                throw new Exception();
            }
        }
    }
}