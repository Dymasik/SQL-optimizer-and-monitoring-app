using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SqlAnalyzer {
    internal class RegexTokenizer : IEnumerable<Token> {
        private readonly string _content;
        private readonly MatchCollection _matches;

        public RegexTokenizer(string content) {
            _content = content;
            
            var regexList = new List<string>();
            var types = SQLTokenType.GetTokenTypes().ToList();
            for (int i = 0; i < types.Count; i++) {
                regexList.Add("(?<g" + i + ">" + SQLTokenType.GetTypeValue(types[i]) + ")");
            }
            var regex = string.Join("|", regexList);
            _matches = Regex.Matches(content, regex, RegexOptions.IgnoreCase);
        }

        public IEnumerator GetEnumerator() {
            return new TokenEnumerator(_matches);
        }

        IEnumerator<Token> IEnumerable<Token>.GetEnumerator() {
            return new TokenEnumerator(_matches);
        }
    }
}
