using System;
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

        public List<TreeNode> children = new List<TreeNode>();
        public List<Token> list;

        public int ind = 0;
        public Parser(){} //constructor
        public void parsing(List<Token> tokenslist) 
        {
            list = tokenslist;
            children.Clear();
            for (int i = 0; i < tokenslist.Count; i++)
            {
                ind = i;
                if (tokenslist[i].type == Type.READ) read();
                else if (tokenslist[i].type == Type.RETURN) ritorno();
                else if (((tokenslist[i].type == Type.DATATYPEFLOAT) || (tokenslist[i].type == Type.DATATYPEINT) || (tokenslist[i].type == Type.DATATYPESTRING)) && tokenslist[i+1].type == Type.IDENTIFIER && tokenslist[i+2].type == Type.LEFTPARENTHESES) functiondec();  
            }

        }
        public bool match(Type x) {
            if (list[ind].type == x) { children.Add(new TreeNode(list[ind].input.ToString())); ind++; return true; }
            return false;
        }

        public bool read()
        {
            bool c1 = match(Type.READ);
            bool c2 = match(Type.IDENTIFIER);
            bool c3 = match(Type.SEMICOLON);
            if (c1 && c2 && c3) { treeprinter(root,children,"READSTATEMENT"); return true; }
            return false;
        }

        public bool ritorno()
        {
            bool c1 = match(Type.RETURN);
            bool c2 = expression();
            bool c3 = match(Type.SEMICOLON);
            if (c1 && c2 && c3) { treeprinter(root, children, "RETURNSTATEMENT"); return true; }
            return false;
        }

        public bool expression()
        {
            bool c1 = espressione(); //0---
            bool c2 = match(Type.PLUSOPERATOR) || match(Type.MINUSOPERATOR);
            bool c3 = term(); //0---
            return (c1 && c2 && c3) || c3;
        }

        public bool espressione() { return true; }

        public bool term()
        {
            bool c1 = termine();
            bool c2 = match(Type.DIVISIONOPERATOR) || match(Type.MULTIPLICATIONOPERATOR);
            bool c3 = factor();
            return (c1 && c2 && c3) || c3;
        }

        public bool termine() { return true; }
        public bool factor()
        {
            bool c1 = match(Type.LEFTPARENTHESES);
            bool c2 = espressione();
            bool c3 = match(Type.RIGHTPARENTHESES);
            bool c4 = match(Type.NUMBER);
            bool c5 = match(Type.STRING);
            return (c1 && c2 && c3) || c4 || c5;
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
            if (c1 && c2 && c3 && c6) { treeprinter(root, children, "FUNCTIONDECLARATION"); return true; }
            else return false;

        }

        public bool parameter()
        {
            bool c1 = match(Type.DATATYPEFLOAT) || match( Type.DATATYPEINT) || match(Type.DATATYPESTRING);
            bool c2 = match(Type.IDENTIFIER);
            return c1 && c2;
        }

        public void treeprinter(TreeNode root, List<TreeNode> tn, string child)
        {
            cc1 = new TreeNode(child);
            for(int i=0;i<tn.Count;i++){
                cc1.Nodes.Add(tn[i]);
            }
            root.Nodes.Add(cc1);
            root = new TreeNode("Parse");
            tn.Clear();
            children.Clear();
        }
        
    }
}
