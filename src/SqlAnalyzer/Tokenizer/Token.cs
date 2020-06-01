using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzer {
    internal class Token {
        public string Text { get; }

        public ITokenType Type { get; }

        public int Start { get; }

        public Token(string text, ITokenType type, int start) {
            Text = text;
            Type = type;
            Start = start;
        }
    }

    internal interface ITokenType {
        SQLTokenTypeEnum GetType();
    }
}
