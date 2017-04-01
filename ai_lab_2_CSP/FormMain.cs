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


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int[,] arr = { { 0, 1, -1, 0, 1 }, { 1, 1, 0, -1, -1 }, { 1, 1, 0, -1, -1 }, { 1, 1, 0, -1, -1 }, { 1, 1, 0, -1, -1 } };
            Drawer.FillBoard(e.Graphics, arr);
        }
    }
}
