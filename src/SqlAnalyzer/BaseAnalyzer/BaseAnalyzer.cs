using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzer {
    internal class BaseAnalyzer {
        private IAnalyzeStrategy _strategy;
        private QueryPart _strategyType;

        public IAnalyzeStrategy Strategy {
            get { return _strategy; }
            private set { _strategy = value; }
        }

        public QueryPart StrategyType {
            get => _strategyType;
            set {
                switch (value) {
                    case QueryPart.QUERY:
                        Strategy = new QueryAnalyzeStrategy();
                        break;
                    case QueryPart.SELECT:
                        Strategy = new SelectAnalyzeStrategy();
                        break;
                    case QueryPart.WHERE:
                        Strategy = new WhereAnalyzeStrategy();
                        break;
                    case QueryPart.FROM:
                        Strategy = new FromAnalyzeStrategy();
                        break;
                    default:
                        throw new ArgumentException();
                }
                _strategyType = value;
            }
        }

        public void Analyze(IEnumerable<Token> tokens, IList<IDictionary<QueryPart, IEnumerable<Token>>> result, IDictionary<QueryPart, IEnumerable<Token>> currParts = null) {
            Strategy.Analyze(tokens, result, currParts);
        }
    }
}
