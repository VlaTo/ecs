using System;
using System.IO;
using System.Text;

namespace ClassLibrary1.Core
{
    internal sealed class EntityPathStringTokenizer : IDisposable
    {
        private readonly TextReader reader;
        private bool isEndOfStream;
        private char? lastSymbol;

        public EntityPathStringTokenizer(TextReader reader)
        {
            this.reader = reader;
        }

        public PathToken GetNextToken()
        {
            if (isEndOfStream)
            {
                return PathToken.EndOfStream;
            }

            var done = false;
            var token = new StringBuilder();

            while (false == done)
            {
                if (false == TryReadNext(out var ch))
                {
                    isEndOfStream = true;
                    break;
                }

                switch (ch)
                {
                    case EntityPathString.PathDelimiter:
                    {
                        if (0 == token.Length)
                        {
                            return PathToken.Delimiter;
                        }

                        lastSymbol = ch;
                        done = true;

                        break;
                    }

                    case '*':
                    {
                        if (0 == token.Length)
                        {
                            return PathToken.WildCard;
                        }

                        throw new Exception();
                    }

                    default:
                    {
                        if (Char.IsLetterOrDigit(ch) || Char.IsPunctuation(ch))
                        {
                            token.Append(ch);
                            break;
                        }

                        throw new Exception();
                    }
                }
            }

            if (0 == token.Length)
            {
                return PathToken.EndOfStream;
            }

            return PathToken.String(token.ToString());
        }

        public void Dispose()
        {
            reader.Dispose();
        }

        private bool TryReadNext(out char symbol)
        {
            if (lastSymbol.HasValue)
            {
                symbol = lastSymbol.Value;
                lastSymbol = null;

                return true;
            }

            var ch = reader.Read();

            if (-1 == ch)
            {
                symbol = Char.MinValue;
                return false;
            }

            symbol = (char) ch;

            return true;
        }
    }
}