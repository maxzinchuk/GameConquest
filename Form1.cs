using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace GameConquest
{
    
    public class contry{
        List<prov> contryProvs;
        List<Ship> contryShips = new List<Ship>();
        Color color;
        int turns;
        int storona = r.Next(4);
        public contry(List<prov> provs, Color myColor) {
            color = myColor;
            contryProvs = provs;
            provs[0].ProvColor = myColor;
            turns = 0;
        }
        public int size {
            get {
                int Size = 0;
                for (int i = 0; i < contryProvs.Count; i++)
                {
                    if (contryProvs[i].ProvColor == color) {
                        Size++;
                    }
                }
                return Size;
            }
        }

        public void testShips(prov[,] map) {
            for (int i = 0; i < contryShips.Count; i++)
            {
                contryShips[i].Go(map);
                if (contryShips[i].Used)
                {
                    contryShips.RemoveAt(i);
                }
            }
            try
            {
                contryShips.Sort();
            }
            catch (Exception) { }
        }

        public int startX {
            get
            {
                int x = 99;
                for (int i = 0; i < contryProvs.Count; i++)
                {
                    if (contryProvs[i].X1 < x) {
                        x = contryProvs[i].X1;
                    }
                }
                return x;
            }
        }
        public int EndX
        {
            get
            {
                int x = 0;
                for (int i = 0; i < contryProvs.Count; i++)
                {
                    if (contryProvs[i].X1 > x)
                    {
                        x = contryProvs[i].X1;
                    }
                }
                return x;
            }
        }
        public int startY
        {
            get
            {
                int y = 99;
                for (int i = 0; i < contryProvs.Count; i++)
                {
                    if (contryProvs[i].Y1 < y)
                    {
                        y = contryProvs[i].Y1;
                    }
                }
                return y;
            }
        }
        public int EndY
        {
            get
            {
                int y = 0;
                for (int i = 0; i < contryProvs.Count; i++)
                {
                    if (contryProvs[i].Y1 > y)
                    {
                        y = contryProvs[i].Y1;
                    }
                }
                return y;
            }
        }
        static Random r = new Random();
        public void AIBrain(prov[,] map) {
            if (contryProvs.Count > 0)
            {
                for (int i = 0; i < contryProvs.Count; i++)
                {
                    storona = r.Next(4);
                    for (int j = 0; j < 4; j++)
                    {
                        switch (storona) {
                            case 0:addNewProv(map, contryProvs[i].X1+1, contryProvs[i].Y1);break;
                            case 1:addNewProv(map, contryProvs[i].X1-1, contryProvs[i].Y1);break;
                            case 2:addNewProv(map, contryProvs[i].X1, contryProvs[i].Y1+1);break;
                            case 3:addNewProv(map, contryProvs[i].X1, contryProvs[i].Y1-1);break;
                        
                        }
                        storona++;
                        storona %= 4;
                    }
                }
            }
        }
        public void testProvs(prov[,] map) {
            for (int i = 0; i < contryShips.Count; i++)
            {
               // contryShips[i].Go(map);
                if (contryShips[i].Used) {
                    contryShips.RemoveAt(i);
                }
            }
            try {
                contryShips.Sort();
            }
            catch (Exception) { }
            for (int i = 0; i < contryProvs.Count; i++)
            {
                if (contryProvs[i].ProvColor != map[contryProvs[i].X1, contryProvs[i].Y1].ProvColor || contryProvs[i].ProvColor != color)
                {
                    contryProvs.RemoveAt(i);
                }
            }
            try
            {
                contryProvs.Sort();
            }
            catch (Exception) { }
        }
        public bool addNewProv(prov[,] map,int x, int y)
        {
            bool result = false;
            try
            {
                if (map[(x - 1) % 100, y % 100].ProvColor == color || map[(x + 1) % 100, y % 100].ProvColor == color || map[x % 100, (y - 1) % 100].ProvColor == color || map[x % 100, (y+ 1) % 100].ProvColor == color)
                {
                    if (turns > 0 && map[x % 100, y % 100].Water && !map[x % 100, y % 100].Shipped1)
                    {
                        map[x % 100, y % 100].shipping(color);
                        contryShips.Add(new Ship(this,x,y,map));
                        turns--;
                        return true;
                    }
                    if (map[x % 100, y % 100].ProvColor != color && turns > 0 && !map[x % 100, y % 100].Water)
                    {
                        if (map[x % 100, y % 100].ProvColor == Color.White)
                        {
                            contryProvs.Insert(r.Next(contryProvs.Count),map[x % 100, y % 100]);
                            map[x % 100, y % 100].ProvColor = color;
                            turns--;
                        }
                        else {
                            map[x % 100, y % 100].ProvColor = Color.White;
                        }
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception) {
                return result;
            }
        }
        public int addTurn {
            set => turns += value;
        }
        public int Turns {
            get => turns;
        }
        public Color Color { get => color; set => color = value; }
        public List<prov> ContryProvs { get => contryProvs; set => contryProvs = value; }
    }
    public class Ship
    {
        contry Owner;
        int x, y;
        int storona = 0;
        public Ship(contry owner, int Xpos, int Ypos, prov[,] map)
        {
            if (map[Xpos%100, Ypos%100].Water)
            {
                Owner = owner;
                x = Xpos + 100;
                y = Ypos + 100;
                map[x % 100, y % 100].shipping(Owner.Color);
            }
        }
        public bool Used = false;

        Random r = new Random();
        public void Go(prov[,] map) {
            storona = r.Next(4);
            switch (storona) {
                case 0:
                    try
                    {
                        if (!map[(x + 1) % 100, y % 100].Shipped1)
                        {
                            if (map[(x + 1) % 100, y % 100].Water)
                            {
                                map[x % 100, y % 100].unShipping();
                                map[(x + 1) % 100, y % 100].shipping(Owner.Color);
                                x++;
                            }
                            else
                            {
                                if (map[(x + 1) % 100, y % 100].ProvColor != Owner.Color)
                                {
                                    map[x % 100, y % 100].unShipping();
                                    Owner.ContryProvs.Add(map[(x + 1) % 100, y % 100]);
                                    map[(x + 1) % 100, y % 100].ProvColor = Owner.Color;
                                    Used = true;
                                }
                            }
                        }
                    }
                    catch (Exception) { 
                    
                    }
                break;
                case 1:
                    if (!map[(x - 1) % 100, y % 100].Shipped1)
                    {
                        if (map[(x - 1) % 100, y % 100].Water)
                        {
                            map[x % 100, y % 100].unShipping();
                            map[(x-1) % 100, y % 100].shipping(Owner.Color);
                            x--;
                        }
                        else
                        {
                            if (map[(x - 1) % 100, y % 100].ProvColor != Owner.Color)
                            {
                                map[x % 100, y % 100].unShipping();
                                Used = true;
                                Owner.ContryProvs.Add(map[(x - 1) % 100, y % 100]);

                                map[(x - 1) % 100, y % 100].ProvColor = Owner.Color;
                            }
                        }
                    }
                    break;
                case 2:
                    try
                    {
                        if (!map[x % 100, (y % 100)+ 1].Shipped1)
                        {
                            if (map[x % 100, (y % 100)+ 1].Water)
                            {
                                map[x % 100, y % 100].unShipping();
                                map[x % 100, (y % 100)+ 1].shipping(Owner.Color);
                                y++;
                            }
                            else
                            {
                                if (map[x + 1, y].ProvColor != Owner.Color)
                                {
                                    map[x % 100, y % 100].unShipping();
                                    Owner.ContryProvs.Add(map[x % 100, (y + 1) % 100]);
                                    Used = true;
                                    map[x % 100, (y + 1) % 100].ProvColor = Owner.Color;
                                }
                            }
                        }
                    }
                    catch (Exception) { }
                    break;
                case 3:
                    try
                    {
                        if (!map[x % 100, (y - 1) % 100].Shipped1)
                        {
                            if (map[x % 100, (y - 1) % 100].Water)
                            {
                                map[x % 100, y % 100].unShipping();
                                map[x % 100, (y - 1) % 100].shipping(Owner.Color);
                                y++;
                            }
                            else
                            {
                                if (map[x - 1, y].ProvColor != Owner.Color)
                                {
                                    map[x % 100, y % 100].unShipping();
                                    map[x % 100, (y - 1) % 100].ProvColor = Owner.Color;
                                Used = true;
                                    Owner.ContryProvs.Add(map[x % 100, (y -1) % 100]);
                                }
                            }
                        }
                    }
                    catch (Exception) { 
                    
                    }
                    break;
            
            }
        }

    }
    
    public class prov{
        int X, Y;
        private Color provColor;
        public prov(int x, int y) {
            X1 = x;
            Y1 = y;
            provColor = Color.White;
        }
        bool water = false;
        bool Shipped = false;
        public prov(int x, int y,bool water)
        {
            Water = water;
            X1 = x;
            Y1 = y;
            provColor = Color.White;
            if (water)
            {
                provColor = Color.Blue;
            }
        }

        public void shipping(Color newColor) {
            if (!Shipped)
            {
                Shipped = true;
                ProvColor = Color.FromArgb(newColor.R / 2, newColor.G / 2, (newColor.B /2) + 128);
            }
        }
        public void unShipping()
        {
                Shipped = false;
                ProvColor = Color.Blue;
        }


        public Color ProvColor { get => provColor; set => provColor = value; }
        public int X1 { get => X; set => X = value; }
        public int Y1 { get => Y; set => Y = value; }
        public bool Water { get => water; set => water = value; }
        public bool Shipped1 { get => Shipped; set => Shipped = value; }
    }
    public partial class Form1 : Form
    {
       

        public prov[,] map = new prov[100, 100];
        contry[] contries = new contry[200];
        public Form1()
        {

            InitializeComponent();
            this.Text = "Game Conquest 1.0.6 by Maxim Zinchuk 21.07.2020";
            start();

        }
               public void start() {
            ticks = 0;
            countOfTicks = 0;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = new prov(i, j);
                }
            }
            for (int i = 0; i < 10; i++)
            {
                int x = r.Next(100), y = r.Next(100);
                for (int j = 0; j < 500; j++)
                {
                    switch (r.Next(4))
                    {
                        case 0:
                            x++; map[x % 100, y % 100] = new prov(x % 100, y % 100, true);
                            break;
                        case 1:
                            try
                            {
                                x--; map[x % 100, y % 100] = new prov(x % 100, y % 100, true);
                            }
                            catch (Exception)
                            {
                                x = 99; map[x % 100, y % 100] = new prov(x % 100, y % 100, true);
                            }
                            break;
                        case 2:
                            y++; map[x % 100, y % 100] = new prov(x % 100, y % 100, true);
                            break;
                        case 3:
                            try
                            {
                                y--; map[x % 100, y % 100] = new prov(x % 100, y % 100, true);
                            }
                            catch (Exception)
                            {
                                y = 99; map[x % 100, y % 100] = new prov(x % 100, y % 100, true);
                            }
                            break;
                    }
                }
            }
            List<prov> contryProvs = new List<prov>();
            contryProvs.Add(map[r.Next(100), r.Next(100)]);
            Player = new contry(contryProvs, Color.Red);
            for (int i = 0; i < contries.Length; i++)
            {
                contryProvs = new List<prov>();
                contryProvs.Add(map[r.Next(100), r.Next(100)]);
                if (contryProvs[0].ProvColor == Color.White)
                {
                    contries[i] = (new contry(contryProvs, Color.FromArgb(r.Next(200), r.Next(200), r.Next(200))));
                }
                else
                {
                    i--;
                }
            }
            update(0, 0, 100, 100);
        }


      contry Player;

    public void update(int startX, int startY, int endX, int endY) {

             zoom = 100 / (1 + (Player.EndX + 2) - (Player.startX - 2));
            if (zoom > (100 / (((Player.EndY + 2) - (Player.startY - 2))))) {
                zoom = 100 / ((Player.EndY + 2) - (Player.startY - 2));
            }
            if (zoom > 20) {
                zoom = 20;
            }
            x = Player.startX-2;
            y = Player.startY-2;
             startX+= 100;
             startY+= 100;
            endX += 100;
            endY += 100;
            Bitmap bitmap = new Bitmap(endX-startX, endY - startY);
            for (int i = startX; i < endX; i++)
            {
                for (int j = startY; j < endY; j++)
                {
                    try
                    {
                        /*
                        pen = new Pen(map[i, j].ProvColor, 1);
                        graph.DrawLine(pen, i - startX, j - startY, i - startX + 100, i - startY + 100);*/
                        bitmap.SetPixel(i - startX, j - startY, map[i % 100, j % 100].ProvColor);
                    }
                    catch(Exception ex) {
                        zoom = 1;
                        x = 0;
                        y = 0;
                        return;
                    }
                }
            }
            using (Graphics gr = Graphics.FromImage(bitmap))
            {
                gr.SmoothingMode = SmoothingMode.None;
                gr.CompositingQuality = CompositingQuality.HighSpeed;
               gr.InterpolationMode = InterpolationMode.Low;
               
                //gr.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width * 5, bitmap.Height * 5)); // 5 - во сколько раз увеличить
            }
            //aw.AddFrame(bitmap);
            
            pictureBox1.Image = bitmap;
        }
        
        Random r = new Random();
        int x = 0, y = 0;
        double zoom = 1;
        private void Button1Click(object sender, EventArgs e) {
            start();
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                zoom += 0.1;
                double procentX = (1 - ((560 - e.X) / 560.0)) + (x / 100), procentY = (1 - ((560 - e.Y) / 560.0)) + (y / 100);
                int svobodnoX = 100 - Convert.ToInt32((Convert.ToDouble(100) / zoom)) - x;
                int svobodnoY = 100 - Convert.ToInt32((Convert.ToDouble(100) / zoom)) - y;
                x = Convert.ToInt32(svobodnoX * procentX);
                y = Convert.ToInt32(svobodnoY * procentY);
            }
            else if(e.Delta < 0)
            {
                zoom -= 0.1;
                if (Convert.ToInt32((Convert.ToDouble(100) / zoom)) + x >= 100) {
                    try
                    {
                        x = 0;
                    }
                    catch (Exception) {
                        x = 0;
                    }
                }
            }
            if (zoom > 8)
            {
                zoom = 8;
            }
            else if (zoom < 1)
            {
                zoom = 1;
            }
            
        }
        
        private void pictureBox1_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                double procentX = (1 - ((560 - e.X) / 560.0)) + (x / 100), procentY = (1 - ((560 - e.Y) / 560.0)) + (y / 100);
                //int EndX = Convert.ToInt32((Convert.ToDouble(100) / zoom)) + x, EndY = Convert.ToInt32((Convert.ToDouble(100) / zoom)) + y;
                int posX = Convert.ToInt32(((100 * procentX) / zoom) + x)+100, posY = Convert.ToInt32(((100 * procentY) / zoom) + y)+100;

                Player.addNewProv(map, posX, posY);
            }
        }
        byte ticks;
        int countOfTicks;
        private void timer1_Tick(object sender, EventArgs e)
        {
            countOfTicks++;
            ticks++;
            this.timer1.Interval = 350 + (Player.Turns * 15)+ (countOfTicks * 3);
            for (int i = (contries.Length / 4) * ticks; i < (contries.Length / 4) + ((contries.Length / 4) * ticks); i++)
            {
                contries[i].testProvs(map);
                contries[i].addTurn = 1 + Convert.ToInt32(contries[i].size / (10.0 + (contries[i].size / 10.0)));
                contries[i].AIBrain(map);
            }
            if (ticks == Convert.ToInt32(3)) {
                Player.testShips(map);
                ticks = 0;
                Player.addTurn = 1 + Convert.ToInt32(Player.size / (10.0 + (Player.size / 10.0))) ;
                if (checkBox1.Checked)
                {
                    Player.AIBrain(map);
                }
                for (int i = 0; i < contries.Length; i++)
                {
                    Player.testShips(map);

                }
            }
           // update(x, y, Convert.ToInt32((Convert.ToDouble(100) / zoom)) + x, Convert.ToInt32((Convert.ToDouble(100) / zoom)) + y);
        }
        public delegate void updatFrame(int x, int y, int x1, int y1);
        private void Tick(object sender, EventArgs e) {
            Player.testProvs(map);
            updatFrame updat = update;
            label1.Text = "Ходы:" + Player.Turns;
            IAsyncResult ar = updat.BeginInvoke(x, y, Convert.ToInt32((Convert.ToDouble(100) / zoom)) + x, Convert.ToInt32((Convert.ToDouble(100) / zoom)) + y,null,null);
        }
    }
}
