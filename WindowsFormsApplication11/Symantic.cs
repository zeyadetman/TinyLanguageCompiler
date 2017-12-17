using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

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
            {1, "This variable declared before!"},
            {2, "you must assign same datatype!"},
            {3, "This variable has never declared before"},
            {4, "Check the functions' parameters"}
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

            DataTable dta = new DataTable();
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
                //MessageBox.Show(parser.Nodes[klm].Nodes.Count.ToString(), klm.ToString());
                int counterloop = parser.Nodes[klm].Nodes.Count;
                for (int i = 0; i < counterloop; i++)
                {
                    string counteritem = splitter(parser.Nodes[klm].Nodes[i].Text.ToString())[0];
                    //MessageBox.Show(counteritem, "sss");
                    if (counteritem == ":=")
                    {
                        string a = "";
                        for (int j = i+1; j < counterloop-1; j++)
                        {
                            //MessageBox.Show(splitter(parser.Nodes[klm].Nodes[j].Text.ToString())[1]);
                            if (splitter(parser.Nodes[klm].Nodes[j].Text.ToString())[1] == "IDENTIFIER")
                            {
                               // MessageBox.Show(symbol[parser.Nodes[klm].Nodes[j].Text.ToString()[0].ToString()].Key.ToString());
                                a += symbol[parser.Nodes[klm].Nodes[j].Text.ToString()[0].ToString()].Key.ToString();
                            }
                            else
                                a+=splitter(parser.Nodes[klm].Nodes[j].Text.ToString())[0];
                            i = j;
                        }
                        var veqaa = dta.Compute(a, "");
                        symbol[element] = new KeyValuePair<string, Type>(veqaa.ToString(), datatype);
                    }
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
                string paramsCount = constraintsOfFunction[0];
                int paramsit = 1;
                foreach (TreeNode x in tree.Nodes)
                {
                    if (splitter(x.Text.ToString())[1] == "IDENTIFIER" && splitter(x.Text.ToString())[0] != element && paramsit <= Convert.ToInt32(paramsCount))
                    {
                        if (constraintsOfFunction[paramsit] == symbol[splitter(x.Text.ToString())[0]].Value.ToString())
                        {
                            MessageBox.Show("Yes!");
                        }
                        else
                        {
                            errorHandler(4);
                        }
                        paramsit++;
                    }
                }
                for (int i = 1; i <= Convert.ToInt32(paramsCount); i++)
                {
                    //MessageBox.Show(constraintsOfFunction[i]);
                }
            }
            catch (Exception e)
            {
                errorHandler(3);
                return;
            }
            
            
        }

        public void ifcond(string element, TreeNode tree, int klm)
        {
            DataTable dta = new DataTable();
            MessageBox.Show(tree.Nodes.Count.ToString());
            int i=0;
            string aa = "";
            if (splitter(tree.Nodes[i].Text.ToString())[0] == "if")
            {
                i++;
                while (splitter(tree.Nodes[i].Text.ToString())[0] != "then")
                {
                    aa += splitter(tree.Nodes[i].Text.ToString())[0];
                    i++;
                }
                var veqaa = dta.Compute(aa, "");
                MessageBox.Show(veqaa.ToString());   
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
                else if (parser.Nodes[klm].Text.ToString() == "If Statement")
                {
                    MessageBox.Show("111");
                    string element = splitter(parser.Nodes[klm].Nodes[0].Text.ToString())[0];
                    ifcond(element, parser.Nodes[klm], klm);
                }

            }
        }

        
    }
}
