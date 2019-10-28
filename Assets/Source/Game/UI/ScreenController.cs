using System;
using UnityEngine;

namespace Laser.Game.UI
{
    public class ScreenController : MonoBehaviour
    {
        public event Action Closing;
        public event Action Closed;

        public virtual void Init(params object[] param)
        {

        }

        public void Close()
        {
            Closing?.Invoke();
            Closing = null;
            BeforeClosed();

            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            Closed?.Invoke();
            Closed = null;

            OnDestroyScreen();
        }

        protected virtual void OnDestroyScreen()
        { }

        protected virtual void BeforeClosed()
        { }
    }
}
