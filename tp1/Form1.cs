using System;
using System.Windows.Forms;

namespace tp1
{
    public partial class Form1 : Form
    {
        HideNSeek game;
        public Form1()
        {
            InitializeComponent();
            game = new HideNSeek(this);
            game.Start();
        }
    }
}
