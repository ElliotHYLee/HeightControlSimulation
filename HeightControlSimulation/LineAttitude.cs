/*============================================================
// Made by: Elliot Hongyun Lee
// Undergrad Research Project
============================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
namespace HeightControlSimulation
{
    class LineAttitude
    {
        private Line line;
        private double canvasWidth, canvasHeight;
        public LineAttitude(Canvas canv)
        {
            this.line = new Line();
            this.canvasWidth = canv.Width;
            this.canvasHeight = canv.Height;
        }

        public Line getLine(double x)
        {
            double angle = -x;
            double origin = this.canvasWidth / 2;
            this.line.Visibility = System.Windows.Visibility.Visible;
            this.line.Stroke = System.Windows.Media.Brushes.Black;

            double lineLength = this.canvasWidth /2  ;
            //Console.WriteLine("line length: " + lineLength);
            this.line.X1 = origin -Math.Cos(angle * Math.PI / 180) * lineLength;
            this.line.Y1 = origin -Math.Sin(angle * Math.PI / 180) * lineLength;

            this.line.X2 = origin + Math.Cos(angle * Math.PI / 180) * lineLength;
            this.line.Y2 = origin + Math.Sin(angle * Math.PI / 180) * lineLength;


            this.line.StrokeThickness = 1;
            return line;
        }
        
        public Line getHorizontalLine(double y)
        {
            y = 500 - y;
            double origin = this.canvasWidth / 2;
            this.line.Visibility = System.Windows.Visibility.Visible;
            this.line.Stroke = System.Windows.Media.Brushes.Black;

            double lineLength = this.canvasWidth;
            Console.WriteLine("line length: " + lineLength);
            this.line.X1 = origin - lineLength/2;
            this.line.Y1 = y;

            this.line.X2 = origin + lineLength / 2;
            this.line.Y2 = y;


            this.line.StrokeThickness = 1;


            return line; 
        }



    }
}