using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ai_lab_2_CSP
{
    public partial class FormMain : Form
    {
        int[,] arr;
        Thread solver;
        bool solved = false;
        DateTime time = DateTime.Now;
        bool graph = false;

        public FormMain()
        {
            InitializeComponent();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (arr != null)
                if (graph)
                    Drawer.FillBoardGraph(e.Graphics, arr);
                else
                    Drawer.FillBoard(e.Graphics, arr);
        }

        //Randomize
        private void button1_Click(object sender, EventArgs e)
        {
            int size = int.Parse(textBox1.Text);
            if (size <= 0 || (size % 2 != 0))
            {
                label2.Visible = true;
                button2.Enabled = false;
                return;
            }
            label2.Visible = false;
            button2.Enabled = true;
            int rand = int.Parse(textBox2.Text);
            if (rand <= 0 || rand > Math.Pow(size, 2))
            {
                label2.Visible = true;
                button2.Enabled = false;
                return;
            }
            Randomizer.Randomize(out arr, int.Parse(textBox1.Text), rand);
            pictureBox1.Invalidate();
        }

        //Solve CSP
        private void button2_Click(object sender, EventArgs e)
        {
            graph = false;
            time = DateTime.Now;
            solved = false;
            ThreadStart starter = new ThreadStart(task);
            solver = new Thread(starter);
            solver.Start();
            pictureBox1.Invalidate();
        }

        private void task()
        {
            CSP_Solver solv = new CSP_Solver(arr);
            try
            {
                solved = solv.solveBackTracking(ref arr);
            }
            catch (Exception ex)
            {

            }
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private int[,] empty(int size)
        {
            int[,] emp = new int[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    emp[i, j] = -1;
            return emp;
        }

        //EMPTY
        private void button3_Click(object sender, EventArgs e)
        {
            int size = int.Parse(textBox1.Text);
            if (size <= 0 || (size % 2 != 0))
            {
                label2.Visible = true;
                button2.Enabled = false;
                return;
            }
            button2.Enabled = true;
            arr = empty(size);
            pictureBox1.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (arr != null && solved)
            //{
            //    System.Diagnostics.Debug.WriteLine("=====");
            //    for (int i = 0; i < Math.Sqrt(arr.Length); i++)
            //    {
            //        for (int j = 0; j < Math.Sqrt(arr.Length); j++)
            //        {
            //            System.Diagnostics.Debug.Write(arr[i,j] + ",");
            //        }
            //        System.Diagnostics.Debug.WriteLine("");
            //    }
            //}

            pictureBox1.Invalidate();
            if (solver != null)
                if (solver.IsAlive)
                {
                    button4.Visible = true;
                    textBox3.Text = ((DateTime.Now - time).TotalSeconds).ToString() + " s";
                }
                else
                {
                    if (solved)
                    {
                        label3.Visible = true;
                        label4.Visible = false;
                    }
                    else
                    {
                        label4.Visible = true;
                        label3.Visible = false;
                    }
                }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            solver.Abort();
        }

        //GRAPH
        private void solveGraph()
        {
            graph = true;
            CSP_Solver solv = new CSP_Solver(arr);
            try
            {
                if (radioButton1.Checked)
                    solved = solv.solveGraphBT(ref arr);
                else
                    solved = solv.solveForwardCheckingGraph(ref arr);
            }
            catch (Exception ex)
            {
                
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            time = DateTime.Now;
            solved = false;
            ThreadStart starter = new ThreadStart(solveGraph);
            solver = new Thread(starter);
            solver.Start();
            pictureBox1.Invalidate();
        }
    }
}
