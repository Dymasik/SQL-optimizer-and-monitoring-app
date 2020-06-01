using System.Collections.Generic;

namespace SqlAnalyzer.SelectElements {
    internal class BaseCondition {
        private Column _left;
        private bool _isExist;
        private string _baseOperator;
        private string _value;

        public bool IsEmpty { get => (!_isExist && _left == null) || (_isExist); }

        public static BaseCondition Exists(IEnumerable<Token> select) {
            var condition = new BaseCondition();
            // condition._select = select;
            condition._isExist = true;
            return condition;
        }

        public static BaseCondition BasicCondition(Column column, string baseOperator, string values) {
            var condition = new BaseCondition();
            condition._left = column;
            condition._value = values;
            condition._baseOperator = baseOperator;
            return condition;
        }

        public override string ToString() {
            if (!string.IsNullOrEmpty(_baseOperator))
                return $"{_left} {_baseOperator} {_value}";
            return $"EXISTS {_value}";
        }
    }
}