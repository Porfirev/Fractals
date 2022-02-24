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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const double eps = 1e-9;
        int oldDepthIndex = 4;
        int oldFractalIndex = 1;
        readonly string[] pathFunnyBalls = {@"FunnyBalls\Barash.jpg", @"FunnyBalls\Yozhik.jpg", @"FunnyBalls\Krosh.jpg", @"FunnyBalls\Nusha.jpg", @"FunnyBalls\Kar-Karich.jpg",
        @"FunnyBalls\Losash.jpg", @"FunnyBalls\Kopatich.jpg", @"FunnyBalls\Pin.jpg", @"FunnyBalls\Sovuniya.jpg", @"FunnyBalls\HSE.jpg"};
        readonly string[] funnyBallsNames = { "Бараш", "Ёжик", "Крош", "Нюша", "Кар-Карыч", "Лосяш", "Копатыч", "Пин", "Совунья", "Не хочу смешариков" };
        public Polyline FractalLine { get; set; }
        public List<Polygon> FractalShape { get; set; }
        public List<Line> FractalDiscontinuously { get; set; }
        readonly int[] maxDepth = { 10, 8, 4, 10, 7 };
        bool correctParams;
        public MainWindow()
        {
            InitializeComponent();
            correctParams = true;
            FractalShape = new List<Polygon>();
            FractalLine = new Polyline();
            FractalDiscontinuously = new List<Line>();
            FractalLine.Stroke = Brushes.Black;
            FractalLine.StrokeThickness = 1.5;
            for (int i = 0; i < 11; ++i)
            {
                depths.Items.Add(i);
            }
            fractals.Items.Add("Дерево Пифагора");
            fractals.Items.Add("Кривая Коха");
            fractals.Items.Add("Ковёр Серпинского");
            fractals.Items.Add("Треугольник Серпинского");
            fractals.Items.Add("Множество Кантора");
            depths.SelectedIndex = 4;
            fractals.SelectedIndex = 1;
            myGrid.Children.Add(FractalLine);
            paramGrid.Visibility = Visibility.Collapsed;
            leftSizeScrollBar.Maximum = 7;
            leftSizeScrollBar.Minimum = 0;
            leftSizeScrollBar.Value = 5.7;
            rightSizeScrollBar.Maximum = 7;
            rightSizeScrollBar.Minimum = 0;
            rightSizeScrollBar.Value = 5.7;
            leftAngleSizeScrollBar.Maximum = 1.2;
            leftAngleSizeScrollBar.Minimum = 0;
            leftAngleSizeScrollBar.Value = 0.9;
            rightAngleSizeScrollBar.Maximum = 1.2;
            rightAngleSizeScrollBar.Minimum = 0;
            rightAngleSizeScrollBar.Value = 0.9;
            intervalScrollBar.Maximum = 0.8;
            intervalScrollBar.Minimum = 0;
            intervalScrollBar.Value = 0.6;
            leftSize.Text = $"{(leftSizeScrollBar.Maximum - leftSizeScrollBar.Value) * 0.1 + 1.3:F2}";
            rightSize.Text = $"{(rightSizeScrollBar.Maximum - rightSizeScrollBar.Value) * 0.1 + 1.3:F2}";
            leftAngle.Text = $"{(leftAngleSizeScrollBar.Maximum - leftAngleSizeScrollBar.Value) * 50 + 30:F0}";
            rightAngle.Text = $"{(rightAngleSizeScrollBar.Maximum - rightAngleSizeScrollBar.Value) * 50 + 30:F0}";
            interval.Text = $"{(intervalScrollBar.Maximum - intervalScrollBar.Value) * 50 + 10:F0}";
            foreach (var funnyBallName in funnyBallsNames)
            {
                funnyBalls.Items.Add(funnyBallName);
            }
            funnyBalls.SelectedIndex = 4;
            myGrid.Background = Brushes.LightCyan;
            paramGrid.Background = Brushes.Beige;
        }

        /// <summary>
        /// Метод проверки параметров для Пифагорова Дерева, на корректность.
        /// </summary>
        /// <param name="leftRatio">переменная в которую возращаеться левый коэффициент.</param>
        /// <param name="rightRatio">переменная в которую возращаеться правы коэффициент.</param>
        /// <param name="leftNormalAngle">переменная в которую возращаеться левый угол.</param>
        /// <param name="rightNormalAngle">переменная в которую возращаеться правый угол.</param>
        private void CheckTreeParams(out double leftRatio, out double rightRatio, out double leftNormalAngle, out double rightNormalAngle)
        {
            int leftDegreeAngle, rightDegreeAngle;
            if (!double.TryParse(leftSize.Text, out leftRatio) || !double.TryParse(rightSize.Text, out rightRatio) || leftRatio < 1.3 - eps
                || leftRatio > 2 + eps || rightRatio < 1.3 - eps || rightRatio > 2 + eps)
            {
                throw new ArgumentTreeException("Вы ввели не подходящие длины");
            }
            else if (!int.TryParse(leftAngle.Text, out leftDegreeAngle) || !int.TryParse(rightAngle.Text, out rightDegreeAngle)
                || leftDegreeAngle < 30 || leftDegreeAngle > 90 || rightDegreeAngle < 30 || rightDegreeAngle > 90)
            {
                throw new ArgumentTreeException("Вы ввели не подходящие углы");
            }
            leftNormalAngle = (leftDegreeAngle * Math.PI) / 180;
            rightNormalAngle = (rightDegreeAngle * Math.PI) / 180;
        }

        /// <summary>
        /// Метод проверки параметров для Множества Кантора, на корректность.
        /// </summary>
        /// <param name="interv">переменная в которую возращаеться интервал в пикселях.</param>
        private void CheckKantorParams(out int interv)
        {
            if (!int.TryParse(interval.Text, out interv) || interv < 10 || interv > 50)
            {
                throw new ArgumentKantorException("Вы ввели не подходящий интервал");
            }
        }

        /// <summary>
        /// Метод, возвращающий y-координаты для Пифогорового Дерева.
        /// </summary>
        /// <param name="depth">глубина</param>
        /// <returns></returns>
        private double[] YCoordsTree(int depth)
        {
            depth = Math.Min(depth, Math.Abs(leftAngleSizeScrollBar.Value - rightAngleSizeScrollBar.Value) < 0.8 ? 4 : 6);
            double[] ans = new double[2];
            ans[0] = (double)2 / 3 - (double)depth / 53 + (double)1 / 5;
            ans[1] = (double)1 / 3 + (double)depth / 53 + (double)1 / 5;
            return ans;
        }
        /// <summary>
        /// Метод вызывающийся перед отрисовкой(всякие очищения, присвоенния и т.п.).
        /// </summary>
        /// <param name="leftRatio">левое отношение</param>
        /// <param name="rightRatio">правая отношение</param>
        /// <param name="leftNormalAngle">левый угол</param>
        /// <param name="rightNormalAngle">правый угол</param>
        /// <param name="interv">интервал в множестве Кантора</param>
        private void StartPaintFractal(out double leftRatio, out double rightRatio, out double leftNormalAngle, out double rightNormalAngle, out int interv)
        {
            correctParams = true;
            FractalLine.Points.Clear();
            foreach (var pol in FractalShape)
            {
                myGrid.Children.Remove(pol);
            }
            foreach (var line in FractalDiscontinuously)
            {
                myGrid.Children.Remove(line);
            }
            FractalDiscontinuously.Clear();
            FractalShape.Clear();
            CheckTreeParams(out leftRatio, out rightRatio, out leftNormalAngle, out rightNormalAngle);
            CheckKantorParams(out interv);
        }

        /// <summary>
        /// Чекер, отрисовка фракталов.
        /// </summary>
        private void PaintFractal()
        {
            try
            {
                DrawFractals();
            }
            catch (ArgumentTreeException ex)
            {
                correctParams = false;
                MessageBox.Show(ex.Message);
            }
            catch (ArgumentKantorException ex)
            {
                correctParams = false;
                MessageBox.Show(ex.Message);
            }
            catch(StackOverflowException ex)
            {
                correctParams = false;
                MessageBox.Show("Ваш компьютер слишком слабый для отрисовки фрактала, используете глубину меньше");
            }
        }

        /// <summary>
        /// Основной метод отрисовки фракталов.
        /// </summary>
        private void DrawFractals()
        {
            StartPaintFractal(out double leftRatio, out double rightRatio, out double leftNormalAngle, out double rightNormalAngle, out int interv);
            switch (fractals.SelectedIndex)
            {
                case 0:
                    Tree tree = new Tree(depths.SelectedIndex);
                    double[] yCoords = YCoordsTree(depths.SelectedIndex);
                    tree.Draw(this, this.Width / 2, yCoords[0] * this.Height, this.Width / 2, yCoords[1] * this.Height, leftRatio, leftNormalAngle, rightRatio, rightNormalAngle);
                    foreach (var line in FractalDiscontinuously)
                    {
                        myGrid.Children.Add(line);
                    }
                    break;
                case 1:
                    Koch koch = new Koch(depths.SelectedIndex);
                    koch.Draw(this, this.Width / 5, 2 * this.Height / 3, 4 * this.Width / 5, 2 * this.Height / 3);
                    break;
                case 2:
                    Carpet carpet = new Carpet(depths.SelectedIndex);
                    carpet.Draw(this, this.Width / 2, this.Height / 2, 2 * this.Height / 3);
                    foreach (var pol in FractalShape)
                    {
                        myGrid.Children.Add(pol);
                    }
                    break;
                case 3:
                    Triangle triangle = new Triangle(depths.SelectedIndex);
                    triangle.Draw(this, this.Width / 3, 2 * this.Height / 3, 2 * this.Width / 3, 2 * this.Height / 3);
                    break;
                case 4:
                    Kantor kantor = new Kantor(depths.SelectedIndex);
                    kantor.Draw(this, this.Width / 4, this.Height / 10, 3 * this.Width / 4, this.Height / 10, interv);
                    foreach (var line in FractalDiscontinuously)
                    {
                        myGrid.Children.Add(line);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Событие открывающие параметры.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Parameter_Click(object sender, RoutedEventArgs e)
        {
            paramGrid.Visibility = Visibility.Visible;
            myGrid.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Событие закрывающие параметры.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseParams_Click(object sender, RoutedEventArgs e)
        {
            PaintFractal();
            if (correctParams)
            {
                paramGrid.Visibility = Visibility.Collapsed;
                myGrid.Visibility = Visibility.Visible;
            }

        }

        /// <summary>
        /// Событие изменяющие, левое отношение с помощью ScrollBar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftSizeScrollBar_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            leftSize.Text = $"{(leftSizeScrollBar.Maximum - leftSizeScrollBar.Value) * 0.1 + 1.3:F2}";
        }

        /// <summary>
        /// Событие изменяющие, правое отношение с помощью ScrollBar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightSizeScrollBar_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            rightSize.Text = $"{(rightSizeScrollBar.Maximum - rightSizeScrollBar.Value) * 0.1 + 1.3:F2}";
        }

        /// <summary>
        /// Событие изменяющие, левый угол с помощью ScrollBar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftAngleSizeScrollBar_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            leftAngle.Text = $"{(leftAngleSizeScrollBar.Maximum - leftAngleSizeScrollBar.Value) * 50 + 30:F0}";
        }

        /// <summary>
        /// Событие изменяющие, правый угол с помощью ScrollBar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightAngleSizeScrollBar_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            rightAngle.Text = $"{(rightAngleSizeScrollBar.Maximum - rightAngleSizeScrollBar.Value) * 50 + 30:F0}";
        }

        /// <summary>
        /// Событие изменяющие, интервал с помощью ScrollBar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IntervalScrollBar_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            interval.Text = $"{(intervalScrollBar.Maximum - intervalScrollBar.Value) * 50 + 10:F0}";
        }

        /// <summary>
        /// Событие реагирующие на изменение глубины.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Depths_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (maxDepth[fractals.SelectedIndex] < (int)depths.SelectedIndex)
                {
                    MessageBox.Show("Эта глубина слишком большая для этого фрактала");
                    depths.SelectedIndex = oldDepthIndex;
                }
                else
                    PaintFractal();
                oldDepthIndex = depths.SelectedIndex;
            }
            catch { }
        }

        /// <summary>
        /// Событие реагирующие на изменение фрактала.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Fractals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (maxDepth[fractals.SelectedIndex] < (int)depths.SelectedItem)
                {
                    MessageBox.Show("Эта глубина слишком большая для этого фрактала");
                    fractals.SelectedIndex = oldFractalIndex;
                }
                else
                    PaintFractal();
                oldFractalIndex = fractals.SelectedIndex;
            }
            catch { }
        }

        /// <summary>
        /// Событие перерисовки.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Redraw_Click(object sender, RoutedEventArgs e)
        {
            PaintFractal();
        }

        /// <summary>
        /// Событие реагирующие на изменение Смешариков.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FunnyBalls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Media.Imaging.BitmapImage bit
            = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\" + pathFunnyBalls[this.funnyBalls.SelectedIndex], UriKind.Absolute));
            funnyBall.Source = bit;
            funnyBall.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Событие изменяющие, левое отношение устоновленное пользователем.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            double size = 0;
            if(!double.TryParse(leftSize.Text, out size) || size < 1.3 - eps || size > 2 + eps)
            {
                leftSizeScrollBar.IsEnabled = false;
                return;
            }
            leftSizeScrollBar.IsEnabled = true;
            leftSizeScrollBar.Value = leftSizeScrollBar.Maximum - (Math.Round(size, 2) - 1.3) * 10;
        }

        /// <summary>
        /// Событие изменяющие, правое отношение устоновленное пользователем.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            double size = 0;
            if (!double.TryParse(rightSize.Text, out size) || size < 1.3 - eps || size > 2 + eps)
            {
                rightSizeScrollBar.IsEnabled = false;
                return;
            }
            rightSizeScrollBar.IsEnabled = true;
            rightSizeScrollBar.Value = rightSizeScrollBar.Maximum - (Math.Round(size, 2) - 1.3) * 10;
        }

        /// <summary>
        /// Событие изменяющие, левый угол устоновленный пользователем.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftAngle_TextChanged(object sender, TextChangedEventArgs e)
        {
            int angle = 0;
            if (!int.TryParse(leftAngle.Text, out angle) || angle < 30 || angle > 90)
            {
                leftAngleSizeScrollBar.IsEnabled = false;
                return;
            }
            leftAngleSizeScrollBar.IsEnabled = true;
            leftAngleSizeScrollBar.Value = leftAngleSizeScrollBar.Maximum - (double)(angle - 30) / 50;
        }

        /// <summary>
        /// Событие изменяющие, правый угол устоновленный пользователем.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightAngle_TextChanged(object sender, TextChangedEventArgs e)
        {
            int angle = 0;
            if (!int.TryParse(rightAngle.Text, out angle) || angle < 30 || angle > 90)
            {
                rightAngleSizeScrollBar.IsEnabled = false;
                return;
            }
            rightAngleSizeScrollBar.IsEnabled = true;
            rightAngleSizeScrollBar.Value = rightAngleSizeScrollBar.Maximum - (double)(angle - 30) / 50;
        }

        /// <summary>
        /// Событие изменяющие, интервал устоновленный пользователем.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Interval_TextChanged(object sender, TextChangedEventArgs e)
        {
            try 
            {
                CheckKantorParams(out int interv);
                intervalScrollBar.IsEnabled = true;
                intervalScrollBar.Value = intervalScrollBar.Maximum - (double)(interv - 10) / 50;
            }
            catch(ArgumentKantorException)
            {
                intervalScrollBar.IsEnabled = false;
            }
        }

        /// <summary>
        /// Событие открывающие README.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadMe_Click(object sender, RoutedEventArgs e)
        {
            string text = File.ReadAllText(@"README.txt");
            MessageBox.Show(text);
        }
    }
}
