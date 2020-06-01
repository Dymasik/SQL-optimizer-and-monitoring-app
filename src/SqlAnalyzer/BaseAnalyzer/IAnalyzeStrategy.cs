using System.Collections.Generic;

namespace SqlAnalyzer {
    internal interface IAnalyzeStrategy {
        void Analyze(IEnumerable<Token> query, IList<IDictionary<QueryPart, IEnumerable<Token>>> result, IDictionary<QueryPart, IEnumerable<Token>> currParts = null);
    }

    internal enum QueryPart {
        QUERY,
        SELECT,
        FROM,
        WHERE,
        HAVING
    }
}