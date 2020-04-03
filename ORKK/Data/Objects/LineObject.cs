using System;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ORKK.Data.Objects
{
    [Serializable]
    public class LineObject
    {
        public double X1 { get; }

        public double X2 { get; }

        public double Y1 { get; }

        public double Y2 { get; }

        public Line AsLine()
        {
            return new Line()
            {
                X1 = X1,
                X2 = X2,
                Y1 = Y1,
                Y2 = Y2,
                Stroke = Brushes.Black,
            };
        }

        public LineObject(Line line)
        {
            X1 = line.X1;
            Y1 = line.Y1;

            X2 = line.X2;
            Y2 = line.Y2;
        }
    }
}
