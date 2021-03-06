﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HeightControlSimulation
{
       
    class RigidBody
    {
        
        private Point _pos;
        private Point _vel;
        private Point _acc;
        private double _KE;
        private double _PE;
        private double _extForce;
        private double _r;
        private double _m;
        private double _g;
        private bool _isDefault;


        public RigidBody()
        {

            _m = 3.5;// 2.85;
            _g = -9.81;
            _pos.Y = 1; // 5 meter
            _PE = _m * _g * _pos.Y/100;
            _vel.X = 0;
            _vel.Y = 0;
            _acc.X = 0;
            _acc.Y = _g;

            if (_pos.Y == 0) _isDefault = true;
            else _isDefault = false;

        }

        public bool IsDefult
        {
            get { return _isDefault; }
            set { _isDefault = value; }
        }

        public void setDefault()
        {
            _m = 2.85;
            _g = -9.81;
            _pos.Y = 0; // 5 meter
            _PE = _m * _g * _pos.Y / 100;
            _vel.X = 0;
            _vel.Y = 0;
            _acc.X = 0;
            _acc.Y = 0;
            _isDefault = true;
        }

        public double mass
        {
            get { return _m; }
            set { _m = value; }
        }

        public double KE
        {
            get { return _KE; }
            set { _KE = value; }
        }

        public double PE
        {
            get { return _PE; }
            set { _PE = value; }
        }


        public double velY
        {
            get { return _vel.Y;}
            set { _vel.Y = value; }
        }

        public double accY
        {
            get { return (_g + _extForce/_m); }
            set { _acc.Y = value; }
        }
        public double extForce
        {
            set { _extForce = value; }
            get { return _extForce; }
        }

        public double posY
        {
            get { return _pos.Y; }
            set { _pos.Y = value; }
        }

        public double radius
        {
            get { return _r; }
            set { _r = value; }
        }

    }
}
