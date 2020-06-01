namespace SqlAnalyzer.SelectElements {
    internal class Condition {
        private BaseCondition _baseCondition;


        private Condition _leftCondition;
        private Condition _rightCondition;
        private LogicalOperatorType _logicalOperator;

        public Condition(BaseCondition baseCondition) {
            _baseCondition = baseCondition;
        }

        public Condition(Condition left) {
            _leftCondition = left;
        }

        public Condition(Condition left, LogicalOperatorType type, Condition right) {
            _leftCondition = left;
            _logicalOperator = type;
            _rightCondition = right;
        }

        public bool IsEmpty { get => (_leftCondition == null || _leftCondition.IsEmpty) && (_baseCondition == null || _baseCondition.IsEmpty); }

        private string BaseToStringImpl() {
            return _baseCondition.ToString();
        }

        public override string ToString() {
            if (_baseCondition != null)
                return BaseToStringImpl();
            if (_rightCondition == null)
                return _leftCondition.ToString();
            return $"{_leftCondition} {_logicalOperator} {_rightCondition}";
        }
    }

    internal enum LogicalOperatorType {
        AND,
        OR
    }
}