using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace tp2
{
    internal class Circle : Entity
    {
        private static int _number = 0;
        private int _id;
        public event Action<string> StatusChanged;
        public Circle(int x, int y, int diameter, Color color) : base(x, y, diameter, diameter, color)
        {
            _id = ++_number;
        }

        public override void Draw(object sender, PaintEventArgs e)
        {
            if (_isHidden) return;

            using (SolidBrush brush = new SolidBrush(_color))
            {
                Graphics g = e.Graphics;
                g.FillEllipse(brush, _x, _y, _width, _height);
            }
        }
        public void MoveToBorder(Rectangle client)
        {
            if (_isHidden) return;
            if (_destination.X == 0 && _destination.Y == 0) CalculateDestination(client);

            if (_x > _destination.X)
                MoveLeft();
            else MoveRight();

            if (_y > _destination.Y)
                MoveUp();
            else MoveDown();
        }
        public override void CalculateDestination(Rectangle client)
        {
            if (_x + _width < client.Width / 2)
                _destination.X = 0;
            else _destination.X = client.Width - _width;

            if (_y + _height < client.Height / 2)
                _destination.Y = 0;
            else _destination.Y = client.Height - _height;
        }
        public void MoveToBorderAsync(Rectangle client) //лаба 2
        {
            if (StatusChanged != null) StatusChanged($"круг номер {_id} начал движение. цвет {_color.Name}. ");

            while (true)
            {
                MoveToBorder(client);
                Thread.Sleep(_period);
            }
        }
    }
}
