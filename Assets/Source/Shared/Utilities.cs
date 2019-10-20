using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Laser.Shared
{
    public static class Utilities
    {
        public static void DestroyChildrens(this Transform transform, bool immediate = false)
        {
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                if (immediate)
                {
                    GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
                }
                else
                {
                    GameObject.Destroy(transform.GetChild(i).gameObject);
                }
            }
        }
    }
}