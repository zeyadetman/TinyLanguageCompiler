using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication11
{
    public enum Type
    {
        DATATYPEINT,DATATYPEFLOAT,DATATYPESTRING, IF, ELSE, ELSEIF, UNTIL, READ, RETURN, WRITE, ENDLINE, END, CONSTANT, PROGRAM, REPEATSTATEMENT, ASSIGNMENTOPERATOR, IDENTIFIER, SEMICOLON, THEN,
        PLUSOPERATOR,MINUSOPERATOR,EQUALTO,MULTIPLICATIONOPERATOR,LESSTHAN,DIVISIONOPERATOR,GREATERTHAN,LEFTCURLYBRACKETS,RIGHTCURLYBRACKETS,
        LEFTPARENTHESES,RIGHTPARENTHESES,COMMA,NOTEQUAL,AND,OR,COMMENT,MAIN,ERROR,NUMBER,STRING,FUNCTIONCALL,FUNCTIONDECLARATION,NEWLINE
    }
    public class Token
    {
        public string input;
        public Type type;
        public Token() { } //constractor mlnash da3wa beeh
        public Token(string inp, Type typ)
        {
            this.input = inp;
            this.type = typ;
        }
        
        
    }
}
