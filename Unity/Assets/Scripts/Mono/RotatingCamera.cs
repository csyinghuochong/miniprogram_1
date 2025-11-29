using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class RotatingCamera : MonoBehaviour
    {
        public float rotateTime = 0.2f;
        private Transform player;
        private bool isRotating = false;

        void Start()
        {
            this.player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        void Update()
        {
            this.transform.position = this.player.position;

            this.Rotate();
        }

        void Rotate()
        {
            if (Input.GetKeyDown(KeyCode.Q) && !this.isRotating)
            {
                StartCoroutine(RotateAround(-45, this.rotateTime));
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(RotateAround(45, this.rotateTime));
            }
        }

        IEnumerator RotateAround(float angel, float time)
        {
            float number = 60 * time;
            float nextAngel = angel / number;
            this.isRotating = true;

            for (int i = 0; i < number; i++)
            {
                this.transform.Rotate(new Vector3(0, 0, nextAngel));
                yield return new WaitForFixedUpdate();
            }

            this.isRotating = false;
        }
    }
}