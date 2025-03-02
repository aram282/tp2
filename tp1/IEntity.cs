using System.Windows.Forms;
using System.Drawing;

namespace tp1
{
    interface IEntity
    {
        void MoveLeft();
        void MoveRight();
        void MoveUp();
        void MoveDown();
        void Show();
        void Hide();
        void Draw(object sender, PaintEventArgs e);
        void CalculateDestination(Rectangle client);
    }
}
