﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication11
{
    public partial class Form1 : Form
    {   

        public string[] rsrvdwrds = {"read","write","repeat","until","if","elseif","else","then","return","endl","while","program","main"};
        public string[] dtatyps = { "int", "float", "string" }; 
        
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
            treeView1.Nodes.Clear();
            treeView1.Refresh();
            //--------------------------

            Scanner sc = new Scanner(richTextBox1.Text);
            sc.scan();
            int ind = 0;
            for (int i = 0; i < sc.tokens.Count ; i++) dataGridView1.Rows.Add(sc.tokens[i].input,sc.tokens[i].type.ToString());
            for (int i = 0; i < sc.tokens.Count; i++) if (sc.tokens[i].type == Type.ERROR) listBox1.Items.Add("Error in Line " + (ind+1).ToString() + ", using " + sc.tokens[i].input); else if (sc.tokens[i].type == Type.NEWLINE) ind++;
            Parser ps = new Parser();
            ps.parsing(sc.tokens);
            //this.treeView1  = (TreeNode) treeView1.Clone();
            treeView1.Nodes.Add(ps.root);
            
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}

