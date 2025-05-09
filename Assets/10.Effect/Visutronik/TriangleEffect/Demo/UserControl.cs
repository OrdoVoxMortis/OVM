using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Version 1.1

namespace Visutronik
{
    public class UserControl : MonoBehaviour
    {
        private double Timer = 0;

        private Rigidbody Rigi;
        private Transform Tran;

        private void Start()
        {
            Rigi = gameObject.GetComponent<Rigidbody>();
            Tran = gameObject.GetComponent<Transform>();
        }


        // Update is called once per frame
        void Update()
        {
            //Jump
            if (Input.GetKeyDown(KeyCode.Space) && Timer <= 0)
            {
                Rigi.AddForce(0, 6, 0, ForceMode.Impulse);
                Timer = 1.25;
            }

            //Move
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                float d = 0.2f;
                Vector3 v1 = new Vector3(d, d, d);
                Vector3 v2 = Vector3.Scale(v1, Tran.forward);
                Vector3 v3 = v2 + Tran.position;

                Rigi.MovePosition(v3);
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                float d = -0.2f;
                Vector3 v1 = new Vector3(d, d, d);
                Vector3 v2 = Vector3.Scale(v1, Tran.forward);
                Vector3 v3 = v2 + Tran.position;

                Rigi.MovePosition(v3);
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                float d = -0.2f;
                Vector3 v1 = new Vector3(d, d, d);
                Vector3 v2 = Vector3.Scale(v1, Tran.right);
                Vector3 v3 = v2 + Tran.position;

                Rigi.MovePosition(v3);
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                float d = 0.2f;
                Vector3 v1 = new Vector3(d, d, d);
                Vector3 v2 = Vector3.Scale(v1, Tran.right);
                Vector3 v3 = v2 + Tran.position;

                Rigi.MovePosition(v3);
            }

            //Rotation
            if (Input.GetKey(KeyCode.Q))
            {
                Tran.Rotate(0, -1, 0, Space.World);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                Tran.Rotate(0, 1, 0, Space.World);
            }

            //Timer for jumping
            if (Timer > 0)
            {
                Timer -= Time.deltaTime;
            }
        }


        private void OnGUI()
        {
            GUI.Box(new Rect(10, 10, 110, 100), "Controlling");

            GUI.Label(new Rect(15, 30, 100, 20), "Moving:");
            GUI.Label(new Rect(15, 45, 100, 20), "WASD or Arrows");

            GUI.Label(new Rect(15, 65, 100, 20), "Rotate:");
            GUI.Label(new Rect(15, 80, 100, 20), "Q and E");
        }
    }
}
