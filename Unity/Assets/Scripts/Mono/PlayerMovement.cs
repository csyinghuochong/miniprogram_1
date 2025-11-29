using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed;
        private Rigidbody2D rigidbody;
        private Animator animator;
        private float inputX, inputY;
        private float stopX, stopY;

        void Start()
        {
            this.rigidbody = this.GetComponent<Rigidbody2D>();
            this.animator = this.GetComponent<Animator>();
        }

        void Update()
        {
            inputX = Input.GetAxisRaw("Horizontal");
            inputY = Input.GetAxisRaw("Vertical");
            Vector2 input = (this.transform.right * inputX + this.transform.up * inputY).normalized;
            this.rigidbody.velocity = new Vector2(input.x * speed, input.y * speed);

            if (input != Vector2.zero)
            {
                this.animator.SetBool("IsMoving", true);
                this.stopX = this.inputX;
                this.stopY = this.inputY;
            }
            else
            {
                this.animator.SetBool("IsMoving", false);
            }

            this.animator.SetFloat("InputX", stopX);
            this.animator.SetFloat("InputY", stopY);
        }
    }
}