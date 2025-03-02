using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace tp2
{
    internal class Rect : Entity
    {
        public event Action<Rectangle> CenterReached;
        private bool _centerReached;    //лаба 2
        public Rect(int x, int y, int width, int height, Color color) : base(x, y, width, height, color)
        {
            _centerReached = false;
        }
        public override void Draw(object sender, PaintEventArgs e)
        {
            if (_isHidden) return;

            using (SolidBrush brush = new SolidBrush(_color))
            {
                Graphics g = e.Graphics;
                g.FillRectangle(brush, _x, _y, _width, _height);
            }
        }
        public override void CalculateDestination(Rectangle client)
        {
            _destination.X = client.Width / 2 - _width / 2;
            _destination.Y = client.Height / 2 - _height / 2;
        }
        public void MoveToCenter(Rectangle client)
        {
            if (_isHidden) return;

            if (_destination.X == 0 && _destination.Y == 0) CalculateDestination(client);
            int xDistance = Math.Abs(_destination.X - _x);
            int yDistance = Math.Abs(_destination.Y - _y);
            if (xDistance <= _dx && yDistance <= _dy)
            {
                OnCenterReached(client);
                return;
            }
            if (xDistance > yDistance)
            {
                if (_destination.X > _x)
                    MoveRight();
                else MoveLeft();
            }
            else
            {
                if (_destination.Y > _y)
                    MoveDown();
                else MoveUp();
            }
        }
        public void OnCenterReached(Rectangle client)
        {
            if (CenterReached == null) return;

            foreach (Delegate del in CenterReached.GetInvocationList())
            {
                Action<Rectangle> act = (Action<Rectangle>)del;
                if (act != null) act.Invoke(client);
            }
        }
        //лаба 2
        public void MoveToCenterAsync(object _client)
        {
            Rectangle client = (Rectangle)_client;
            while (!_centerReached)
            {
                if (_isHidden)
                {
                    Thread.Sleep(_period);
                    continue;
                }

                if (_destination.X == 0 && _destination.Y == 0) CalculateDestination(client);
                int xDistance = Math.Abs(_destination.X - _x);
                int yDistance = Math.Abs(_destination.Y - _y);
                if (xDistance <= _dx && yDistance <= _dy)
                {
                    OnCenterReachedAsync(client);
                    return;
                }
                if (xDistance > yDistance)
                {
                    if (_destination.X > _x)
                        MoveRight();
                    else MoveLeft();
                }
                else
                {
                    if (_destination.Y > _y)
                        MoveDown();
                    else MoveUp();
                }

                Thread.Sleep(_period);
            }
        }
        public void OnCenterReachedAsync(Rectangle client)
        {
            if (CenterReached == null) return;

            Task.Factory.StartNew(() => Parallel.ForEach(CenterReached.GetInvocationList(),
                (del) =>
                {
                    Action<Rectangle> act = (Action<Rectangle>)del;
                    if (act != null) act.Invoke(client);
                }
            ));

            _centerReached = true;
        }
    }
}
