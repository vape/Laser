using System;
using UnityEngine;

namespace Laser.Game.Main.Grid
{
    [SelectionBase]
    [ExecuteInEditMode]
    public class GridElementController : MonoBehaviour
    {
        public GridTile Tile
        {
            get
            {
                return new GridTile(X, Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public bool IsDirty
        { 
            get
            {
                return isDirty;
            }
            set
            {
                isDirty = value;

                if (isDirty && transform.parent != null)
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

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                if (x != value)
                {
                    x = value;
                    IsDirty = true;
                }
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
                if (y != value)
                {
                    y = value;
                    IsDirty = true;
                }
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
