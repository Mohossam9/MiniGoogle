using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace MiniGoogle
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        public static void Loadfiles(string sourceDir, List<string> allFiles)
        {

            string[] textnames = Directory.GetFiles(sourceDir);
            for (int text = 0; text < textnames.Length; text++)
            {
                allFiles.Add(textnames[text]);
            }

        }


        public static bool stopwords(string words)
        {

            if (words.Length < 3)
            {
                return true;
            }
            else if (words == "this" || words == "That" || words == "those" || words == "they" || words == "The" || words == "our" || words == "these" || words == "off" || words == "has" || words == "have" || words == "are" || words == "them" || words == "could" || words == "for" || words == "can")
            {
                return true;
            }

            return false;

        }
        public static void Indexfiles(Dictionary<string, List<Tuple<string, int>>> word, List<string> allFiles)
        {

            foreach (string filename in allFiles)
            {
                string[] Lines = File.ReadAllLines(filename);
                int linenum = 1;
                foreach (string s in Lines)
                {
                    if (s != "")
                    {
                        string[] words = s.Split(new Char[] { ' ', ',', '&', '.', '!', '?', '<', '>', '{', '}', '(', ')', '*', '%', ':', ';', '-', '\\', '/', '[', ']', '$', '^' });
                        addtodic(word, filename, words, linenum);
                    }
                    linenum++;
                }
            }

        }

        public static void addtodic(Dictionary<string, List<Tuple<string, int>>> word, string filename, string[] words, int Linenumber)
        {

            for (int w = 0; w < words.Length; w++)
            {
                if (!stopwords(words[w]))
                {
                    if (word.ContainsKey(words[w]))
                    {
                        word[words[w]].Add(Tuple.Create(filename, Linenumber));
                    }
                    else
                    {
                        List<Tuple<string, int>> newword = new List<Tuple<string, int>>();
                        newword.Add(Tuple.Create(filename, Linenumber));
                        word.Add(words[w], newword);
                    }
                }
            }

        }



        public static void results(Dictionary<string,List<Tuple<string,int>>> Word,string searchWord,DataGridView v)
        {
            DataTable res = new DataTable();
            DataSet ds = new DataSet();
            ds.Tables.Add(res);
            DataColumn sw = new DataColumn("Word");
            DataColumn path = new DataColumn("Filepath");
            DataColumn line = new DataColumn("Linenumber");
            res.Columns.Add(sw);
            res.Columns.Add(path);
            res.Columns.Add(line);
            if (searchWord == "")
            {
                MessageBox.Show("Please Enter a word !");
            }
            else
            {
                if (Word.ContainsKey(searchWord))
                {
                    
                    foreach (Tuple<string, int> inlist in Word[searchWord])
                    {
                        DataRow rw = res.NewRow();
                        string pathword = inlist.Item1;
                        int lineword = inlist.Item2;
                        rw[0] = searchWord;
                        rw[1] = pathword;
                        rw[2] = lineword;
                        res.Rows.Add(rw);
                    }
                    v.DataSource = res;

                }
                else
                {
                    DataRow rw = res.NewRow();
                    rw[0] = searchWord;
                    rw[1] = "Notfound";
                    rw[2] = "Notfound";
                    res.Rows.Add(rw);
                    v.DataSource = res;
                }
            }
        }





 
                      
        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<Tuple<string, int>>> Word = new Dictionary<string, List<Tuple<string, int>>>();
            string sourceFolder = "textAll";
            string searchWord = textBox1.Text;
            List<string> allFiles = new List<string>();
            List<Tuple<string, int>> wordinfo = new List<Tuple<string, int>>();
            Loadfiles(sourceFolder, allFiles);
            Indexfiles(Word, allFiles);
            results(Word, searchWord, dataGridView1);
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
