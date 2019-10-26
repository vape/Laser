using Newtonsoft.Json;
using UnityEngine;

namespace Laser.Game.Main.Grid
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public struct GridTile
    {
        [JsonProperty("x")]
        public int X;
        [JsonProperty("y")]
        public int Y;

        public GridTile(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    [ExecuteInEditMode]
    public class GridController : MonoBehaviour
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
                }
            }
        }

        public float CellSize;
        [Range(1, 30)]
        public int Width;
        [Range(1, 30)]
        public int Height;

        private Vector3 Origin
        { get { return transform.position; } }

        private bool isDirty;
        private float prevCellSize;

        private void Start()
        {
            Layout();
        }

        private void Update()
        {
            if (isDirty)
            {
                Layout();
                isDirty = false;
            }
        }

        public void Layout()
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                var element = transform.GetChild(i).GetComponent<GridElementController>();
                if (element != null)
                {
                    element.transform.position = GridToWorld(GetTileCenter(new GridTile(element.X, element.Y))) + new Vector3(0, element.transform.position.y, 0);
                    element.IsDirty = false;
                }
            }
        }

        public bool CanPlaceIn(GridElementController newElement, GridTile tile)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                var element = transform.GetChild(i).GetComponent<GridElementController>();
                if (element != null)
                {
                    if (element != newElement && element.X == tile.X && element.Y == tile.Y)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public Vector2? RaycastGrid(Ray ray)
        {
            var plane = new Plane(Vector3.up, Origin);
            if (plane.Raycast(ray, out var distance))
            {
                var p = ray.GetPoint(distance);
                return WorldToGrid(p);
            }

            return null;
        }

        public GridTile? RaycastTile(Ray ray)
        {
            var gp = RaycastGrid(ray);
            if (gp == null)
            {
                return null;
            }

            return GetGridTile(gp.Value);
        }

        public Vector2 WorldToGrid(Vector3 worldPosition)
        {
            var local = transform.InverseTransformPoint(worldPosition);
            return new Vector2(local.x, local.z);
        }

        public Vector3 GridToWorld(Vector2 gridPosition)
        {
            var world = transform.TransformPoint(new Vector3(gridPosition.x, 0, gridPosition.y));
            return world;
        }

        public GridTile GetGridTile(Vector2 gridPosition)
        {
            var x = (gridPosition.x / CellSize);
            var y = (gridPosition.y / CellSize);

            if (x < 0) x -= 1;
            if (y < 0) y -= 1;

            return new GridTile() { X = (int)x, Y = (int)y };
        }

        public Vector2 GetTileCenter(GridTile tile)
        {
            var x = (tile.X * CellSize) + (CellSize / 2f);
            var y = (tile.Y * CellSize) + (CellSize / 2f);

            return new Vector2(x, y);
        }

        public Rect GetTileRect(GridTile tile)
        {
            var x0 = (tile.X * CellSize);
            var y0 = (tile.Y * CellSize);

            return new Rect(x0, y0, CellSize, CellSize);
        }

        public Vector3[] GetWorldSpacedTileRect(GridTile tile)
        {
            var rect = GetTileRect(tile);
            var v0 = GridToWorld(new Vector2(rect.xMin, rect.yMin));
            var v1 = GridToWorld(new Vector2(rect.xMax, rect.yMin));
            var v2 = GridToWorld(new Vector2(rect.xMax, rect.yMax));
            var v3 = GridToWorld(new Vector2(rect.xMin, rect.yMax));

            return new Vector3[4] { v0, v1, v2, v3 };
        }
    }
}
