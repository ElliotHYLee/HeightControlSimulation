using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeightControlSimulation
{
    class Controller
    {
        private double _targetPos;
        private double _prevErr;
        private RigidBody _myobj;
        private MainWindow _main;

        public Controller(RigidBody obj, MainWindow x)
        {
            _targetPos = 2.5;
            _prevErr=0;
            _myobj = obj;
            _main = x;
        }

        public void thrustControl(double currentPos)
        {
            //Console.WriteLine("ctrl working");
            double pro=0, intgrl=0, dir=0, error, totalExtAcc;
            double kp, ki, kd;

            error = _targetPos - currentPos;
            //Console.WriteLine("err = " + error);
            if (error < 0.001 && error > -0.001) return;
            

            if (_myobj.posY > _targetPos) // above target
            {
                // err < 0
                kp = 0.1;
                pro = kp/error;
            }
            else
            {
                // below target
                if (error < 0.5) kp = 0.0005;
                else kp = 2.0;
                pro = error * kp;
            }

            
            if (_myobj.velY >= 0)
            {
                //going up
                if (Math.Abs(error) < 0.5) kd = 40;
                else kd = 5;
                
            }
            else
            {
                //going down
                kd = 60;
            }

            ki = 0;
            if (Math.Abs(error) < 0.5)
            {
                ki = 200;
            }
            
            dir = (_myobj.velY) * kd;

            intgrl += error * ki;
            double limit = 500;
            if (Math.Abs(intgrl) > limit)
            {
                if (intgrl > 0) intgrl = limit;
                else intgrl = -limit;

            }

            totalExtAcc = pro - dir + intgrl;
            if (error <0) // above target
            {
                _myobj.accY = -9.81 + totalExtAcc;
            }
            else
            {
                _myobj.accY = _myobj.accY + totalExtAcc;
                //Console.WriteLine("total acc on body = " + _myobj.accY);
            }
            
            _prevErr = error;

            try
            {
                _main.txtError.Text = error.ToString().Substring(0, 5) + " meters";
            }
            catch(Exception e) {; }
            




        }

    }
}
