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
using System.Windows.Threading;

namespace HeightControlSimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LineAttitude highLine;
        private LineAttitude lowLine;
        private LineAttitude targetLine;
        private DispatcherTimer Timer;
        private Point canvasCoord;
        double tInterval;
        private RigidBody myObj;
        private World myWorld;
        private Controller myCtrl;

        private double angle;
        double cw;
        double ch;
        double w;
        double h;
  
        double t;

        double prevVel, prevPos, prevAcc;


        public MainWindow()
        {
            InitializeComponent();
            this.loaded();
        }

        public void loaded()
        {
            cw = this.canv.Width;
            ch = this.canv.Height;
            w = this.circle.Width;
            h = this.circle.Height;

            myObj = new RigidBody();
            myObj.radius = w / 2;
            myWorld = new World(myObj);
            myCtrl = new Controller(myObj);


            angle = 90;
            //timer values
            t = 0;
            tInterval = 0.01;
            myWorld.tInterval = tInterval;

            highLine = new LineAttitude(this.canv);
            lowLine = new LineAttitude(this.canv);
            targetLine = new LineAttitude(this.canv);

            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            Timer.Tick += Timer_Tick;

            
            // Console.WriteLine(this.canv.Width + "  " + this.canv.Height);
            updateLine(angle);
            poseCircle();
            Timer.Start();
            
        }

        public void updateLine(double ang)
        {
            this.canv.Children.Clear();
            this.canv.Children.Add(this.highLine.getHorizontalLine(500));
            this.canv.Children.Add(this.lowLine.getHorizontalLine(-myObj.radius*2));
            this.canv.Children.Add(this.targetLine.getHorizontalLine(250 - myObj.radius));
            this.canv.Children.Add(this.circle);
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            t += tInterval;

            myCtrl.thrustControl(myObj.posY);
            myWorld.calculateNextMathCoord();

            if (myObj.posY >5 || myObj.posY < 0)
            {
                Console.WriteLine("timer stops");
                Timer.Stop();
            }
            else
            {
                poseCircle();
            }
           

           
            

                
        }



        private void poseCircle()
        {
            convertToCanvasCoord();
            Canvas.SetLeft(circle, canvasCoord.X);
            Canvas.SetTop(circle, canvasCoord.Y);
                        
        }


        private void convertToCanvasCoord()
        {
            canvasCoord.X = 250;// + cw / 2 - w / 2;
            canvasCoord.Y = -myObj.posY*100+500;//  - h / 2;
        }


    }
}
