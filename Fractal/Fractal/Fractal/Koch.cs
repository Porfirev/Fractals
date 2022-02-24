using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Fractal
{
    /// <summary>
    /// Класс реализующий Кривую Коха.
    /// </summary>
    class Koch : FractalClass
    {
        public Koch(int depth) : base(depth) { }
        /// <summary>
        /// Перегруженный метод для отрисовки Кривой Коха.
        /// </summary>
        /// <param name="window">окно</param>
        /// <param name="args">точки старта и финиша</param>
        public override void Draw(MainWindow window, params double[] args)
        {
            double startX = args[0];
            double startY = args[1];
            double finishX = args[2];
            double finishY = args[3];
            if (depth == 0)
            {
                window.FractalLine.Points.Add(new Point(startX, startY));
                window.FractalLine.Points.Add(new Point(finishX, finishY));
            }
            else
            {
                double middleLeftX = (finishX + 2 * startX) / 3;
                double middleLeftY = (finishY + 2 * startY) / 3;
                double middleRightX = (2 * finishX + startX) / 3;
                double middleRightY = (2 * finishY + startY) / 3;
                double middleX = (middleRightX - middleLeftX) * Math.Cos(Pi / 3) + (middleRightY - middleLeftY) * Math.Sin(Pi / 3) + middleLeftX;
                double middleY = -(middleRightX - middleLeftX) * Math.Sin(Pi / 3) + (middleRightY - middleLeftY) * Math.Cos(Pi / 3) + middleLeftY;
                Koch koch = new Koch(depth - 1);
                koch.Draw(window, startX, startY, middleLeftX, middleLeftY);
                koch.Draw(window, middleLeftX, middleLeftY, middleX, middleY);
                koch.Draw(window, middleX, middleY, middleRightX, middleRightY);
                koch.Draw(window, middleRightX, middleRightY, finishX, finishY);
            }
        }
    }
}
