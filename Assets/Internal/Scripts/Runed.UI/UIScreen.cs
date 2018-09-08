using UnityEngine;

namespace Runed.UI
{
    public abstract class UIScreen : MonoBehaviour
    {
        public bool Exclusive { get; set; }

        public virtual void Show()
        {
        }

        public virtual void Hide()
        {
        }

        public virtual void OnKeyUp(KeyCode key)
        {
        }

        public virtual void OnKeyDown(KeyCode key)
        {
        }

        public virtual void OnKey(KeyCode key)
        {
        }

        public virtual void OnBack()
        {
        }
    }
}