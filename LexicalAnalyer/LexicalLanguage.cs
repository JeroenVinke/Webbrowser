using Compiler.Common;
using System;
using System.Collections.Generic;

namespace Compiler.LexicalAnalyer
{
    public static class LexicalLanguage
    {
        public static Dictionary<string, Func<string, Token>> GetLanguage()
        {
            Dictionary<string, Func<string, Token>> lexLanguage = new Dictionary<string, Func<string, Token>>();
            lexLanguage.Add(" #", (string value) => { return new WordToken { Type = TokenType.Nothing }; });
            lexLanguage.Add("\r#", (string value) => { return new WordToken { Type = TokenType.Nothing }; });
            lexLanguage.Add("\n#", (string value) => { return new WordToken { Type = TokenType.Nothing }; });
            lexLanguage.Add("\t#", (string value) => { return new WordToken { Type = TokenType.Nothing }; });
            lexLanguage.Add("<#", (string value) => { return new WordToken { Type = TokenType.LessThan }; });
            lexLanguage.Add(">#", (string value) => { return new WordToken { Type = TokenType.GreaterThan }; });
            lexLanguage.Add("\\/#", (string value) => { return new WordToken { Type = TokenType.ForwardSlash }; });
            lexLanguage.Add("([a-zA-Z])*.([a-zA-Z])*.#", (string value) =>
            {
                return new WordToken { Type = TokenType.Identifier, Lexeme = value };
            });
            lexLanguage.Add("([a-zA-Z])+([a-zA-Z0-9])*#", (string value) =>
            {
                return new WordToken { Type = TokenType.Identifier, Lexeme = value };
            });
            return lexLanguage;
        }
    }
}
