using UnityEngine;

namespace Laser.Game
{
    public interface IGameObjectClickHandler
    {
        void HandleClick();
    }

    [RequireComponent(typeof(Camera))]
    public class MapClickHandler : MonoBehaviour
    {
        private Vector3 mouseDownPos;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseDownPos = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                var delta = Input.mousePosition - mouseDownPos;
                if (delta.magnitude > 10)
                {
                    return;
                }

                var ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                var hit = default(RaycastHit);

                if (Physics.Raycast(ray, out hit))
                {
                    var gameObject = hit.transform.gameObject;
                    var handlers = gameObject.GetComponents<IGameObjectClickHandler>();
                    for (int i = 0; i < handlers.Length; ++i)
                    {
                        handlers[i].HandleClick();
                    }
                }
            }
        }
    }
}
