﻿#region Copyright
// ****************************************************************************
// <copyright file="XmlTokenizer.cs">
// Copyright © Vyacheslav Volkov 2012-2014
// </copyright>
// ****************************************************************************
// <author>Vyacheslav Volkov</author>
// <email>vvs0205@outlook.com</email>
// <project>MugenMvvmToolkit</project>
// <web>https://github.com/MugenMvvmToolkit/MugenMvvmToolkit</web>
// <license>
// See license.txt in this solution or http://opensource.org/licenses/MS-PL
// </license>
// ****************************************************************************
#endregion
using System;
using MugenMvvmToolkit.Binding.Models;

namespace MugenMvvmToolkit.Binding.Parse
{
    internal sealed class XmlTokenizer : Tokenizer
    {
        #region Constructors

        public XmlTokenizer()
            : base(false)
        {
        }

        #endregion

        #region Overrides of Tokenizer

        protected override TokenType NextTokenInternal(bool ignoreWhitespace)
        {
            if (ignoreWhitespace)
            {
                while (Char.IsWhiteSpace(CurrentChar))
                    NextChar();
                FirstCharPosition = Position;
            }
            else
                FirstCharPosition = Position;

            TokenType t = TokenType.Unknown;
            int tokenPos = Position;
            switch (CurrentChar)
            {
                case '<':
                    NextChar();
                    if (CurrentChar == '!')
                    {
                        NextChar();
                        if (CurrentChar == '-')
                        {
                            NextChar();
                            if (CurrentChar == '-')
                            {
                                NextChar();
                                t = XmlTokens.StartComment;
                            }
                        }
                    }
                    else if (CurrentChar == '/')
                    {
                        NextChar();
                        t = XmlTokens.ComplexCloseElement;
                    }
                    else
                        t = TokenType.LessThan;
                    break;
                case '/':
                    NextChar();
                    if (CurrentChar == '>')
                    {
                        NextChar();
                        t = XmlTokens.CloseElement;
                    }
                    break;
                case '-':
                    NextChar();
                    if (CurrentChar == '-')
                    {
                        NextChar();
                        if (CurrentChar == '>')
                        {
                            NextChar();
                            t = XmlTokens.EndComment;
                        }
                    }
                    break;
                case '=':
                    NextChar();
                    t = TokenType.Equal;
                    break;
                case '>':
                    NextChar();
                    t = TokenType.GreaterThan;
                    break;
                case '"':
                    bool isValid = true;
                    char quote = CurrentChar;
                    int position = Position;
                    do
                    {
                        NextChar();
                        while (Position < Length && CurrentChar != quote)
                        {
                            if (CurrentChar == '>' && Source[Position - 1] != '=')
                                break;
                            if (CurrentChar == '<')
                                break;
                            NextChar();
                        }
                        if (Position == Length)
                        {
                            Position = position;
                            CurrentChar = quote;
                            isValid = false;
                            NextChar();
                            break;
                        }
                        if (CurrentChar == quote)
                            NextChar();
                        else if (CurrentChar == '>' && Source[Position - 1] == '/')
                        {
                            CurrentChar = '/';
                            --Position;
                            isValid = false;
                        }
                    } while (CurrentChar == quote);
                    t = isValid ? TokenType.StringLiteral : TokenType.Unknown;
                    break;
                default:
                    if (Char.IsWhiteSpace(CurrentChar))
                    {
                        NextChar();
                        t = TokenType.Whitespace;
                        break;
                    }
                    if (IsValidIdentifierSymbol(true, CurrentChar))
                    {
                        do
                        {
                            NextChar();
                        } while (IsValidIdentifierSymbol(false, CurrentChar));
                        t = TokenType.Identifier;
                        break;
                    }
                    if (Position == Length)
                    {
                        t = TokenType.Eof;
                        break;
                    }

                    NextChar();
                    break;
            }
            Token = t;
            Value = Source.Substring(tokenPos, Position - tokenPos);
            return t;
        }

        protected override bool IsValidIdentifierSymbol(bool firstSymbol, char symbol)
        {
            bool isValid = symbol == ':' || char.IsLetter(symbol) || symbol == '_';
            if (firstSymbol)
                return isValid;
            return isValid || symbol == '-' || symbol == '.' || char.IsDigit(symbol);
        }

        #endregion
    }
}