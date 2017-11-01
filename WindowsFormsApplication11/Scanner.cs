using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApplication11
{
    class Scanner
    {
        public string tinyCode = "";
        public Token last;
        public Dictionary<string, Type> data = new Dictionary<string, Type> 
        {
            {"int",Type.DATATYPEINT},{"float",Type.DATATYPEFLOAT},{"string",Type.DATATYPESTRING},{"if",Type.IF},{"else",Type.ELSE},
            {"elseif",Type.ELSEIF},{"until",Type.UNTIL},{"read",Type.READ},{"return",Type.RETURN},{"endl",Type.ENDLINE},
            {"end",Type.END},{"constant",Type.CONSTANT},{"program",Type.PROGRAM},{"repeat",Type.REPEATSTATEMENT},{":=",Type.ASSIGNMENTOPERATOR},
            {"<>",Type.NOTEQUAL},{"&&",Type.AND},{"||",Type.OR},{"main",Type.MAIN},{"write",Type.WRITE},{"then",Type.THEN}
        };
        public Dictionary<Char, Type> operators = new Dictionary<char, Type>{{';',Type.SEMICOLON},{'+',Type.PLUSOPERATOR},{'-',Type.MINUSOPERATOR},{'*',Type.MULTIPLICATIONOPERATOR},{'<',Type.LESSTHAN},{'/',Type.DIVISIONOPERATOR},
            {'>',Type.GREATERTHAN},{'=',Type.EQUALTO},{'{',Type.LEFTCURLYBRACKETS},{'(',Type.LEFTPARENTHESES},{')',Type.RIGHTPARENTHESES},{'}',Type.RIGHTCURLYBRACKETS},{',',Type.COMMA},
            };
        public Type typeassign(string s)  // return type for lexeme
        {
            if(data.ContainsKey(s)) return data[s];
            return Type.IDENTIFIER;
        }
        public Scanner(string tiny) { // read tiny code
            this.tinyCode = tiny;
        }
        public List<Token> tokens = new List<Token>();
        public string lexeme = "";
        public Scanner() {}
        public void addtotokens(string s, Type t) 
        {
            Token to = new Token(s, t);
            tokens.Add(to);
            last = new Token(s,t);
            lexeme = "";
        }
        public void scan() 
        {
            for (int i = 0; i < tinyCode.Length;i++)
            {
                if (Char.IsLetterOrDigit(tinyCode[i]) || tinyCode[i] == '_')
                {
                    if (Char.IsLetter(tinyCode[i]) || tinyCode[i] == '_')
                    {
                        int j = i;
                        while(j<tinyCode.Length && (Char.IsLetterOrDigit(tinyCode[j]) || tinyCode[j] == '_'))
                        { 
                            lexeme += tinyCode[j];
                            j++;
                        }
                        addtotokens(lexeme,typeassign(lexeme));
                        i = j-1;
                    }
                    else {
                        int j=i;
                        while (j < tinyCode.Length && (Char.IsDigit(tinyCode[j]) || tinyCode[j] == '.'))
                            {
                                lexeme += tinyCode[j];
                                j++;
                            }
                        if(lexeme.Count(x => x == '.') > 1) addtotokens(lexeme,Type.ERROR);
                        else addtotokens(lexeme,Type.NUMBER);
                            i = j-1;
                   }
                }
                else if (tinyCode[i] == '"')
                {
                    int j = i;
                    do
                    {
                        lexeme += tinyCode[j];
                        j++;
                    } while (j < tinyCode.Length && tinyCode[j] != '"');
                    if (j == tinyCode.Length && tinyCode[j-1] != '"') addtotokens(lexeme, Type.ERROR);
                    else {
                        lexeme += tinyCode[j];
                        addtotokens(lexeme, Type.STRING);                    
                    }
                    i = j;
                }

                else if (i + 1 < tinyCode.Length && (tinyCode[i] == '/' && tinyCode[i + 1] == '*'))
                {
                    int j = i;
                    lexeme +=tinyCode[j];
                    lexeme += tinyCode[j + 1];
                    j += 2;
                    while (j + 1 < tinyCode.Length && (tinyCode[j] != '*' && tinyCode[j + 1] != '/'))
                    {
                        lexeme += tinyCode[j];
                        j++;
                    }
                    if (j + 1 == tinyCode.Length)
                    {
                        lexeme += tinyCode[j];
                        addtotokens(lexeme, Type.ERROR);
                        i = j;
                    }
                    else
                    {
                        lexeme += tinyCode[j];
                        lexeme += tinyCode[j + 1];
                        addtotokens(lexeme, Type.COMMENT); i = j + 1;
                    }
                }
                else if (tinyCode[i] == '(')
                {
                    lexeme += tinyCode[i];
                    tokens.Add(new Token(lexeme, Type.LEFTPARENTHESES));
                    lexeme = "";
                    
                    if (last != null && last.type == Type.IDENTIFIER) 
                    {
                       i++;
                       int j = i;
                       while (j < tinyCode.Length && (Char.IsLetterOrDigit(tinyCode[j]) || tinyCode[j] == '_'))
                       {
                            lexeme += tinyCode[j];
                            j++;
                       }
                       if (typeassign(lexeme) == Type.IDENTIFIER) tokens[tokens.Count - 2].type = Type.FUNCTIONCALL;
                       else tokens[tokens.Count-2].type = Type.FUNCTIONDECLARATION;
                       lexeme = "";
                       i--;
                    }
                    else 
                    {
                    }

                }
                else if (tinyCode[i] == '|' && i + 1 < tinyCode.Length && (tinyCode[i + 1] == '|')) { lexeme += "||"; addtotokens(lexeme, Type.OR); i++; }
                else if (tinyCode[i] == '&' && i + 1 < tinyCode.Length && (tinyCode[i + 1] == '&')) { lexeme += "&&"; addtotokens(lexeme, Type.AND); i++; }
                else if (tinyCode[i] == ':' && i + 1 < tinyCode.Length && (tinyCode[i + 1] == '=')) { lexeme += ":="; addtotokens(lexeme, Type.ASSIGNMENTOPERATOR); i++; }

                else if (operators.ContainsKey(tinyCode[i])) 
                {
                    if (tinyCode[i] == '<' && i+1<tinyCode.Length && (tinyCode[i + 1] == '>')) { lexeme += "<>"; addtotokens(lexeme, Type.NOTEQUAL); i++; }
                    else
                    {
                        lexeme += tinyCode[i];
                        addtotokens(lexeme, operators[tinyCode[i]]);
                    }
                }
                else if (tinyCode[i] == ' ' || tinyCode[i] == '\n')
                {
                    if (tinyCode[i] == '\n') tokens.Add(new Token("\n", Type.NEWLINE));
                }
                else
                {
                    lexeme += tinyCode[i];
                    addtotokens(lexeme, Type.ERROR);
                }
                
            }
        }
        
        //public void view() { MessageBox.Show(tokens[0].input); }
    }
}
