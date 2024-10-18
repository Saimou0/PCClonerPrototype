﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PCClonerPrototype.Forms;

namespace PCClonerPrototype
{
    public partial class MainForm : Form
    {
        public static Panel MainPanel;
        public MainForm()
        {
            InitializeComponent();
            MainPanel = panel1;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MainPage mainPage = new();
            mainPage.Dock = DockStyle.Fill;
            mainPage.TopLevel = false;
            panel1.Controls.Clear();
            panel1.Controls.Add(mainPage);
            mainPage.Show();
        }
    }
}
