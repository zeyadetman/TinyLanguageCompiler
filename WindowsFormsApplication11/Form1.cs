using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication11
{
    public partial class Form1 : Form
    {   

        public string tinyCode = "";
        public string[] rsrvdwrds = {"read","write","repeat","until","if","elseif","else","then","return","endl","while","program","main"};
        public string[] dtatyps = { "int", "float", "string" }; 
        public List<Tuple<string, string>> lexemeANDtokens = new List<Tuple<string, string>>();
        Dictionary<string, string> tokens = new Dictionary<string, string>()
        {
            {"main","Main_Function"},{"int","Datatype"},{"float","Datatype"},{"string","Datatype"},{":=","AssignmentOperator"},
            {"write","Write_Statement"},{"read","Read_Statement"},{"return","Return_Statement"},{"repeat","Repeat_Statement"},
            {"program","Program"},{"if","If_Statement"},{"elseif","Else_If_Statement"},{"else","Else_Statement"},{"endl","Endline"}
        };
        
        Dictionary<char, string> operators = new Dictionary<char, string>(){
            {'+',"PlusOperator"},{'-',"MinusOperator"},{'*',"MultiplicationOperator"},{';',"Semicolon"},{'<',"less than"},{'/',"DivisionOperator"},
            {'>',"greater than"},{'=',"is equal"},{'{',"left curly brackets"},{'(',"left parentheses"},{')',"right parentheses"},{'}',"right curly brackets"},{',',"Comma"}
        };

        public void checker(string store, Dictionary<string, string> tokensLocal, string daType)
        {
            if (!tokensLocal.ContainsKey(store)) dataGridView1.Rows.Add(store, daType);
            else dataGridView1.Rows.Add(store, tokensLocal[store]);
                            
        }
        public void extract(string tinyCodeLocal, Dictionary<string, string> tokensLocal, Dictionary<char,string> operatorsLocal)
        {
            string lastWord = "";
            string savedWord = "";
            for (int i = 0; i < tinyCodeLocal.Length; i++)
            {
                if (Char.IsDigit(tinyCodeLocal[i]))
                {
                    savedWord += tinyCodeLocal[i];
                    for (int j = ++i; j < tinyCodeLocal.Length; j++)
                    {
                        if (Char.IsDigit(tinyCodeLocal[j]) || tinyCodeLocal[j] == '.')
                        {
                            savedWord += tinyCodeLocal[j];
                        }
                        else if (operatorsLocal.ContainsKey(tinyCodeLocal[j])) {
                            i = j;
                            dataGridView1.Rows.Add(savedWord, "Number");
                            if (tinyCodeLocal[j] == '<' && tinyCodeLocal[j + 1] == '>')
                            {
                                dataGridView1.Rows.Add("<>", "Not equal");
                                j++;
                            }
                            else if (tinyCodeLocal[j] == '|' && tinyCodeLocal[j + 1] == '|')
                            {
                                dataGridView1.Rows.Add("||", "OR");
                                j++;
                            }
                            else if (tinyCodeLocal[j] == '&' && tinyCodeLocal[j + 1] == '&')
                            {
                                dataGridView1.Rows.Add("&&", "AND");
                                j++;
                            }
                            else if (tinyCodeLocal[j] == '<' && tinyCodeLocal[j + 1] == '=')
                            {
                                dataGridView1.Rows.Add("<=", "Less than or equal");
                                j++;
                            }
                            else if (tinyCodeLocal[j] == '>' && tinyCodeLocal[j + 1] == '=')
                            {
                                dataGridView1.Rows.Add(">=", "Greater than or equal");
                                j++;
                            }
                            else
                            dataGridView1.Rows.Add(tinyCodeLocal[j], operatorsLocal[tinyCodeLocal[j]]);
                            savedWord = "";
                            break;
                        }
                        else
                        {
                            i = j;
                            dataGridView1.Rows.Add(savedWord, "Number");
                            lastWord = "Number";
                            savedWord = "";
                            break;
                        }
                    }
                }
                else if (tinyCodeLocal[i] == '/' && tinyCodeLocal[i + 1] == '*')
                {
                    i += 2;
                    for (int j = i; j < tinyCodeLocal.Length; j++)
                    {
                        if (tinyCodeLocal[j] == '*' && tinyCodeLocal[j + 1] == '/')
                        {
                            i = j + 1;
                            dataGridView1.Rows.Add(savedWord, "Comment");
                            lastWord = savedWord;
                            savedWord = "";
                            break;
                        }
                        else savedWord += tinyCodeLocal[j];
                    }

                }

                else if (Char.IsLetter(tinyCodeLocal[i])) {
                    savedWord += tinyCodeLocal[i];
                    for (int j = ++i; j < tinyCodeLocal.Length; j++)
                    {
                        if (Char.IsLetterOrDigit(tinyCodeLocal[j])) { savedWord += tinyCodeLocal[j]; }
                        else if (tinyCodeLocal[j] == '(')
                        {
                            i = j;
                            if (tokensLocal.ContainsKey(lastWord))
                            {
                                if (tokensLocal[lastWord] == "Datatype")
                                {
                                    checker(savedWord, tokensLocal, "FunctionName");
                                    dataGridView1.Rows.Add(tinyCodeLocal[j], operatorsLocal[tinyCodeLocal[j]]);
                                    lastWord = savedWord;
                                    savedWord = "";
                                    break;
                                }
                                else
                                {
                                    dataGridView1.Rows.Add(savedWord, "Function_Call");
                                    dataGridView1.Rows.Add(tinyCodeLocal[j], operatorsLocal[tinyCodeLocal[j]]);
                                    lastWord = savedWord;
                                    savedWord = "";
                                    break;
                                }
                            }
                            else
                            {
                                dataGridView1.Rows.Add(savedWord, "Function_Call");
                                dataGridView1.Rows.Add(tinyCodeLocal[j], operatorsLocal[tinyCodeLocal[j]]);
                             
                                lastWord = savedWord;
                                savedWord = "";
                                break;
                            }

                        } 
                        else if (tinyCodeLocal[j] == ';')
                        {
                            checker(savedWord, tokensLocal, "Identifier");
                            i = j; 
                            dataGridView1.Rows.Add(";", operatorsLocal[';']);
                            lastWord = savedWord;
                            savedWord = "";
                            break;
                        }
                        else if (operatorsLocal.ContainsKey(tinyCodeLocal[j]))
                        {
                            i = j;
                            dataGridView1.Rows.Add(savedWord, "Number");
                            if (tinyCodeLocal[j] == '<' && tinyCodeLocal[j + 1] == '>')
                            {
                                dataGridView1.Rows.Add("<>", "Not equal");
                                j++;
                            }
                            else if (tinyCodeLocal[j] == '|' && tinyCodeLocal[j + 1] == '|')
                            {
                                dataGridView1.Rows.Add("||", "OR");
                                j++;
                            }
                            else if (tinyCodeLocal[j] == '&' && tinyCodeLocal[j + 1] == '&')
                            {
                                dataGridView1.Rows.Add("&&", "AND");
                                j++;
                            }
                            else if (tinyCodeLocal[j] == '<' && tinyCodeLocal[j + 1] == '=')
                            {
                                dataGridView1.Rows.Add("<=", "Less than or equal");
                                j++;
                            }
                            else if (tinyCodeLocal[j] == '>' && tinyCodeLocal[j + 1] == '=')
                            {
                                dataGridView1.Rows.Add(">=", "Greater than or equal");
                                j++;
                            }
                            else
                                dataGridView1.Rows.Add(tinyCodeLocal[j], operatorsLocal[tinyCodeLocal[j]]);
                            
                            savedWord = "";
                            break;
                        }
                        else if (tinyCodeLocal[j] == ':' && tinyCodeLocal[j + 1] == '=')
                        {
                            checker(savedWord, tokensLocal, "Identifier");
                            j++; i = j; dataGridView1.Rows.Add(":=", tokensLocal[":="]); lastWord = ":=";
                            savedWord = "";
                            break;
                        }
                        else
                        {
                            i = j;
                            checker(savedWord, tokensLocal, "Identifier");                            
                            lastWord = savedWord;
                            savedWord = "";
                            break;
                        }
                    }
                }
                else if (tinyCodeLocal[i] == '"') {
                    for (int j = ++i; j < tinyCodeLocal.Length; j++)
                    {
                        if (tinyCodeLocal[j] != '"') { savedWord += tinyCodeLocal[j]; }
                        else
                        {
                            i = j;
                            dataGridView1.Rows.Add(savedWord, "String");
                            lastWord = savedWord;
                            savedWord = "";
                            break;
                        }
                    }
                }
                else if (tinyCodeLocal[i] == ';') { dataGridView1.Rows.Add(";", operatorsLocal[';']); }
                else if (operatorsLocal.ContainsKey(tinyCodeLocal[i]))
                {
                   if (tinyCodeLocal[i] == '<' && tinyCodeLocal[i + 1] == '>')
                    {
                        dataGridView1.Rows.Add("<>", "Not equal");
                        i++;
                    }
                    else if (tinyCodeLocal[i] == '|' && tinyCodeLocal[i + 1] == '|')
                    {
                        dataGridView1.Rows.Add("||", "OR");
                        i++;
                    }
                    else if (tinyCodeLocal[i] == '&' && tinyCodeLocal[i + 1] == '&')
                    {
                        dataGridView1.Rows.Add("&&", "AND");
                        i++;
                    }
                    else if (tinyCodeLocal[i] == '<' && tinyCodeLocal[i + 1] == '=')
                    {
                        dataGridView1.Rows.Add("<=", "Less than or equal");
                        i++;
                    }
                    else if (tinyCodeLocal[i] == '>' && tinyCodeLocal[i + 1] == '=')
                    {
                        dataGridView1.Rows.Add(">=", "Greater than or equal");
                        i++;
                    }
                    else
                        dataGridView1.Rows.Add(tinyCodeLocal[i], operatorsLocal[tinyCodeLocal[i]]);                            
                        savedWord = "";
                }
                else if (tinyCodeLocal[i] == ':' && tinyCodeLocal[i + 1] == '=') { i++; dataGridView1.Rows.Add(":=", tokensLocal[":="]); }
                
                
                

            }

        }
        public void Setter()
        {
            int ff = dataGridView1.Rows.Count;
            for (int i = 0; i < ff-1; i++)
            {
                // Khaled Function will replaced below
                //if (dataGridView1[1, i].Value.ToString() == "Number") MessageBox.Show(dataGridView1[0, i].Value.ToString(), cnvrt2num(string));
                lexemeANDtokens.Add(new Tuple<string,string>((string)dataGridView1[0, i].Value.ToString(), (string)dataGridView1[1, i].Value.ToString()));
            }
        }

        public void CheckKeyword(string word, Color color, int startIndex)
        {
            if (this.richTextBox1.Text.Contains(word))
            {
                int index = -1;
                int selectStart = this.richTextBox1.SelectionStart;

                while ((index = this.richTextBox1.Text.IndexOf(word, (index + 1))) != -1)
                {
                    this.richTextBox1.Select((index + startIndex), word.Length);
                    this.richTextBox1.SelectionColor = color;
                    this.richTextBox1.Select(selectStart, 0);
                    this.richTextBox1.SelectionColor = Color.Black;
                }
            }
        }


        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Refresh the dataGridView
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            //--------------------------

            //read tinyCode from user and extract values 
            tinyCode = richTextBox1.Text;
            extract(tinyCode, tokens, operators);
            Setter();
            //-----------------------------------------
        }

        private void label1_Click(object sender, EventArgs e){}

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {//rsrvdwrds
            for (int i = 0; i < rsrvdwrds.Length;i++) this.CheckKeyword(rsrvdwrds[i], Color.Blue, 0);
            for (int i = 0; i < dtatyps.Length; i++) this.CheckKeyword(dtatyps[i], Color.Green, 0);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

