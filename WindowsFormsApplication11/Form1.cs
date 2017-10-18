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
        public string[] rsrvdwrds = {"int","float","string","read","write","repeat","until","if","elseif","else","then","return","endl"};
        
        Dictionary<string, string> tokens = new Dictionary<string, string>()
        {
            {"main","Main_Function"},{"int","Datatype"},{"float","Datatype"},{"string","Datatype"},{":=","AssignmentOperator"},
            {"write","Write_Statement"},{"read","Read_Statement"},{"return","Return_Statement"},{"repeat","Repeat_Statement"},
            {"program","Program"},{"if","If_Statement"},{"elseif","Else_If_Statement"},{"else","Else_Statement"},{"<>","not equal"}
        };
        
        Dictionary<char, string> operators = new Dictionary<char, string>(){
            {'+',"PlusOperator"},{'-',"MinusOperator"},{'*',"MultiplicationOperator"},{';',"Semicolon"},{'<',"less than"},
            {'>',"greater than"},{'=',"is equal"}
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
                                    //if (!tokensLocal.ContainsKey(savedWord)) dataGridView1.Rows.Add(savedWord, "FunctionName");
                                    //else dataGridView1.Rows.Add(savedWord, tokensLocal[savedWord]);
                                    lastWord = savedWord;
                                    savedWord = "";
                                    break;
                                }
                                else
                                {
                                    dataGridView1.Rows.Add(savedWord, "Function_Call");
                                    lastWord = savedWord;
                                    savedWord = "";
                                    break;
                                }
                            }
                            else
                            {
                                dataGridView1.Rows.Add(savedWord, "Function_Call");
                                lastWord = savedWord;
                                savedWord = "";
                                break;
                            }

                        } 
                        else if (tinyCodeLocal[j] == ';')
                        {
                            checker(savedWord, tokensLocal, "Identifier");
                            //if (!tokensLocal.ContainsKey(savedWord)) dataGridView1.Rows.Add(savedWord, "Identifier");
                            //else dataGridView1.Rows.Add(savedWord, tokensLocal[savedWord]);
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
                            dataGridView1.Rows.Add(tinyCodeLocal[j], operatorsLocal[tinyCodeLocal[j]]);
                            savedWord = "";
                            break;
                        }
                        else if (tinyCodeLocal[j] == ':' && tinyCodeLocal[j + 1] == '=')
                        {
                            checker(savedWord, tokensLocal, "Identifier");
                            //if (!tokensLocal.ContainsKey(savedWord)) dataGridView1.Rows.Add(savedWord, "Identifier");
                            //else dataGridView1.Rows.Add(savedWord, tokensLocal[savedWord]);
                            j++; i = j; dataGridView1.Rows.Add(":=", tokensLocal[":="]); lastWord = ":=";
                            savedWord = "";
                            break;
                        }
                        else
                        {
                            i = j;
                            checker(savedWord, tokensLocal, "Identifier");                            
                            //if (!tokensLocal.ContainsKey(savedWord)) dataGridView1.Rows.Add(savedWord, "Identifier");
                            //else dataGridView1.Rows.Add(savedWord, tokensLocal[savedWord]);
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
                    dataGridView1.Rows.Add(tinyCodeLocal[i], operatorsLocal[tinyCodeLocal[i]]);
                    savedWord = "";
                }
                else if (tinyCodeLocal[i] == ':' && tinyCodeLocal[i + 1] == '=') { i++; dataGridView1.Rows.Add(":=", tokensLocal[":="]); }
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
            //-----------------------------------------
        }

        private void label1_Click(object sender, EventArgs e){}

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

