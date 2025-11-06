using UnityEngine;

namespace ET
{
    // Y值同步到Z轴
    public class SpriteSort : MonoBehaviour
    {
        private void Awake()
        {
            Vector3 newPos = transform.position;
            newPos.z = newPos.y;
            transform.position = newPos;
        }
    }
}