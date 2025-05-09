using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Visutronik
{

    public class TriangleTileScript : MonoBehaviour
    {
        private enum RotationDirection { Neg, Pos, Home}

        private Transform TGameObject;
        private RotationDirection RotDir = RotationDirection.Neg;
        private float RotValue = 0;
        private float Timer = 0.5f;


        public float MaxRot { get; set; }
        public float RotationSpeed { get; set; }
        public Vector3 VecRot { get; set; }
        public bool Blocked { get; set; } = false;
        public bool EffectIsRunning { get; set; } = false;
        
        

        private void Start()
        {
            TGameObject = this.gameObject.transform;
        }

        private void Update()
        {
            if(EffectIsRunning == true)
            {
                float r = Time.deltaTime * RotationSpeed;

                if (RotDir == RotationDirection.Neg)
                {
                    if (RotValue - r < MaxRot * 1)
                    {
                        RotDir = RotationDirection.Pos;
                        RotValue += r;
                    }
                    else
                    {
                        RotValue -= r;
                    }
                }
                else if (RotDir == RotationDirection.Pos)
                {
                    if (RotValue + r > MaxRot)
                    {
                        RotDir = RotationDirection.Home;
                        RotValue -= r;
                    }
                    else
                    {
                        RotValue += r;
                    }
                }
                else
                {
                    if (RotValue - r < 0)
                    {
                        RotDir = RotationDirection.Pos;
                        r = RotValue;

                        //Finish
                        EffectIsRunning = false;
                        RotValue = 0;
                        RotDir = RotationDirection.Neg;
                        Timer = 0.2f;
                    }                    
                    else
                    {
                        RotValue -= r;
                    }
                }


                TGameObject.RotateAround(TGameObject.position, VecRot, r * (RotDir == RotationDirection.Pos ? 1 : -1));
            }
            else if(Blocked == true)
            {
                Timer -= Time.deltaTime;

                if(Timer <= 0)
                {
                    Blocked = false;
                }
            }
        }
    }
}