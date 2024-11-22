using Compiler.Common;
using System;
using System.Collections.Generic;

namespace HTMLLexicalAnalyer
{
    public static class LexicalLanguage
    {
        public static Dictionary<string, Func<string, Token>> GetLanguage()
        {
            Dictionary<string, Func<string, Token>> lexLanguage = new Dictionary<string, Func<string, Token>>();
            lexLanguage.Add("( )+#", (string value) => { return new WordToken { Type = TokenType.Nothing }; });
            lexLanguage.Add("\r#", (string value) => { return new WordToken { Type = TokenType.Nothing }; });
            lexLanguage.Add("\n#", (string value) => { return new WordToken { Type = TokenType.Nothing }; });
            lexLanguage.Add("\t#", (string value) => { return new WordToken { Type = TokenType.Nothing }; });
            lexLanguage.Add("<([a-zA-Z0-9])+([a-zA-Z0-9]|=|\"| |:|.|/|-|%)*( )*/>#", (string value) =>
            {
                return new WordToken { Type = TokenType.SelfClosingTag, Lexeme = value };
            });
            lexLanguage.Add("<([a-zA-Z0-9])+([a-zA-Z0-9]|=|\"| |:|.|/|-|%)*>#", (string value) =>
            {
                return new WordToken { Type = TokenType.OpenTag, Lexeme = value };
            });
            lexLanguage.Add("</([a-zA-Z0-9])*>#", (string value) =>
            {
                return new WordToken { Type = TokenType.CloseTag, Lexeme = value };
            });
            lexLanguage.Add("([a-zA-Z0-9]| |:|.|/|%)+#", (string value) =>
            {
                return new WordToken { Type = TokenType.Identifier, Lexeme = value };
            });
            return lexLanguage;
        }
    }
}
