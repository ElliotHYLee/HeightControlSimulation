using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeightControlSimulation
{
    class World
    {

        private RigidBody myObj;
        private double angle;
        double _tInterval;
        double tempCnt;
        double freeFallCnt;

        public World(RigidBody rb)
        {
            myObj = rb;
            angle = 90;
            tempCnt = 0; // random number
        }

        public double tInterval
        {
            set { _tInterval = value; }
            get { return _tInterval; }
        }

        public void calculateNextMathCoord()
        {

            double pos, vel;
            vel = myObj.velY + myObj.accY*_tInterval;
            pos = myObj.posY +  myObj.velY * _tInterval;
            if (pos < 0)
            {
                setRepulsion(myObj.accY, myObj.velY, myObj.posY);
            }
            else
            {
                myObj.velY = vel;
                myObj.posY = pos;
            }
            myObj.KE = 0.5 * myObj.mass * myObj.velY*myObj.velY;
            myObj.PE = myObj.mass * 9.81 * myObj.posY;

            if (myObj.accY !=0 )
            {
                //Console.Write("acc:{0:F3} ", myObj.accY);
                //Console.Write("vel:{0:F3} ", myObj.velY);
                //Console.Write(" pos:{0:F3} ", myObj.posY);
                //Console.Write(" KE:{0:F3}", myObj.KE);
                //Console.Write(" PE:{0:F3}", myObj.PE);
                //Console.WriteLine(" TE:{0:F3}", myObj.PE + myObj.KE);
            }
            

            freeFallCnt ++;
        }

        public bool setRepulsion(double prevAcc, double prevVel, double prevPos)
        {
            
            if (freeFallCnt > 2)
           
            {
                Console.WriteLine("repulse~");
                // Console.WriteLine("vel: " + prevVel + "pos: " + prevPos);
                double a, b, c, dt, dt1, dt2;

                a = 1;
                b = 2 / prevAcc * prevVel;
                c = -2 / prevAcc * (0 - prevPos);

                dt1 = (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
                dt2 = (-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a);

                if (dt1 < 0 || dt2>tInterval)
                {
                    Console.WriteLine(dt1);
                    Console.WriteLine("error");
                    return false;
                }
                else
                {
                    dt = 0;
                    if (dt1>0 && dt1<tInterval)
                    {
                        dt = dt1;
                    }

                    if (dt2>0 && dt2<tInterval)
                    {
                        dt = dt2;
                    }
                    double interVel, KE, interPos, restDt;
                    interVel = prevVel + prevAcc * dt;
                    KE = 0.5 * myObj.mass * interVel * interVel;
                    KE = KE * 0.2;
                    interVel = Math.Sqrt(KE / myObj.mass * 2);
                    interPos = 0;
                    restDt = tInterval - dt;
                    myObj.velY = interVel + prevAcc * restDt;
                    myObj.posY = interPos + myObj.velY * restDt;
                    Console.WriteLine("repulse pos = " + myObj.posY);
                    
                }
                freeFallCnt = 0;
            
            }
            else
            {
                if (freeFallCnt ==2)
                {
                    myObj.setDefault();
                    Console.WriteLine("here");
                    
                    Console.Write("acc:{0:F3} ", myObj.accY);
                    Console.Write("vel:{0:F3} ", myObj.velY);
                    Console.Write(" pos:{0:F3} ", myObj.posY);
                    Console.Write(" KE:{0:F3}", myObj.KE);
                    Console.Write(" PE:{0:F3}", myObj.PE);
                    Console.WriteLine(" TE:{0:F3}", myObj.PE + myObj.KE);
                }

            }

            return true;



        }






    }
}
