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
    /// Класс реализующий Пифагорово Дерево.
    /// </summary>
    class Tree : FractalClass
    {
        public Tree(int depth) : base(depth) { }
        /// <summary>
        /// Перегруженный метод для отрисовки Пифагорова Дерева.
        /// </summary>
        /// <param name="window">окно</param>
        /// <param name="args">точки старта и финиша, параметры дерева.</param>
        public override void Draw(MainWindow window, params double[] args)
        {
            double startX = args[0];
            double startY = args[1];
            double finishX = args[2];
            double finishY = args[3];
            Line line = new Line();
            line.X1 = startX;
            line.Y1 = startY;
            line.X2 = finishX;
            line.Y2 = finishY;
            line.Stroke = Brushes.Black;
            line.StrokeThickness = 2;
            window.FractalDiscontinuously.Add(line);
            if (depth != 0)
            {
                double left = args[4];
                double leftAng = args[5];
                double right = args[6];
                double rightAng = args[7];
                double leftPointX = ((finishX - startX) * Math.Cos(leftAng) + (finishY - startY) * Math.Sin(leftAng)) / left + finishX;
                double leftPointY = (-(finishX - startX) * Math.Sin(leftAng) + (finishY - startY) * Math.Cos(leftAng)) / left + finishY;
                double rightPointX = ((finishX - startX) * Math.Cos(rightAng) - (finishY - startY) * Math.Sin(rightAng)) / right + finishX;
                double rightPointY = ((finishX - startX) * Math.Sin(rightAng) + (finishY - startY) * Math.Cos(rightAng)) / right + finishY;
                Tree tree = new Tree(depth - 1);
                tree.Draw(window, finishX, finishY, leftPointX, leftPointY, left, leftAng, right, rightAng);
                tree.Draw(window, finishX, finishY, rightPointX, rightPointY, left, leftAng, right, rightAng);
            }
        }
    }
}
