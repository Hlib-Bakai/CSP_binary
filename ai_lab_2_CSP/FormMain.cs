using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ai_lab_2_CSP
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }


        int[,] arr;

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {   if (arr != null)
                Drawer.FillBoard(e.Graphics, arr);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Randomizer.Randomize(out arr, int.Parse(textBox1.Text));
            pictureBox1.Invalidate();
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }
    }
}
