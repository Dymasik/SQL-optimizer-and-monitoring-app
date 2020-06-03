using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzer.SelectElements {
    internal class Table {
        private readonly Table _leftTable;
        private readonly Table _rightTable;
        private readonly JoinType _joinType;
        private readonly Condition _condition;
        private readonly string _name;
        private readonly string _alias;

        public Table(string name, string alias = null) {
            _name = name;
            _alias = alias;
        }

        public Table(JoinType joinType, Table leftTable, Table rightTable, Condition condition, string alias = null) {
            _joinType = joinType;
            _leftTable = leftTable;
            _rightTable = rightTable;
            _condition = condition;
            _alias = alias;
        }

        private string ToStringBaseImpl() {
            return _name + (!string.IsNullOrWhiteSpace(_alias) ? $" AS {_alias}" : "");
        }

        public override string ToString() {
            if (!string.IsNullOrWhiteSpace(_name))
                return ToStringBaseImpl();
            return $"{_leftTable.ToString()} {_joinType} JOIN {_rightTable.ToString()} ON {_condition.ToString()}";
        }

        public static IEnumerable<IEnumerable<Token>> SplitByJoin(IEnumerable<Token> tokens) {
            var list = new List<IEnumerable<Token>>();
            tokens = tokens.SkipWhile(token => !(token.Type.GetType() == SQLTokenTypeEnum.KEYWORD &&
                Enum.GetNames(typeof(JoinType)).Contains(token.Text.ToUpper())));
            var join = tokens.TakeWhile((token, i) => !(token.Type.GetType() == SQLTokenTypeEnum.KEYWORD &&
                Enum.GetNames(typeof(JoinType)).Contains(token.Text) && i != 0));
            while (join.Count() > 0) {
                list.Add(join);
                tokens = tokens.Skip(1).SkipWhile(token => !(token.Type.GetType() == SQLTokenTypeEnum.KEYWORD &&
                    Enum.GetNames(typeof(JoinType)).Contains(token.Text.ToUpper())));
                join = tokens.TakeWhile((token, i) => !(token.Type.GetType() == SQLTokenTypeEnum.KEYWORD &&
                    Enum.GetNames(typeof(JoinType)).Contains(token.Text.ToUpper()) && i != 0));
            }
            return list;
        }

        public static IEnumerable<Token> GetRootTable(IEnumerable<Token> tokens) {
            return tokens.TakeWhile(token => !(token.Type.GetType() == SQLTokenTypeEnum.KEYWORD &&
                Enum.GetNames(typeof(JoinType)).Contains(token.Text.ToUpper())));
        }
    }

    internal enum JoinType {
        LEFT,
        RIGHT,
        INNER,
        CROSS
    }
}
