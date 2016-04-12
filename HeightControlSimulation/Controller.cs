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
        private double _intgrl = 0;
        double _kp, _ki, _kd_up, _kd_down;
        public Controller(RigidBody obj, MainWindow x)
        {
            _targetPos = 0.4*1000;
            _prevErr=0;
            _myobj = obj;
            _main = x;
            
        }

        public double Kp
        {
            get { return _kp; }
            set { _kp = value; }
        }

        public double Kd_up
        {
            get { return _kd_up; }
            set { _kd_up = value; }
        }

        public double Kd_down
        {
            get { return _kd_down; }
            set { _kd_down = value; }
        }

        public double Ki
        {
            get { return _ki; }
            set { _ki = value; }
        }



        public double TargetPos
        { set {_targetPos = value; } get { return _targetPos; } }


        public double pwm2Thrust(int pwm)
        {
            double thrust = 2.81 / (1449 - 1200) * (pwm - 1200) * 9.81;

            return thrust;
        }

        public void thrustControl(double currentPos)
        {

            //_kp = 1;
            //_kd = 1;
            //_ki = 0.1;

            //Console.WriteLine("ctrl working");
            double pro=0, dir=0, error, totalPwm;
            
            // units in milli meters
            error = _targetPos - currentPos * 1000;

            #region proportional

            
            pro = error * _kp;
            #endregion

            #region diravitive
            
            if (_myobj.velY < 0) dir = (_myobj.velY * 1000) * _kd_down;
            else dir = (_myobj.velY*1000) * _kd_up;

            #endregion

            #region integral

            
            _intgrl += error * _ki;


            int b = 1500;
            int maxB = b, minB = -1 * b;
            if (_intgrl > maxB) _intgrl = maxB;
            if (_intgrl < minB) _intgrl = minB;

            #endregion

            totalPwm = (pro - dir + _intgrl);



            if (totalPwm < 1200) totalPwm = 1200;
            if (totalPwm > 1600) totalPwm = 1550;

            double thrust  = pwm2Thrust((int)totalPwm);
            _myobj.extForce = thrust;

            //_prevErr = error;

            try
            {
                _main.txtError.Text = ((int)error).ToString() + " mili meters";
                _main.txtPro.Text = ((int)pro).ToString();
                _main.txtDir.Text = ((int)dir).ToString();
                _main.txtInt.Text = ((int)_intgrl).ToString();
                _main.txtPwm.Text = ((int)totalPwm).ToString() + " pwm";
                _main.txtThrust.Text = (Math.Truncate(100 * thrust) / 100).ToString() + " Newton";
                _main.txtTotAcc.Text = (Math.Truncate(100*_myobj.accY)/100).ToString() + " meters per sec^2";
                _main.txtVel.Text = (Math.Truncate(100 * _myobj.velY) / 100).ToString() + " meters per sec";
            }
            catch(Exception e) {; }
            




        }

    }
}
