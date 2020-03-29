﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zatacka_own
{
    class Player
    {
        private float PosX { set; get; }
        private float PosY { set; get; }

        public static double Speed { set; get; } = 2.5;
        public static double Turn { set; get; } = 0.1;


        private double Direction { set; get; }

        private List<PointF> path;  //List storing player 1's path
        private List<PointF> gapPath; //gap path
        private List<List<PointF>> gapList; //list of gap paths
        private List<List<PointF>> pathList; //list of paths
        public static List<Player> result = new List<Player>(); //Result of one round;

        private Color lineColor;
        private Keys left;
        private Keys right;

        private Boolean turningRight;
        private Boolean turningLeft;

        public Boolean Player_death { set; get; } = false;

        public Player(Color lineColor, Keys left, Keys right, float x, float y, double dir)
        {
            this.lineColor = lineColor;
            this.left = left;
            this.right = right;


            path = new List<PointF>(); //Path of the player
            pathList = new List<List<PointF>>(); //List containing the complete path
            gapPath = new List<PointF>();
            gapList = new List<List<PointF>>();
            result = new List<Player>(); //Result of one round


            PosX = x;
            PosY = y;
            Direction = dir;

            PointF start = new PointF(PosX, PosY);

            path.Add(start);
            path.Add(start);
        }

        //Getting the Color of the current player
        public Color GetColor()
        {
            return lineColor;
        }

        internal void paint(PaintEventArgs e)
        {
            Graphics grap = e.Graphics;

            grap.DrawCurve(new Pen(lineColor, 6), path.ToArray());

            foreach (List<PointF> g in gapList)
            {
                if (g.Count > 1)
                {
                    grap.DrawCurve(new Pen(Game.ActiveForm.BackColor, 6), g.ToArray());
                }
            }
        }

        //Getting the color of the specific pixel pair
        public Color GetPixelColor(int x, int y)
        {
            return Game.b.GetPixel(x, y);
        }

        public void addGapToList()
        {
            if(Game.gapCounter == 20)
            {
                gapList.Add(gapPath);
                gapPath.Clear();
            }
        }

        public void addPathToList()
        {
            if(Game.tickCounter == Game.Gap * 20)
            {
                pathList.Add(path);
                path.Clear();
            }
        }

        public void tick()
        {
            if (Player_death == true)
            {
                return;
            }
            else
            {
                if (turningLeft)
                    Direction -= Turn;

                if (turningRight)
                    Direction += Turn;

                PosX += (float)(Math.Cos(Direction) * Speed);

                PosY += (float)(Math.Sin(Direction) * Speed);

                //Adding player's new path point to the list
                PointF newPoint = new PointF(PosX, PosY);
                int newX = Convert.ToInt32(PosX);

                int newY = Convert.ToInt32(PosY);


                //Checking valid X, Y coordinates
                if (newX >= Game.ActiveForm.Width || newX <= 1
                    || newY >= Game.ActiveForm.Height || newY <= 1)
                {
                    //Death
                    Player_death = true;
                    result.Add(this);//Adding to the result list
                }
                //Checking background color
                else if (this.GetPixelColor(newX, newY) != Color.FromArgb(255, 240, 240, 240)
                    && this.GetPixelColor(newX, newY) != Color.FromArgb(0, 0, 0, 0))
                {
                    //Death
                    Player_death = true;
                    result.Add(this);//Adding to the result list
                }
                else
                {
                    if (Game.gapCounter != 0)
                    {
                        gapPath.Add(newPoint);
                        addGapToList();
                    }
                    path.Add(newPoint);
                    //addPathToList();
                        
                }
            }
        }

        public void keyDownEvent(Keys key)
        {
            if (left.Equals(key))
                turningLeft = true;

            if (right.Equals(key))
                turningRight = true;
        }

        public void keyUpEvent(Keys key)
        {
            if (left.Equals(key))
                turningLeft = false;

            if (right.Equals(key))
                turningRight = false;
        }

    }
}
