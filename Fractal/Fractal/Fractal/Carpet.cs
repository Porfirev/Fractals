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
    /// Класс реализующий Ковёр Серпинского.
    /// </summary>
    class Carpet : FractalClass
    {
        public Carpet(int depth) : base(depth) { }
        /// <summary>
        /// Перегруженный метод для отрисовки Ковра Серпинского.
        /// </summary>
        /// <param name="window">окно</param>
        /// <param name="args">центр квадрата, длинна стороны</param>
        public override void Draw(MainWindow window, params double[] args)
        {
            double centerX = args[0];
            double centerY = args[1];
            double side = args[2];
            double leftUpX = centerX - side / 2;
            double leftUpY = centerY - side / 2;
            double rightDownX = centerX + side / 2;
            double rightDownY = centerY + side / 2;
            if (depth == 0)
            {
                Polygon pol = new Polygon();
                pol.Points.Add(new Point(leftUpX, leftUpY));
                pol.Points.Add(new Point(leftUpX, rightDownY));
                pol.Points.Add(new Point(rightDownX, rightDownY));
                pol.Points.Add(new Point(rightDownX, leftUpY));
                pol.Fill = Brushes.Black;
                window.FractalShape.Add(pol);
            }
            else
            {
                Carpet carp = new Carpet(depth - 1);
                for (int i = 0; i < 9; ++i)
                {
                    if (i != 4)
                    {
                        int row = i / 3;
                        int col = i % 3;
                        double newLeftUPX = leftUpX + row * (rightDownX - leftUpX) / 3;
                        double newLeftUPY = leftUpY + col * (rightDownY - leftUpY) / 3;
                        double newRightDownX = leftUpX + (row + 1) * (rightDownX - leftUpX) / 3;
                        double newRightDownY = leftUpY + (col + 1) * (rightDownY - leftUpY) / 3;
                        double newCenterX = (newLeftUPX + newRightDownX) / 2;
                        double newCenterY = (newLeftUPY + newRightDownY) / 2;
                        carp.Draw(window, newCenterX, newCenterY, side / 3);
                    }
                }
            }
        }
    }
}
