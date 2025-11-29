using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class FacingCamera : MonoBehaviour
    {
        private Transform[] childs;

        void Start()
        {
            this.childs = new Transform[this.transform.childCount];
            for (int i = 0; i < this.transform.childCount; i++)
            {
                this.childs[i] = this.transform.GetChild(i);
            }
        }

        void Update()
        {
            for (int i = 0; i < this.childs.Length; i++)
            {
                this.childs[i].rotation = Camera.main.transform.rotation;
            }
        }
    }
}