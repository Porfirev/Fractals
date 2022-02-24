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
    class Kantor : FractalClass
    {
        public Kantor(int depth) : base(depth) { }
        /// <summary>
        /// Перегруженный метод для отрисовки Множества Кантора.
        /// </summary>
        /// <param name="window">окно</param>
        /// <param name="args">старт, финиш</param>
        public override void Draw(MainWindow window, params double[] args)
        {
            double startX = args[0];
            double startY = args[1];
            double finishX = args[2];
            double finishY = args[3];
            if (depth != 0)
            {
                double interval = args[4];
                double middleStartX = startX + (finishX - startX) / 3;
                double middleStartY = startY + (finishY - startY) / 3 + interval;
                double middleFinishX = finishX - (finishX - startX) / 3;
                double middleFinishY = finishY - (finishY - startY) / 3 + interval;
                Kantor kantor = new Kantor(depth - 1);
                kantor.Draw(window, startX, startY + interval, middleStartX, middleStartY, interval);
                kantor.Draw(window, middleFinishX, middleFinishY, finishX, finishY + interval, interval);
            }
            Line line = new Line();
            line.X1 = startX;
            line.Y1 = startY;
            line.X2 = finishX;
            line.Y2 = finishY;
            line.Stroke = Brushes.Black;
            line.StrokeThickness = 4;
            window.FractalDiscontinuously.Add(line);
        }
    }
}
