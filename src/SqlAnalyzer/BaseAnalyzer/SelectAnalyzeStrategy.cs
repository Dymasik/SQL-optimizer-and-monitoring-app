using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlAnalyzer {
    internal class SelectAnalyzeStrategy : IAnalyzeStrategy {

        protected virtual IEnumerable<Token> ReplaceCount(IEnumerable<Token> tokens) {
            var prevCount = false;
            for (var i = 0; i < tokens.Count(); i++) {
                var token = tokens.Skip(i).Take(1).FirstOrDefault();
                if (token.Type.GetType() == SQLTokenTypeEnum.KEYWORD
                    && token.Text.Equals("count", StringComparison.OrdinalIgnoreCase)) {
                    prevCount = true;
                }
                if (prevCount && (token.Type.GetType() == SQLTokenTypeEnum.NUMBER
                    || token.Type.GetType() == SQLTokenTypeEnum.REAL_NUMBER
                    || token.Type.GetType() == SQLTokenTypeEnum.STRING
                    || token.Type.GetType() == SQLTokenTypeEnum.ID)) {
                    var tokenType = new SQLTokenType(SQLTokenTypeEnum.NUMBER);
                    token = new Token("1", tokenType, token.Start);
                    prevCount = false;
                }
                yield return token;
            }
        } 

        public void Analyze(IEnumerable<Token> query, IList<IDictionary<QueryPart, IEnumerable<Token>>> result, IDictionary<QueryPart, IEnumerable<Token>> currParts = null) {
            query = ReplaceCount(query);
            if (currParts != null) {
                currParts[QueryPart.SELECT] = query;
            }
        }
    }
}
