using SqlAnalyzer.SelectElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzer {
    public class SelectOptimizer {

        private SelectBuilder BuidSelect(IDictionary<QueryPart, IEnumerable<Token>> parts, SelectBuilder sb = null) {
            sb = sb ?? new SelectBuilder();
            foreach (var column in Column.SplitByDelimeter(parts[QueryPart.SELECT])) {
                sb.Column(column);
            }
            sb.From(Table.GetRootTable(parts[QueryPart.FROM]));
            foreach (var join in Table.SplitByJoin(parts[QueryPart.FROM])) {
                sb.Join(join);
            }
            sb.Where(parts[QueryPart.WHERE]);
            return sb;
        }

        public virtual string GetOptimizedQuery(string select) {
            var tokenizer = new RegexTokenizer(select);
            var analyzer = new BaseAnalyzer();
            var res = new List<IDictionary<QueryPart, IEnumerable<Token>>>();
            analyzer.StrategyType = QueryPart.QUERY;
            analyzer.Analyze(tokenizer, res);
            var resultSelect = new SelectBuilder();
            foreach (var tokenQuery in res) {
                if (resultSelect.IsEmpty) {
                    BuidSelect(tokenQuery, resultSelect);
                } else {
                    resultSelect.Union(BuidSelect(tokenQuery));
                }
            }
            return resultSelect.ToString();
        }
    }
}
