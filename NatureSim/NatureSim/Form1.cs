using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NatureSim
{
    public partial class Form1 : Form
    {
        private Logic logic;
        private NetworkViewerForm viewer;

        public Form1()
        {
            InitializeComponent();
            viewer = new NetworkViewerForm();
            viewer.Visible = false;
            logic = new Logic(this, canvas, pcCanvas, mission, progressbar, viewer);
        }

        private void Form1_Load(object sender, EventArgs e)
        {         
        }

        private void ok_Click(object sender, EventArgs e)
        {
            logic.DrawMapOnPC();
            logic.AddCurrentChar();
            logic.NextMission();
            logic.ClearUserCanvas();
            logic.ClearMap();
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            logic.userPaint = true;
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            logic.userPaint = false;
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            logic.UserDrawOnCanvas(e.X, e.Y);
        }

        private void clear_Click(object sender, EventArgs e)
        {
            logic.ClearUserCanvas();
            logic.ClearMap();
        }

        private void Test_Click(object sender, EventArgs e)
        {
            logic.TestChar();
        }

        private void learn_Click(object sender, EventArgs e)
        {
            logic.LearnAllChars();
        }

        private void view_Click(object sender, EventArgs e)
        {
            viewer.Visible = true;
            logic.DrawNetwork();
        }
    }
}