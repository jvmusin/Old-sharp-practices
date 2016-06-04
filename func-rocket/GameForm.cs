using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace func_rocket
{
    public class GameForm : Form
    {
        private readonly Image rocket;
        private readonly Image target;
        private readonly Image okulovsky;
        private bool okulovskyEnabled;
        private readonly Timer timer;
        private readonly ComboBox spaces;
        private GameSpace space;
        private bool left;
        private bool right;
        private readonly EasterEggs eggs = new EasterEggs();
        private bool autopilotEnabled;
        public Func<Rocket, Vector, Turn> Autopilot = (r, v) => Turn.None;

        public GameForm(IEnumerable<GameSpace> gameSpaces)
        {
            DoubleBuffered = true;
            Text = "Use Left and Right arrows to control rocket";
            rocket = Image.FromFile("images/rocket.png");
            target = Image.FromFile("images/target.png");
            okulovsky = Image.FromFile("images/okulovsky.png");
            timer = new Timer { Interval = 10 };
            timer.Tick += TimerTick;
            timer.Start();
            WindowState = FormWindowState.Maximized;
            spaces = new ComboBox {Width = 130};
            KeyPreview = true;
            spaces.SelectedIndexChanged += SpaceChanged;
            spaces.Parent = this;
            spaces.Items.AddRange(gameSpaces.Cast<object>().ToArray());
            Controls.Add(spaces);
            Focus();
            InitEasterEggs();
        }

        private void InitEasterEggs()
        {
            eggs.RegisterEgg("auto", () => autopilotEnabled = true);
            eggs.RegisterEgg("disauto", () => autopilotEnabled = false);
            eggs.RegisterEgg("bonus", () => okulovskyEnabled ^= true);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (spaces.Items.Count > 0)
                spaces.SelectedIndex = 0;
        }

        private void SpaceChanged(object sender, EventArgs e)
        {
            space = (GameSpace)spaces.SelectedItem;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (space == null) return;
            Turn control =
                left
                ? Turn.Left
                : right
                ? Turn.Right
                : autopilotEnabled
                ? Autopilot(space.Rocket, space.Target)
                : Turn.None;
            space.Move(ClientRectangle, control);
            if ((space.Rocket.Location - space.Target).Length < 40)
                timer.Stop();
            Invalidate();
            Update();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Left) left = true;
            if (e.KeyCode == Keys.Right) right = true;
            if (e.KeyCode == Keys.Space)
            {
                space.Rocket.Location = Vector.Zero;
                timer.Start();
            }
            eggs.KeyPressed(e.KeyCode);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Left) left = false;
            if (e.KeyCode == Keys.Right) right = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillRectangle(Brushes.Beige, ClientRectangle);

            if (space == null) return;

            DrawGravity(g);
            var matrix = g.Transform;

            g.TranslateTransform((float)space.Target.X, (float)space.Target.Y);
            g.DrawImage(target, new Point(-target.Width / 2, -target.Height / 2));

            if (timer.Enabled)
            {
                g.Transform = matrix;
                g.TranslateTransform((float)space.Rocket.Location.X, (float)space.Rocket.Location.Y);
                g.RotateTransform(90 + (float)(space.Rocket.Direction * 180 / Math.PI));

                var curRocket = okulovskyEnabled ? okulovsky : rocket;
                var x = -curRocket.Width/2;
                var y = okulovskyEnabled ? -20 : -curRocket.Height/2;
                g.DrawImage(curRocket, new Point(x, y));
            }
        }

        private void DrawGravity(Graphics g)
        {
            Action<Vector, Vector> draw =
                (a, b) => g.DrawLine(Pens.DeepSkyBlue, (int)a.X, (int)a.Y, (int)b.X, (int)b.Y);
            for (int x = 0; x < ClientRectangle.Width; x += 50)
                for (int y = 0; y < ClientRectangle.Height; y += 50)
                {
                    var p1 = new Vector(x, y);
                    var v = space.Gravity(p1);
                    var p2 = p1 + 20 * v;
                    var end1 = p2 - 8 * v.Rotate(0.5);
                    var end2 = p2 - 8 * v.Rotate(-0.5);
                    draw(p1, p2);
                    draw(p2, end1);
                    draw(p2, end2);
                }
        }
    }

    internal class EasterEggs
    {
        private readonly List<EggHolder> eggs = new List<EggHolder>();

        internal void RegisterEgg(string keyword, Action action)
        {
            eggs.Add(new EggHolder(keyword, action));
        }

        internal void KeyPressed(Keys key)
        {
            var keyCode = (char) key;
            if (char.IsLetter(keyCode))
            {
                keyCode = char.ToLower(keyCode);
                eggs.ForEach(egg => egg.KeyPressed(keyCode));
            }
        }

        private class EggHolder
        {
            private readonly string keyword;
            private readonly Action action;
            private int index;

            internal EggHolder(string keyword, Action action)
            {
                this.keyword = keyword;
                this.action = action;
            }

            internal void KeyPressed(char keyCode)
            {
                if (keyword[index] != keyCode)
                {
                    index = 0;
                    return;
                }

                if (++index == keyword.Length)
                    action();
                index %= keyword.Length;
            }
        }
    }
}
