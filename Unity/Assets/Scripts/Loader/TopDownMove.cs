using UnityEngine;

namespace ET
{
    [RequireComponent(typeof(Rigidbody))]
    public class TopDownMove : MonoBehaviour
    {
        public float moveSpeed = 5f;

        private Rigidbody rb;
        private Vector3 input;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            float h = Input.GetAxisRaw("Horizontal"); // A/D
            float v = Input.GetAxisRaw("Vertical"); // W/S
            input = new Vector3(h, 0, v).normalized;
        }

        void FixedUpdate()
        {
            rb.velocity = input * moveSpeed;
        }
    }
}