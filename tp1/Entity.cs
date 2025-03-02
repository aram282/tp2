using System;
using System.Windows.Forms;
using System.Drawing;

namespace tp1
{
    abstract class Entity : IEntity
    {
        protected int _x;
        protected int _y;
        protected readonly int _width;
        protected readonly int _height;
        protected int _dx;
        protected int _dy;
        protected bool _isHidden;
        protected Color _color;
        protected Point _destination;
        protected int _period;  //для асинхронности, 2 лабораторная

        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
        public Color Color { get => _color; set => _color = value; }
        public bool IsHidden { get => _isHidden; }
        public int Period { get => _period; protected set => value = _period; }

        public Entity(int x, int y, int width, int height, Color color)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
            _dx = 3;
            _dy = 3;
            _isHidden = false;
            _color = color;
            _destination = new Point(0, 0);
            _period = 70;
        }

        public void MoveLeft() => _x -= _dx;
        public void MoveRight() => _x += _dx;
        public void MoveUp() => _y -= _dy;
        public void MoveDown() => _y += _dy;
        public void Show() => _isHidden = false;
        public void Hide() => _isHidden = true;
        public abstract void Draw(object sender, PaintEventArgs e);
        public abstract void CalculateDestination(Rectangle client);
    }
}
