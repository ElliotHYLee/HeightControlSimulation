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

        private double targetHeight;
        double prevVel, prevPos, prevAcc;

        bool ctrlIsOn;


        public MainWindow()
        {
            InitializeComponent();
            this.loaded();
        }

        public void setTargetHeight(double x) //x in meters
        {
            this.targetHeight = x * 100;
        }

        public void loaded()
        {
            ctrlIsOn = false;
            btnControllerSwitch.Content = "Turn On";
            SolidColorBrush dd = new SolidColorBrush();
            dd.Color = Colors.Green;
            btnControllerSwitch.Background = dd;
            cw = this.canv.Width;
            ch = this.canv.Height;
            w = this.circle.Width;
            h = this.circle.Height;
            txtPosY.Text = "5";

            myObj = new RigidBody();
            myObj.radius = w / 2;
            myWorld = new World(myObj);
            myCtrl = new Controller(myObj, this);

            myCtrl.Kp = 0.5;
            myCtrl.Kd_up = 2;
            myCtrl.Kd_down = 16;
            myCtrl.Ki = 0.1;

            txtKp.Text = myCtrl.Kp.ToString();
            txtKd_up.Text = myCtrl.Kd_up.ToString();
            txtKd_down.Text = myCtrl.Kd_down.ToString();
            txtKi.Text = myCtrl.Ki.ToString();



            angle = 90;
            //timer values
            t = 0;
            tInterval = 0.01;
            myWorld.tInterval = tInterval;

            highLine = new LineAttitude(this.canv);
            lowLine = new LineAttitude(this.canv);

            

            targetLine = new LineAttitude(this.canv);

            setTargetHeight(2.5);
            txtTarget.Text = (targetHeight/100).ToString();

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
            this.canv.Children.Add(this.targetLine.getHorizontalLine(targetHeight - myObj.radius));
            this.canv.Children.Add(this.circle);
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            t += tInterval;

            if (ctrlIsOn)
            {
                myCtrl.thrustControl(myObj.posY);
            }

            myWorld.calculateNextMathCoord();

            if (myObj.posY >5.5 || myObj.posY < 0)
            {
                Console.WriteLine("pos = " + myObj.posY);
                Console.WriteLine("timer stops");
                Timer.Stop();
            }
            else
            {
                poseCircle();
                
            }
                
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            double temp = double.Parse(txtTarget.Text);
            setTargetHeight(temp);
            updateLine(angle);
            myCtrl.TargetPos = temp*1000;
        }

        private void btnControllerSwitch_Click(object sender, RoutedEventArgs e)
        {
            if(ctrlIsOn)
            {
                ctrlIsOn = false;
                btnControllerSwitch.Content = "Trun On";
                SolidColorBrush dd = new SolidColorBrush();
                dd.Color = Colors.Green;
                btnControllerSwitch.Background = dd;
                myObj.extForce = 0;
            }else
            {
                btnControllerSwitch.Content = "Trun Off";
                SolidColorBrush dd = new SolidColorBrush();
                dd.Color = Colors.Red;
                btnControllerSwitch.Background = dd;
                ctrlIsOn = true;
            }
            

            myObj.IsDefult = false;
        }

        private void btnSetGains_Click(object sender, RoutedEventArgs e)
        {
            myCtrl.Kp = double.Parse(txtKp.Text);
            myCtrl.Kd_down = double.Parse(txtKd_down.Text);
            myCtrl.Kd_up = double.Parse(txtKd_up.Text);
            myCtrl.Ki = double.Parse(txtKi.Text);
           
        }

        private void btnResetHeight(object sender, RoutedEventArgs e)
        {
            if(ctrlIsOn)
            {
                myObj.posY = double.Parse(txtPosY.Text);
            }
            else
            {
                myObj.posY = double.Parse(txtPosY.Text);
                myObj.IsDefult = false;
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
