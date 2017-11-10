﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication11
{
    //int x:=4;
    class Parser
    {
        public TreeNode root = new TreeNode("Parse");
        public TreeNode cc1;
        Stack<char> parentheses = new Stack<char>();
        public List<TreeNode> children = new List<TreeNode>();
        public List<Token> list;
        public string myStart;

        public int ind = 0;
        public Parser(){} //constructor
        public void parsing(List<Token> tokenslist) 
        {
            list = tokenslist;
            children.Clear();
            bool flag = false;
            for (int i = 0; i < tokenslist.Count; i++)
            {
                ind = i;
                if (tokenslist[i].type == Type.READ) read();
                else if (tokenslist[i].type == Type.WRITE) { flag = true; write(); }
                else if (tokenslist[i].type == Type.RETURN) { flag = true; ritorno(); }
                else if (tokenslist[i].type == Type.IDENTIFIER) { assignment(); flag = true; }
                else if (tokenslist[i].type == Type.NUMBER || tokenslist[i].type == Type.LEFTPARENTHESES) { flag = true; expression(); }
                else if (((tokenslist[i].type == Type.DATATYPEFLOAT) || (tokenslist[i].type == Type.DATATYPEINT) || (tokenslist[i].type == Type.DATATYPESTRING)) && tokenslist[i + 1].type == Type.IDENTIFIER && tokenslist[i + 2].type == Type.LEFTPARENTHESES) { flag = true; functiondec(); }
                if (flag) i = ind;
                children.Clear();
            }

        }

        public bool assignment()
        {
            myStart = "assignment";
            bool c1 = match(Type.IDENTIFIER);
            bool c2 = match(Type.ASSIGNMENTOPERATOR);
            bool c3 = expression();
            return c1 && c2 && c3;
        }
        public bool match(Type x) {
            if (ind<list.Count && list[ind].type == x) { children.Add(new TreeNode(list[ind].input.ToString())); ind++; return true; }
            return false;
        }

        public bool read()
        {
            myStart = "read";
            bool c1 = match(Type.READ);
            bool c2 = match(Type.IDENTIFIER);
            bool c3 = match(Type.SEMICOLON);
            if (c1 && c2 && c3) { treeprinter(root,children,"Read Statement"); return true; }
            return false;
        }

        public bool ritorno()
        {
            myStart = "return";
            bool c1 = match(Type.RETURN);
            bool c2 = expression();
            bool c3 = match(Type.SEMICOLON);
            //if (c1 && c2 && c3) { treeprinter(root, children, "Return Statement"); return true; }
            return false;
        }

        public bool expression()
        {
            bool c1 = term();
            bool c2 = espressione();
            if (c1||c2) {
                if (myStart == "assignment")
                { treeprinter(root, children, "Assignment Statement"); return true; }
                else if (myStart == "read") { treeprinter(root, children, "Read Statement"); return true; }
                else if (myStart == "return") { treeprinter(root, children, "Return Statement"); return true; }
                else if (myStart == "write") { treeprinter(root, children, "Write Statement"); return true; }
                else
                { treeprinter(root, children, "Expression"); return true; } 
            }
            return c1 || c2;
        }

        public bool espressione()
        {
            bool c1 = match(Type.PLUSOPERATOR) || match(Type.MINUSOPERATOR);
            bool c2 = term();
            bool c3 = false;
            if (c1 && c2)
            { c3 = espressione(); }
            return c1 && c2 && c3;
        }

        public bool write()
        {
            myStart = "write";
            bool c1 = match(Type.WRITE);
            bool c2 = expression();
            bool c3 = match(Type.ENDLINE);
            if ((c1 && c2 && c3) || (c3)) { treeprinter(root, children, "Write Statement"); return true; }
            return ((c1 && c2 && c3) || (c3));
        }

        public bool factor()
        {
            bool c1 = match(Type.LEFTPARENTHESES), c2 = false, c3 = false;
            if (parentheses.Count >= 1 && match(Type.RIGHTPARENTHESES)) { parentheses.Pop(); c3 = true; }
            else if (c1)
            {
                parentheses.Push('(');
                c2 = expression();
                c3 = match(Type.RIGHTPARENTHESES);
            }
            bool c4 = match(Type.NUMBER);
            bool c5 = match(Type.STRING);
            bool c6 = match(Type.IDENTIFIER);
            return (c1 && c2 && c3) || c4 || c5 || c6;
        }
        
        public bool functiondec()
        {
            bool c1 = match(Type.DATATYPEFLOAT) || match( Type.DATATYPEINT) || match(Type.DATATYPESTRING);
            bool c2 = match(Type.IDENTIFIER);
            bool c3 = match(Type.LEFTPARENTHESES);
            bool c4 = parameter();
            bool c6 = false;
            if (c4==false) c6 = match(Type.RIGHTPARENTHESES);
            while (c4)
            {
                bool c5 = match(Type.COMMA);
                if (c5) c4 = parameter();
                else { c6 = match(Type.RIGHTPARENTHESES); c4 = false; }
            }
            //if(!c4 && c5) error
            if (c1 && c2 && c3 && c6) { treeprinter(root, children, "Function Declaration"); return true; }
            else return false;

        }

        public bool parameter()
        {
            bool c1 = match(Type.DATATYPEFLOAT) || match( Type.DATATYPEINT) || match(Type.DATATYPESTRING);
            bool c2 = match(Type.IDENTIFIER);
            return c1 && c2;
        }
        public bool term()
        {
            bool c1 = factor();
            bool c2 = termine();
            return c1 || c2;
        }
        public bool termine()
        {
            bool c1 = match(Type.MULTIPLICATIONOPERATOR) || match(Type.DIVISIONOPERATOR);
            bool c2 = factor();
            bool c3 = false;
            if(c1 && c2) c3 = termine();
            return c1 && c2 && c3;
        }
        
        public void treeprinter(TreeNode root, List<TreeNode> tn, string child)
        {
            cc1 = new TreeNode(child);
            for(int i=0;i<tn.Count;i++){
                cc1.Nodes.Add(tn[i]);
            }
            if (list[ind].type == Type.SEMICOLON) {cc1.Nodes.Add(list[ind].input); ind++;}
            root.Nodes.Add(cc1);
            root = new TreeNode("Parse");
            tn.Clear();
            children.Clear();
        }
        
    }
}