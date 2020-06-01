using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzer {
    internal class FromAnalyzeStrategy : IAnalyzeStrategy {
        public void Analyze(IEnumerable<Token> query, IList<IDictionary<QueryPart, IEnumerable<Token>>> result, IDictionary<QueryPart, IEnumerable<Token>> currParts = null) {
            query = ConditionQueryUtilities.ReplaceOrWithIn(query);
            query = ConditionQueryUtilities.ReplaceAndWithBetween(query);
            query = ConditionQueryUtilities.SplitOnUnion(QueryPart.FROM, query, result, currParts);
            if (currParts != null) {
                currParts[QueryPart.FROM] = query;
            }
        }
    }
}
