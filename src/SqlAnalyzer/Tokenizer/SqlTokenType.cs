using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzer {
    internal class SQLTokenType : ITokenType {
        private readonly static Dictionary<SQLTokenTypeEnum, string> _regexes = new Dictionary<SQLTokenTypeEnum, string> {
            { SQLTokenTypeEnum.KEYWORD, "\\b(?:select|from|where|group|by|order|or|and|not|exists|having|join|left|right|inner|distinct|top|count|min|max|avarage|between|as|on)\\b" },
            { SQLTokenTypeEnum.ID, "(?:[A-Za-z][A-Za-z0-9]*\\.[A-Za-z][A-Za-z0-9]*|[A-Za-z][A-Za-z0-9]*)" },
            { SQLTokenTypeEnum.REAL_NUMBER, "[0-9]+\\.[0-9]*" },
            { SQLTokenTypeEnum.NUMBER, "[0-9]+" },
            { SQLTokenTypeEnum.STRING, "'[^']*'" },
            { SQLTokenTypeEnum.OPERATOR, "(?:=|>|<|>=|<|<=|<>)" },
            { SQLTokenTypeEnum.SPACE, "\\s+" },
            { SQLTokenTypeEnum.COMMENT, "\\-\\-[^\\n\\r]*" },
            { SQLTokenTypeEnum.OPERATION, "[+\\-\\*/.=\\(\\)]" },
            { SQLTokenTypeEnum.DELIMETER, "," },
        };

        private readonly SQLTokenTypeEnum _type;

        public SQLTokenType(SQLTokenTypeEnum type) {
            _type = type;
        }
    
        public SQLTokenTypeEnum GetType() {
            return _type;
        }

        public static IEnumerable<SQLTokenTypeEnum> GetTokenTypes() {
            return _regexes.Keys; 
        }

        public static string GetTypeValue(SQLTokenTypeEnum type) {
            if (_regexes.ContainsKey(type))
                return _regexes[type];
            throw new InvalidOperationException();
        }

        public override string ToString() {
            return _type.ToString();
        }
    }

    internal enum SQLTokenTypeEnum {
        KEYWORD,
        ID,
        REAL_NUMBER,
        NUMBER,
        STRING,
        OPERATOR,
        SPACE,
        COMMENT,
        OPERATION,
        DELIMETER
    }
}
