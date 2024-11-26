using Compiler.Common;
using System;
using System.Collections.Generic;

namespace CSSLexicalAnalyzer
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
            lexLanguage.Add(":#", (string value) => { return new WordToken { Type = TokenType.Colon }; });
            lexLanguage.Add(";#", (string value) => { return new WordToken { Type = TokenType.Semicolon }; });
            lexLanguage.Add("{#", (string value) => { return new WordToken { Type = TokenType.BracketOpen }; });
            lexLanguage.Add("}#", (string value) => { return new WordToken { Type = TokenType.BracketClosed }; });
            lexLanguage.Add("([a-zA-Z0-9]|-|.|,| |\\#|%)+#", (string value) =>
            {
                return new WordToken { Type = TokenType.Identifier, Lexeme = value };
            });
            return lexLanguage;
        }
    }
}
