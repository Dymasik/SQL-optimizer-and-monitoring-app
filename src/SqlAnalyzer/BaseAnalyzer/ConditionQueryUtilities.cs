using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzer {
    internal static class ConditionQueryUtilities {
        private static IList<Token> CreateIn(string id, IEnumerable<string> values) {
            var result = new List<Token>();
            var updateTokenId = new Token(id, new SQLTokenType(SQLTokenTypeEnum.ID), -1);
            result.Add(updateTokenId);
            var updateTokenOp = new Token("IN", new SQLTokenType(SQLTokenTypeEnum.KEYWORD), -1);
            result.Add(updateTokenOp);
            var startBracketToken = new Token("(", new SQLTokenType(SQLTokenTypeEnum.OPERATION), -1);
            result.Add(startBracketToken);
            foreach (var value in values) {
                var token = new Token(value, new SQLTokenType(SQLTokenTypeEnum.NUMBER), -1);
                result.Add(token);
            }
            var endBracketToken = new Token(")", new SQLTokenType(SQLTokenTypeEnum.OPERATION), -1);
            result.Add(endBracketToken);
            return result;
        }

        private static IList<Token> CreateBetween(string id, Token lessValue, Token greatValue) {
            var result = new List<Token>();
            var updateTokenId = new Token(id, new SQLTokenType(SQLTokenTypeEnum.ID), -1);
            result.Add(updateTokenId);
            var updateTokenOp = new Token("BETWEEN", new SQLTokenType(SQLTokenTypeEnum.KEYWORD), -1);
            result.Add(updateTokenOp);
            result.Add(lessValue);
            var andToken = new Token("AND", new SQLTokenType(SQLTokenTypeEnum.KEYWORD), -1);
            result.Add(andToken);
            result.Add(greatValue);
            return result;
        }

        public static IEnumerable<Token> ReplaceOrWithIn(IEnumerable<Token> tokens) {
            var tokenArray = tokens.ToArray();
            var result = new List<Token>();
            for (var i = 0; i < tokenArray.Length; i++) {
                var token = tokenArray[i];
                if (i < tokenArray.Length - 1 && token.Type.GetType() == SQLTokenTypeEnum.ID) {
                    var opToken = tokenArray[i + 1];
                    if (opToken.Text.Equals("=")) {
                        var values = new List<string> { tokenArray[i + 2].Text };
                        var index = i + 3;
                        while (index < tokenArray.Length - 3 && tokenArray[index].Text.Equals("OR", StringComparison.OrdinalIgnoreCase)) {
                            var newToken = tokenArray[index + 1];
                            var newOp = tokenArray[index + 2].Text;
                            var newVal = tokenArray[index + 3].Text;
                            if (newToken.Type.GetType() == SQLTokenTypeEnum.ID && newToken.Text.Equals(token.Text) && newOp.Equals("=")) {
                                values.Add(newVal);
                                index += 4;
                            } else {
                                break;
                            }
                        }
                        if (values.Count > 1) {
                            result.AddRange(CreateIn(token.Text, values));
                            i = index - 1;
                        } else {
                            result.Add(token);
                        }
                    } else {
                        result.Add(token);
                    }
                } else {
                    result.Add(token);
                }
            }
            return result;
        }

        public static IEnumerable<Token> ReplaceAndWithBetween(IEnumerable<Token> tokens) {
            var tokenArray = tokens.ToArray();
            var result = new List<Token>();
            var lessCompareOps = new string[2] { "<=", "<" };
            var greatCompareOps = new string[2] { ">=", ">" };
            for (var i = 0; i < tokenArray.Length; i++) {
                var token = tokenArray[i];
                if (i < tokenArray.Length - 1 && token.Type.GetType() == SQLTokenTypeEnum.ID) {
                    var opToken = tokenArray[i + 1];
                    if (opToken.Text.Equals("<=") || (opToken.Text.Equals("<") && tokenArray[i + 2].Type.GetType() == SQLTokenTypeEnum.NUMBER)) {
                        var greatToken = tokenArray[i + 2];
                        if (opToken.Text.Equals("<")) {
                            int.TryParse(tokenArray[i + 2].Text, out int great);
                            greatToken = new Token((great - 1).ToString(), tokenArray[i + 2].Type, -1);
                        }
                        if (i < tokenArray.Length - 6 && tokenArray[i + 3].Text.Equals("and", StringComparison.OrdinalIgnoreCase)
                            && token.Text.Equals(tokenArray[i + 4].Text)) {
                            if (tokenArray[i + 5].Text.Equals(">=") || (tokenArray[i + 5].Text.Equals(">") && tokenArray[i + 6].Type.GetType() == SQLTokenTypeEnum.NUMBER)) {
                                var lessToken = tokenArray[i + 6];
                                if (tokenArray[i + 5].Text.Equals(">")) {
                                    int.TryParse(tokenArray[i + 6].Text, out int less);
                                    lessToken = new Token((less + 1).ToString(), tokenArray[i + 6].Type, -1);
                                }
                                result.AddRange(CreateBetween(token.Text, lessToken, greatToken));
                                i += 6;
                            } else {
                                result.Add(token);
                            }
                        } else {
                            result.Add(token);
                        }
                    } else if (opToken.Text.Equals(">=") || (opToken.Text.Equals(">") && tokenArray[i + 2].Type.GetType() == SQLTokenTypeEnum.NUMBER)) {
                        var lessToken = tokenArray[i + 2];
                        if (opToken.Text.Equals(">")) {
                            int.TryParse(tokenArray[i + 2].Text, out int less);
                            lessToken = new Token((less + 1).ToString(), tokenArray[i + 2].Type, -1);
                        }
                        if (i < tokenArray.Length - 6 && tokenArray[i + 3].Text.Equals("and", StringComparison.OrdinalIgnoreCase)
                            && token.Text.Equals(tokenArray[i + 4].Text)) {
                            if (tokenArray[i + 5].Text.Equals("<=") || (tokenArray[i + 5].Text.Equals("<") && tokenArray[i + 6].Type.GetType() == SQLTokenTypeEnum.NUMBER)) {
                                var greatToken = tokenArray[i + 6];
                                if (tokenArray[i + 5].Text.Equals("<")) {
                                    int.TryParse(tokenArray[i + 6].Text, out int great);
                                    greatToken = new Token((great - 1).ToString(), tokenArray[i + 6].Type, -1);
                                }
                                result.AddRange(CreateBetween(token.Text, lessToken, greatToken));
                                i += 6;
                            } else {
                                result.Add(token);
                            }
                        } else {
                            result.Add(token);
                        }
                    } else {
                        result.Add(token);
                    }
                } else {
                    result.Add(token);
                }
            }
            return result;
        }

        public static IEnumerable<Token> SplitOnUnion(QueryPart part, IEnumerable<Token> tokens, IList<IDictionary<QueryPart, IEnumerable<Token>>> result, IDictionary<QueryPart, IEnumerable<Token>> currParts) {
            var tokenArray = tokens.ToArray();
            var resultTokens = new List<Token>();
            var analyzer = new BaseAnalyzer();
            for (var i = 0; i < tokenArray.Length; i++) {
                var token = tokenArray[i];
                if (token.Type.GetType() == SQLTokenTypeEnum.KEYWORD && token.Text.Equals("or", StringComparison.OrdinalIgnoreCase)) {
                    var additionalArray = new Token[resultTokens.Count];
                    resultTokens.CopyTo(additionalArray);
                    var newTokens = additionalArray
                        .Reverse()
                        .SkipWhile(t => !t.Text.Equals("or", StringComparison.OrdinalIgnoreCase)
                            && !t.Text.Equals("and", StringComparison.OrdinalIgnoreCase)
                            && !t.Text.Equals("on", StringComparison.OrdinalIgnoreCase))
                        .Reverse()
                        .ToList();
                    newTokens.AddRange(tokenArray.Skip(i + 1));
                    var newParts = new Dictionary<QueryPart, IEnumerable<Token>>(currParts);
                    newParts[part] = newTokens;
                    analyzer.StrategyType = QueryPart.QUERY;
                    analyzer.Analyze(newTokens, result, newParts);
                    i = tokenArray.Skip(i + 1)
                        .SkipWhile(t => !t.Text.Equals("or", StringComparison.OrdinalIgnoreCase)
                            || !t.Text.Equals("and", StringComparison.OrdinalIgnoreCase)
                            || !t.Text.Equals("on", StringComparison.OrdinalIgnoreCase))
                        .Select((t, index) => index).FirstOrDefault();
                    if (i == 0)
                        break;
                } else {
                    resultTokens.Add(token);
                }
            }
            return resultTokens;
        }
    }
}
