using System.Collections.Generic;
using System.Linq;

namespace SqlAnalyzer.SelectElements {
    internal class Column {
        private string _name;
        public string _alias;
        private ColumnType _type;

        public Column(string name, ColumnType type, string alias = null) {
            _name = name;
            _type = type;
            _alias = alias;
        }

        public string Alias { get => _alias; }
        public string Name { get => _name; }
        public ColumnType Type { get =>  _type; }


        public override string ToString() {
            var tpl = $"{{0}}{_name}{{1}}{(!string.IsNullOrWhiteSpace(_alias) ? " as " + _alias : "")}";
            if (_type == ColumnType.NONE)
                return string.Format(tpl, "", "");
            else {
                var type = _type.ToString();
                return string.Format(tpl, type + "(", ")");
            }
        }

        public static IEnumerable<IEnumerable<Token>> SplitByDelimeter(IEnumerable<Token> tokens) {
            var list = new List<IEnumerable<Token>>();
            var column = tokens.TakeWhile(token => token.Type.GetType() != SQLTokenTypeEnum.DELIMETER);
            while (column.Count() > 0) {
                list.Add(column);
                tokens = tokens.SkipWhile(token => token.Type.GetType() != SQLTokenTypeEnum.DELIMETER).Skip(1);
                column = tokens.TakeWhile(token => token.Type.GetType() != SQLTokenTypeEnum.DELIMETER);
            }
            return list;
        }
    }

    internal enum ColumnType {
        NONE,
        COUNT,
        SUM,
        MIN,
        MAX,
        AVARAGE
    }
}
