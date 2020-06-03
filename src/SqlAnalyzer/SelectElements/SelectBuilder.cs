using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlAnalyzer.SelectElements;

namespace SqlAnalyzer {
    internal class SelectBuilder {
        private readonly IList<Column> _columns;
        private Table _from;
        private Condition _where;
        private IList<SelectBuilder> _union;

        public SelectBuilder() {
            _columns = new List<Column>();
            _union = new List<SelectBuilder>();
        }

        public bool IsEmpty { get => !_columns.Any(); }

        private Token GetFirstOperator(IEnumerable<Token> tokens) {
            var tokenArray = tokens.ToArray();
            var prevBetween = false;
            for (var i = 0; i < tokenArray.Length; i++) {
                var token = tokenArray[i];
                if (token.Text.Equals("BETWEEN", StringComparison.OrdinalIgnoreCase))
                    prevBetween = true;
                if (token.Text.Equals("AND", StringComparison.OrdinalIgnoreCase)) {
                    if (prevBetween)
                        prevBetween = false;
                    else
                        return token;
                }
                if (token.Text.Equals("OR", StringComparison.OrdinalIgnoreCase))
                    return token;
            }
            return null;
        }

        private Condition GetCondition(IEnumerable<Token> tokens) {
            var firstToken = tokens.FirstOrDefault();
            Condition condition;
            if (firstToken == null)
                return null;
            if (firstToken.Type.GetType() == SQLTokenTypeEnum.OPERATION
                && firstToken.Text.Equals("(")) {
                var leftConditionTokens = tokens.Skip(1).TakeWhile(t => !(t.Type.GetType() == SQLTokenTypeEnum.OPERATION
                    && t.Text.Equals(")")));
                var rightConditionTokens = tokens.SkipWhile(t => !(t.Type.GetType() == SQLTokenTypeEnum.OPERATION
                    && t.Text.Equals(")"))).Skip(2);
                var operatorToken = tokens.SkipWhile(t => !(t.Type.GetType() == SQLTokenTypeEnum.OPERATION
                    && t.Text.Equals(")"))).Skip(1).FirstOrDefault();
                if (operatorToken == null) {
                    condition = new Condition(GetCondition(leftConditionTokens));
                }
                var type = (LogicalOperatorType)Enum.Parse(typeof(LogicalOperatorType), operatorToken.Text.ToUpper());
                condition = new Condition(GetCondition(leftConditionTokens), type, GetCondition(rightConditionTokens));
            } else {
                var operatorToken = GetFirstOperator(tokens);
                if (operatorToken == null) {
                    var isExists = firstToken?.Text.Equals("EXISTS", StringComparison.OrdinalIgnoreCase);
                    if (isExists.HasValue && isExists.Value) {
                        var exist = tokens.Skip(1);
                        condition = new Condition(BaseCondition.Exists(exist));
                    } else {
                        var column = GetColumn(tokens);
                        var op = GetOperator(tokens);
                        var value = GetValue(tokens);
                        condition = new Condition(BaseCondition.BasicCondition(column, op, value));
                    }
                } else {
                    var left = tokens.TakeWhile(t => !(t.Type.GetType() == SQLTokenTypeEnum.KEYWORD
                        && (t.Text.Equals("AND", StringComparison.OrdinalIgnoreCase) || t.Text.Equals("OR", StringComparison.OrdinalIgnoreCase))));
                    var right = tokens.SkipWhile(t => !(t.Type.GetType() == SQLTokenTypeEnum.KEYWORD
                        && (t.Text.Equals("AND", StringComparison.OrdinalIgnoreCase) || t.Text.Equals("OR", StringComparison.OrdinalIgnoreCase))))
                        .Skip(1);
                    var type = (LogicalOperatorType)Enum.Parse(typeof(LogicalOperatorType), operatorToken.Text.ToUpper());
                    condition = new Condition(GetCondition(left), type, GetCondition(right));
                }
            }
            return condition;
        }

        private string GetOperator(IEnumerable<Token> tokens) {
            var column = GetColumn(tokens);
            if (!string.IsNullOrWhiteSpace(column.Alias)) {
                var opToken = tokens.SkipWhile(t => !t.Text.Equals(column.Alias)).Skip(1).FirstOrDefault();
                if (opToken != null) {
                    return opToken.Text;
                }
                throw new NotImplementedException();
            }
            if (column.Type == ColumnType.NONE) {
                var opToken = tokens.Skip(1).FirstOrDefault();
                if (opToken != null) {
                    return opToken.Text;
                }
                throw new NotImplementedException();
            }
            var token = tokens.SkipWhile(t => !t.Text.Equals(")")).Skip(1).FirstOrDefault();
            if (token != null) {
                return token.Text;
            }
            throw new NotImplementedException();
        }

        private string GetValue(IEnumerable<Token> tokens) {
            var op = GetOperator(tokens);
            var valueTokens = tokens.SkipWhile(t => !t.Text.Equals(op)).Skip(1);
            if (op.Equals("IN", StringComparison.OrdinalIgnoreCase)) {
                valueTokens = valueTokens.Where(t => !(t.Text.Equals("(") || t.Text.Equals(")")));
                return $"({string.Join(",", valueTokens.Select(t => t.Text))})";
            }
            if (op.Equals("BETWEEN", StringComparison.OrdinalIgnoreCase)) {
                return $"{string.Join(" ", valueTokens.Select(t => t.Text))}";
            }
            return valueTokens.FirstOrDefault()?.Text;
        }

