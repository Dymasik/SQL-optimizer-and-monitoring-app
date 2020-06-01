using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzer {
    internal class QueryAnalyzeStrategy : IAnalyzeStrategy {

        private int _currentPosition = 0;

        private IEnumerable<Token> GetSelect(IEnumerable<Token> query) {
            var queryList = query.ToList();
            var token = queryList[_currentPosition];
            while (token.Type.GetType() != SQLTokenTypeEnum.KEYWORD || !token.Text.Equals("select", StringComparison.OrdinalIgnoreCase)) {
                _currentPosition++;
                if (_currentPosition >= queryList.Count)
                    break;
                token = queryList[_currentPosition];
            }
            var select = new List<Token>();
            var bracket = 0;
            _currentPosition++;
            if (_currentPosition >= queryList.Count)
                return select;
            token = queryList[_currentPosition];
            do {
                if (token.Type.GetType() != SQLTokenTypeEnum.SPACE && token.Type.GetType() != SQLTokenTypeEnum.COMMENT)
                    select.Add(token);
                if (token.Type.GetType() == SQLTokenTypeEnum.OPERATION && token.Text.Equals("("))
                    bracket++;
                if (token.Type.GetType() == SQLTokenTypeEnum.OPERATION && token.Text.Equals(")"))
                    bracket--;
                ++_currentPosition;
                if (_currentPosition >= queryList.Count)
                    break;
                token = queryList[_currentPosition];
            } while (bracket != 0 || token.Type.GetType() != SQLTokenTypeEnum.KEYWORD || !token.Text.Equals("from", StringComparison.OrdinalIgnoreCase));
            return select;
        }

        private IEnumerable<Token> GetFrom(IEnumerable<Token> query) {
            var queryList = query.ToList();
            var from = new List<Token>();
            if (_currentPosition >= queryList.Count)
                return from;
            var token = queryList[_currentPosition];
            while (token.Type.GetType() != SQLTokenTypeEnum.KEYWORD || !token.Text.Equals("from", StringComparison.OrdinalIgnoreCase)) {
                _currentPosition++;
                if (_currentPosition >= queryList.Count)
                    break;
                token = queryList[_currentPosition];
            }
            var bracket = 0;
            _currentPosition++;
            if (_currentPosition >= queryList.Count)
                return from;
            token = queryList[_currentPosition];
            do {
                if (token.Type.GetType() != SQLTokenTypeEnum.SPACE && token.Type.GetType() != SQLTokenTypeEnum.COMMENT)
                    from.Add(token);
                if (token.Type.GetType() == SQLTokenTypeEnum.OPERATION && token.Text.Equals("("))
                    bracket++;
                if (token.Type.GetType() == SQLTokenTypeEnum.OPERATION && token.Text.Equals(")"))
                    bracket--;
                ++_currentPosition;
                if (_currentPosition >= queryList.Count)
                    break;
                token = queryList[_currentPosition];
            } while (bracket != 0 || token.Type.GetType() != SQLTokenTypeEnum.KEYWORD
                || !(token.Text.Equals("where", StringComparison.OrdinalIgnoreCase)
                || token.Text.Equals("group", StringComparison.OrdinalIgnoreCase)
                || token.Text.Equals("order", StringComparison.OrdinalIgnoreCase)));
            return from;
        }

        private IEnumerable<Token> GetWhere(IEnumerable<Token> query) {
            var queryList = query.ToList();
            var where = new List<Token>();
            if (_currentPosition >= queryList.Count)
                return where;
            var token = queryList[_currentPosition];
            while (token.Type.GetType() != SQLTokenTypeEnum.KEYWORD || !token.Text.Equals("where", StringComparison.OrdinalIgnoreCase)) {
                _currentPosition++;
                if (_currentPosition >= queryList.Count)
                    break;
                token = queryList[_currentPosition];
            }
            var bracket = 0;
            _currentPosition++;
            if (_currentPosition >= queryList.Count)
                return where;
            token = queryList[_currentPosition];
            do {
                if (token.Type.GetType() != SQLTokenTypeEnum.SPACE && token.Type.GetType() != SQLTokenTypeEnum.COMMENT)
                    where.Add(token);
                if (token.Type.GetType() == SQLTokenTypeEnum.OPERATION && token.Text.Equals("("))
                    bracket++;
                if (token.Type.GetType() == SQLTokenTypeEnum.OPERATION && token.Text.Equals(")"))
                    bracket--;
                ++_currentPosition;
                if (_currentPosition >= queryList.Count)
                    break;
                token = queryList[_currentPosition];
            } while (bracket != 0 || token.Type.GetType() != SQLTokenTypeEnum.KEYWORD
                || !(token.Text.Equals("group", StringComparison.OrdinalIgnoreCase)
                || token.Text.Equals("order", StringComparison.OrdinalIgnoreCase)));
            return where;
        }

        private IEnumerable<Token> GetHaving(IEnumerable<Token> query) {
            var queryList = query.ToList();
            var having = new List<Token>();
            if (_currentPosition >= queryList.Count)
                return having;
            var token = queryList[_currentPosition];
            while (token.Type.GetType() != SQLTokenTypeEnum.KEYWORD || !token.Text.Equals("having", StringComparison.OrdinalIgnoreCase)) {
                _currentPosition++;
                if (_currentPosition >= queryList.Count)
                    break;
                token = queryList[_currentPosition];
            }
            var bracket = 0;
            if (_currentPosition >= queryList.Count)
                return having;
            do {
                if (token.Type.GetType() != SQLTokenTypeEnum.SPACE && token.Type.GetType() != SQLTokenTypeEnum.COMMENT)
                    having.Add(token);
                if (token.Type.GetType() == SQLTokenTypeEnum.OPERATION && token.Text.Equals("("))
                    bracket++;
                if (token.Type.GetType() == SQLTokenTypeEnum.OPERATION && token.Text.Equals(")"))
                    bracket--;
                ++_currentPosition;
                if (_currentPosition >= queryList.Count)
                    break;
                token = queryList[_currentPosition];
            } while (bracket != 0 || token.Type.GetType() != SQLTokenTypeEnum.KEYWORD
                || !token.Text.Equals("order", StringComparison.OrdinalIgnoreCase));
            return having;
        }

        public void Analyze(IEnumerable<Token> query, IList<IDictionary<QueryPart, IEnumerable<Token>>> result, IDictionary<QueryPart, IEnumerable<Token>> currParts = null) {
            currParts = currParts ?? new Dictionary<QueryPart, IEnumerable<Token>> {
                { QueryPart.SELECT, GetSelect(query) },
                { QueryPart.FROM, GetFrom(query) },
                { QueryPart.WHERE, GetWhere(query) }
            };
            var analyzer = new BaseAnalyzer();
            var keys = new QueryPart[currParts.Count];
            currParts.Keys.CopyTo(keys, 0);
            foreach (var key in keys) {
                analyzer.StrategyType = key;
                analyzer.Analyze(currParts[key], result, currParts);
            }
            result.Add(currParts);
        }
    }

    
}
