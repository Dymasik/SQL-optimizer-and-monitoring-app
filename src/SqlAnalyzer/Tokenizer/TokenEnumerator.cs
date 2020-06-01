using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SqlAnalyzer {
    internal class TokenEnumerator : IEnumerator<Token> {
        private int position = -1;
        private readonly MatchCollection _matches;
        public TokenEnumerator(MatchCollection matches) {
            _matches = matches;
        }

        public Token Current {
            get {
                if (position == -1 || position >= _matches.Count)
                    throw new InvalidOperationException();
                var text = _matches[position].Value;
                foreach (var type in SQLTokenType.GetTokenTypes()) {
                    var pattern = SQLTokenType.GetTypeValue(type);
                    var match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);
                    if (match.Success && match.Index == 0 && match.Length == text.Length) {
                        var tokenType = new SQLTokenType(type);
                        return new Token(text, tokenType, _matches[position].Index);
                    }
                }
                throw new InvalidOperationException();
            }
        }

        object IEnumerator.Current {
            get {
                if (position == -1 || position >= _matches.Count)
                    throw new InvalidOperationException();
                return (object)this.Current;
            }
        }

        public bool MoveNext() {
            if (position < _matches.Count - 1) {
                position++;
                return true;
            } else
                return false;
        }

        public void Reset() {
            position = -1;
        }
        public void Dispose() { }
    }
}