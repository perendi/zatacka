﻿using System;
using System.Windows.Forms;

namespace Zatacka_own
{
    public partial class Game : Form
    {
        Player p1;

        public Game()
        {
            p1 = new Player();
            InitializeComponent();
            timer1.Start();
        }

        private void paint(object sender, PaintEventArgs e)
        {

            p1.paint(100, 100, e);

        }

        private void timer_tick(object sender)
        {
            p1.tick();
            Invalidate();
        }
    }
}