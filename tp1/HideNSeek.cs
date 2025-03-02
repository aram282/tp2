using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace tp1
{
    internal class HideNSeek
    {
        private Rect _rectangle;
        private List<Circle> _circles;
        private Form _form;
        private System.Windows.Forms.Timer _timer;
        private Button _timerButton;

        public HideNSeek(Form form)
        {
            this._form = form;

            _rectangle = new Rect(420, 220, 30, 30, Color.Plum);
            _rectangle.Hide();

            _circles = new List<Circle>
            {
                new Circle(420, 60, 40, Color.Black),
                new Circle(120, 60, 40, Color.BlueViolet),
                new Circle(120, 320, 40, Color.DeepPink),
                new Circle(600, 300, 40, Color.DarkGray)
            };

            _form.SuspendLayout();
            //
            // timer
            //
            _timer = new System.Windows.Forms.Timer();
            _timerButton = new System.Windows.Forms.Button();
            _timer.Enabled = false;
            _timer.Tick += new EventHandler(TimerTick);
            //
            // button
            //
            _timerButton.Location = new System.Drawing.Point(532, 112);
            _timerButton.Name = "button1";
            _timerButton.Size = new System.Drawing.Size(79, 39);
            _timerButton.TabIndex = 0;
            _timerButton.Text = "таймер выключен";
            _timerButton.UseVisualStyleBackColor = true;
            _timerButton.Click += new EventHandler(TimerButton_Click);
            _form.Controls.Add(_timerButton);

            _form.Resize += new EventHandler(FormResize);
            // для асинхронности добавить обработчик Load
            _form.Load += new EventHandler(FormLoad);

            _form.ResumeLayout(false);
        }

        public void Start()
        {
            foreach (Circle c in _circles)
            {
                //_rectangle.CenterReached += c.MoveToBorder;
                _rectangle.CenterReached += c.MoveToBorderAsync;
                _form.Paint += c.Draw;
                c.StatusChanged += ShowStatus;
            }
            _form.Invalidate();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            //_rectangle.MoveToCenter(_form.ClientRectangle); // для лабы 2 не надо
            _form.Invalidate();
        }
        private void TimerButton_Click(object sender, EventArgs e)
        {
            _timer.Enabled = !_timer.Enabled;
            if (_timer.Enabled) _timerButton.Text = "таймер включен";
            else _timerButton.Text = "таймер выключен";

            if (_rectangle.IsHidden)
            {
                _form.Paint += _rectangle.Draw;
                _rectangle.Show();
            }
        }
        private void FormResize(object sender, EventArgs e)
        {
            _rectangle.CalculateDestination(_form.ClientRectangle);
            foreach (var c in _circles) c.CalculateDestination(_form.ClientRectangle);
        }
        private void FormLoad(object sender, EventArgs e)   //лаба 2
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(_rectangle.MoveToCenterAsync), _form.ClientRectangle);
        }
        private string status;
        private void ShowStatus(string _status)
        {
            status += _status;
            _form.Invoke((Action)delegate 
                {
                    _form.Text = status;
                }
            );
        }
    }
}
