using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication11
{
    /// <summary>
    /// tests
    /// </summary>
    /*
    int a:=3;
float b:=4.3;
string s:="asd";
string aas:= "asd",ss:="4",sff;
string s;
      */
    ///
    ///
    ///

    class Symantic
    {
        public Dictionary<string, KeyValuePair<string, Type>> symbol = new Dictionary<string, KeyValuePair<string, Type>>();
        public Dictionary<int, string> errors = new Dictionary<int, string>(){
            {1,"This variable declared before!"},
            {2,"you must assign same datatype!"},
            {3,"This variable has never declared before"}
        };
        public TreeNode tree = new TreeNode();
        public List<string> errlst = new List<string>();
        public int paramss = 0;
        public List<string> types = new List<string>();
        Scanner sc = new Scanner();
        public string[] splitter(string splito)
        {
            string[] fa={splito,""}; 
            if(splito.Contains('^'))
            return splito.Split('^');
            return fa;
        }
        public void errorHandler(int e)
        {
            errlst.Add(errors[e]);
        }
        public bool checkerDT(string s, Type t)
        {
            if ((s == "NUMBER" && (t == Type.DATATYPEFLOAT || t == Type.DATATYPEINT)) || (s == "STRING" && (t == Type.DATATYPESTRING))) return true;
            return false;
        }

        public void decStatement(string typee, string dataTypeSTR,Type datatype,string tmp,TreeNode parser)
        {
            string ssss = "";
            string type = splitter(typee)[0];
            if(type == "function") ssss = splitter(typee)[1];

            paramss = 0;
            types.Clear();

            for (int i = 1; i < parser.Nodes.Count; i++)
            {
                string[] element = splitter(parser.Nodes[i].Text.ToString());
                if (element[1].ToString() == Type.DATATYPEFLOAT.ToString() || element[1].ToString() == Type.DATATYPEINT.ToString() || element[1].ToString() == Type.DATATYPESTRING.ToString())
                {
                    datatype = sc.data[element[0]];
                }
                if (element[1].ToString() == Type.IDENTIFIER.ToString() )
                {
                    tmp = element[0];
                    try
                    {
                        symbol.Add(element[0], new KeyValuePair<string, Type>("UNINTIALIZED", datatype));
                        if (element[0] == ssss)
                        {
                            continue;
                        }
                        else
                        {
                            paramss++;
                            types.Add(datatype.ToString());
                        }

                    }
                    catch (Exception e)
                    {
                        errorHandler(1);
                    }

                }
                else if (element[1].ToString() == Type.ASSIGNMENTOPERATOR.ToString())
                {
                    i++;
                    string[] ele = splitter(parser.Nodes[i].Text.ToString());
                    if (checkerDT(ele[1], datatype))
                    {
                        symbol[tmp] = new KeyValuePair<string, Type>(ele[0], datatype);
                    }
                    else
                    {
                        errorHandler(2);
                        symbol.Remove(tmp);
                    }

                }
            }
        }

        public void assStatement(string element, TreeNode parser, int klm)
        {
            if (symbol.Keys.Contains(element))
            {
                Type datatype = symbol[element].Value;
                string[] ele = splitter(parser.Nodes[klm].Nodes[2].Text.ToString());
                if (checkerDT(ele[1], datatype))
                {
                    symbol[element] = new KeyValuePair<string, Type>(ele[0], datatype);
                }
                else
                {
                    errorHandler(2);
                }
            }
            else
            {
                errorHandler(1);
            }
        }

        public void funDec(string s,TreeNode x) {
            string v = splitter(x.Nodes[0].Text.ToString())[0];
            if (v == "int" || v == "float" || v=="string")
            {
                string dataTypeSTR = splitter(x.Nodes[0].Text.ToString())[0];
                Type datatype = sc.data[dataTypeSTR];
                string tmp = "";
                string gglol = "function^"+s;
                decStatement(gglol,dataTypeSTR, datatype, tmp, x);
            }
            string pop = "";
            pop += paramss.ToString() + '^';
            foreach (var ss in types)
            {
                pop += ss +'^';
            }
            symbol[s] = new KeyValuePair<string, Type>(pop, sc.data[splitter(x.Nodes[0].Text.ToString())[0]]);        
        }

        public void funCall(string element, TreeNode tree){
            
            string[] constraintsOfFunction = { };
            try
            {
                constraintsOfFunction = splitter(symbol[element].Key.ToString());
            }
            catch (Exception e)
            {
                errorHandler(3);
                return;
            }
            string paramsCount = constraintsOfFunction[0];
            foreach (var x in tree.Nodes)
            {
                MessageBox.Show(x.ToString());
            }
            for (int i = 1; i <= Convert.ToInt32(paramsCount); i++)
            {
                MessageBox.Show(constraintsOfFunction[i]);
            
            }
            
        }
        

        internal void getTree(TreeView treeView1)
        {
            TreeNode parser = treeView1.Nodes[0];
            for (int klm = 0; klm < parser.Nodes.Count; klm++)
            {
                if (parser.Nodes[klm].Text.ToString() == "Declaration_Statement")
                {
                    string dataTypeSTR = splitter(parser.Nodes[klm].Nodes[0].Text.ToString())[0];
                    Type datatype = sc.data[dataTypeSTR];
                    string tmp = "";
                    decStatement("", dataTypeSTR, datatype, tmp, parser.Nodes[klm]);
                }
                else if (parser.Nodes[klm].Text.ToString() == "Assignment Statement")
                {
                    string element = splitter(parser.Nodes[klm].Nodes[0].Text.ToString())[0];
                    assStatement(element,parser,klm);
                }
                else if (parser.Nodes[klm].Text.ToString() == "Function Declaration")
                {
                    string element = splitter(parser.Nodes[klm].Nodes[1].Text.ToString())[0];
                    funDec(element,parser.Nodes[klm]);
                }
                else if (parser.Nodes[klm].Text.ToString() == "Function Call")
                {
                    string element = splitter(parser.Nodes[klm].Nodes[0].Text.ToString())[0];
                    funCall(element, parser.Nodes[klm]);
                }

            }
        }

        
    }
}
