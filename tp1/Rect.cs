using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace tp3
{
    internal class Rect : Entity
    {
        public event Action<Rectangle, CancellationToken> CenterReached;
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
        //лаба 3
        public void MoveToCenterAsync(Rectangle client, CancellationToken token)
        {
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
                    OnCenterReachedAsync(client, token);
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
        public void OnCenterReachedAsync(Rectangle client, CancellationToken token)
        {
            if (CenterReached == null) return;

            ParallelOptions parOpts = new ParallelOptions();
            parOpts.CancellationToken = token;

            Parallel.Invoke(parOpts, new Action[] {
                () => ((Action<Rectangle, CancellationToken>)CenterReached.GetInvocationList()[0]).Invoke(client, token),
                () => ((Action<Rectangle, CancellationToken>)CenterReached.GetInvocationList()[1]).Invoke(client, token),
                () => ((Action<Rectangle, CancellationToken>)CenterReached.GetInvocationList()[2]).Invoke(client, token),
                () => ((Action<Rectangle, CancellationToken>)CenterReached.GetInvocationList()[3]).Invoke(client, token)
            });

            _centerReached = true;
        }
    }
}
