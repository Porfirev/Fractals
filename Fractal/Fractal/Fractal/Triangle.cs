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
    /// Класс реализующий Треугольник Серпинского.
    /// </summary>
    class Triangle : FractalClass
    {
        public Triangle(int depth) : base(depth) { }
        /// <summary>
        /// Перегруженный метод для отрисовки Треугольника Серпинского.
        /// </summary>
        /// <param name="window">окно</param>
        /// <param name="args">Три вершины</param>
        public override void Draw(MainWindow window, params double[] args)
        {
            double firstX = args[0];
            double firstY = args[1];
            double secondX = args[2];
            double secondY = args[3];
            double thirdX = (secondX - firstX) * Math.Cos(Pi / 3) + (secondY - firstY) * Math.Sin(Pi / 3) + firstX;
            double thirdY = -(secondX - firstX) * Math.Sin(Pi / 3) + (secondY - firstY) * Math.Cos(Pi / 3) + firstY;
            if (depth == 0)
            {
                window.FractalLine.Points.Add(new Point(firstX, firstY));
                window.FractalLine.Points.Add(new Point(secondX, secondY));
                window.FractalLine.Points.Add(new Point(thirdX, thirdY));
                window.FractalLine.Points.Add(new Point(firstX, firstY));
            }
            else
            {
                double middleFirstSecondX = (firstX + secondX) / 2;
                double middleFirstSecondY = (firstY + secondY) / 2;
                double middleFirstThirdX = (firstX + thirdX) / 2;
                double middleFirstThirdY = (firstY + thirdY) / 2;
                double middleSecondThirdX = (secondX + thirdX) / 2;
                double middleSecondThirdY = (secondY + thirdY) / 2;
                Triangle triangle = new Triangle(depth - 1);
                triangle.Draw(window, firstX, firstY, middleFirstSecondX, middleFirstSecondY, middleFirstThirdX, middleFirstThirdY);
                triangle.Draw(window, middleFirstSecondX, middleFirstSecondY, secondX, secondY, middleSecondThirdX, middleSecondThirdY);
                triangle.Draw(window, middleFirstThirdX, middleFirstThirdY, middleSecondThirdX, middleSecondThirdY, thirdX, thirdY);
                window.FractalLine.Points.Add(new Point(firstX, firstY));
            }
        }
    }
}