        private Column GetColumn(IEnumerable<Token> tokens) {
            var hasBracket = tokens
                .TakeWhile(t => !(t.Type.GetType() == SQLTokenTypeEnum.NUMBER
                    || t.Type.GetType() == SQLTokenTypeEnum.REAL_NUMBER
                    || t.Type.GetType() == SQLTokenTypeEnum.STRING
                    || t.Type.GetType() == SQLTokenTypeEnum.ID))
                .Where(token => token.Type.GetType() == SQLTokenTypeEnum.OPERATION && token.Text.Equals("("))
                .Any();
            Column column = null;
            var name = tokens.Where(t => t.Type.GetType() == SQLTokenTypeEnum.NUMBER
                || t.Type.GetType() == SQLTokenTypeEnum.REAL_NUMBER
                || t.Type.GetType() == SQLTokenTypeEnum.STRING
                || t.Type.GetType() == SQLTokenTypeEnum.ID).FirstOrDefault()?.Text;
            var alias = string.Empty;
            if (hasBracket) {
                var type = (ColumnType)Enum.Parse(typeof(ColumnType), tokens.Where(
                    t => t.Type.GetType() == SQLTokenTypeEnum.KEYWORD)
                    .FirstOrDefault().Text.ToUpper());
                var tokAlias = tokens.SkipWhile(t => !t.Text.Equals(")"))
                    .Skip(1)
                    .SkipWhile(t => t.Text.Equals("AS", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
                if (tokAlias != null && tokAlias.Type.GetType() == SQLTokenTypeEnum.ID)
                    alias = tokAlias.Text;
                column = new Column(name, type, alias);
            } else {
                var tokAlias = tokens.SkipWhile(t => !t.Text.Equals(name))
                    .Skip(1)
                    .SkipWhile(t => t.Text.Equals("AS", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
                if (tokAlias != null && tokAlias.Type.GetType() == SQLTokenTypeEnum.ID)
                    alias = tokAlias.Text;
                column = new Column(name, ColumnType.NONE, alias);
            }
            return column;
        }

        public SelectBuilder Column(IEnumerable<Token> tokens) {
            _columns.Add(GetColumn(tokens));
            return this;
        }

        public SelectBuilder From(IEnumerable<Token> tokens) {
            if (!tokens.Any())
                return this;
            var name = tokens.Where(t => t.Type.GetType() == SQLTokenTypeEnum.ID).FirstOrDefault().Text;
            var alias = tokens.Where(t => t.Type.GetType() == SQLTokenTypeEnum.ID).Skip(1).FirstOrDefault()?.Text;
            _from = new Table(name, alias);
            return this;
        }

        public SelectBuilder Join(IEnumerable<Token> tokens) {
            var type = (JoinType)Enum.Parse(typeof(JoinType), tokens.Where(
                   t => t.Type.GetType() == SQLTokenTypeEnum.KEYWORD)
                   .FirstOrDefault().Text.ToUpper());
            var name = tokens.Where(t => t.Type.GetType() == SQLTokenTypeEnum.ID).FirstOrDefault().Text;
            var alias = tokens.Where(t => t.Type.GetType() == SQLTokenTypeEnum.ID).Skip(1).FirstOrDefault().Text;
            var glbAlias = tokens.Reverse().Where(t => t.Type.GetType() == SQLTokenTypeEnum.ID).FirstOrDefault().Text;
            var hasGlbAlias = tokens.Reverse().Where(t => t.Type.GetType() == SQLTokenTypeEnum.OPERATION
                && t.Text == ")").Any();
            var condTokens = tokens.SkipWhile(t => !(t.Text.Equals("ON", StringComparison.OrdinalIgnoreCase)
                && t.Type.GetType() == SQLTokenTypeEnum.KEYWORD))
                .Skip(1);
            _from = new Table(type, _from, new Table(name, alias), GetCondition(condTokens), hasGlbAlias ? glbAlias : null);
            return this;
        }

        public SelectBuilder Where(IEnumerable<Token> tokens) {
            _where = GetCondition(tokens);
            return this;
        }

        public SelectBuilder Union(SelectBuilder sb) {
            _union.Add(sb);
            return this;
        }

        public override string ToString() {
            var sb = new StringBuilder("SELECT ");
            for (int i = 0; i < _columns.Count; i++) {
                if (i < _columns.Count - 1)
                    sb.Append($"{_columns[i]},\n");
                else
                    sb.Append($"{_columns[i]}\n");
            }
            if (_from != null)
                sb.Append($"FROM {_from}\n");
            if (_where != null && !_where.IsEmpty)
                sb.Append($"WHERE {_where}\n");
            foreach (var query in _union) {
                sb.Append("UNION\n");
                sb.Append(query);
            }
            return sb.ToString();
        }
    }
}
