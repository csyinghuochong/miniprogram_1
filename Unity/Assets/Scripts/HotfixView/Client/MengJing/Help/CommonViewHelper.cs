using UnityEngine;

namespace ET.Client
{
    public static class CommonViewHelper
    {
        public static void TargetFrameRate(int frame)
        {
            Application.targetFrameRate = frame;
        }
    }
}