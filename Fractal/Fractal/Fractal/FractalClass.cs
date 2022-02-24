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
    /// Абстрактный класс реализующий фрактал.
    /// </summary>
    abstract class FractalClass
    {
        protected const double Pi = Math.PI;
        protected int depth;
        public FractalClass(int depth)
        {
            this.depth = depth;
        }
        /// <summary>
        /// Абстрактный метод отрисовки фракталов.
        /// </summary>
        /// <param name="window">окно</param>
        /// <param name="args">аргументы</param>
        abstract public void Draw(MainWindow window, params double[] args);
    }
}
