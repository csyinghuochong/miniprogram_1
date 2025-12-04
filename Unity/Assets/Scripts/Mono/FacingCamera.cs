using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class FacingCamera : MonoBehaviour
    {
        private Transform[] targetTransforms;

        void Start()
        {
            CollectSpriteRendererTransforms(transform);
        }

        void Update()
        {
            if (targetTransforms == null || targetTransforms.Length == 0) return;

            for (int i = 0; i < targetTransforms.Length; i++)
            {
                targetTransforms[i].rotation = Camera.main.transform.rotation;
            }
        }

        private void CollectSpriteRendererTransforms(Transform parentTrans)
        {
            List<Transform> tempList = new List<Transform>();

            RecursiveCollect(parentTrans, tempList);

            targetTransforms = tempList.ToArray();
        }

        private void RecursiveCollect(Transform currentTrans, List<Transform> resultList)
        {
            if (currentTrans.GetComponent<SpriteRenderer>() != null)
            {
                resultList.Add(currentTrans);
            }

            for (int i = 0; i < currentTrans.childCount; i++)
            {
                RecursiveCollect(currentTrans.GetChild(i), resultList);
            }
        }
    }
}