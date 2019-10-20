using System;
using UnityEngine;

namespace Laser.Game.Main
{
    public class GridElementController : MonoBehaviour
    {
        public bool IsDirty
        { 
            get
            {
                return isDirty;
            }
            set
            {
                if (isDirty != value)
                {
                    isDirty = value;

                    if (isDirty)
                    {
                        var parentElement = transform.parent.GetComponent<GridElementController>();
                        if (parentElement != null)
                        {
                            parentElement.IsDirty = true;
                        }
                        else
                        {
                            var parent = transform.parent.GetComponent<GridController>();
                            if (parent != null)
                            {
                                parent.IsDirty = true;
                            }
                        }
                    }
                }
            }
        }

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                IsDirty = true;
            }
        }
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                IsDirty = true;
            }
        }

        [SerializeField]
        private int x;
        [SerializeField]
        private int y;

        private bool isDirty = true;

#if UNITY_EDITOR
        [NonSerialized]
        private int __x;
        [NonSerialized]
        private int __y;

        // support changing positions in inspector
        private void Update()
        {
            if (__x != x) { __x = x; IsDirty = true; }
            if (__y != y) { __y = y; IsDirty = true; }
        }
#endif

        public void ResetDirty()
        {
            IsDirty = false;
        }
    }
}
