using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Runed.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        public List<UIScreen> UIPrefabs = new List<UIScreen>();

        public List<UIScreen> VisibleScreens = new List<UIScreen>();
        private Transform _mainCanvas;

        void Awake()
        {
            Instance = this;

            this._mainCanvas = GameObject.FindObjectOfType<Canvas>().transform;

            var screens = Resources.LoadAll<UIScreen>("");
            foreach (var uiScreen in screens)
            {
                if(!this.UIPrefabs.Find(x => x.GetType() == uiScreen.GetType()))
                {
                    this.UIPrefabs.Add(uiScreen);
                }
            }
        }

        void OnDestroy()
        {
            Instance = null;
        }

        //TODO: Move to InputManager
        private void Update()
        {
            if(Input.anyKey)
            {
                foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
                {
                    foreach (var screen in this.VisibleScreens)
                    {
                        if (Input.GetKeyUp(key))
                        {
                            screen.OnKeyUp(key);

                            if (key == KeyCode.Escape)
                            {
                                screen.OnBack();
                            }
                        }

                        if (Input.GetKeyDown(key))
                        {
                            screen.OnKeyDown(key);
                        }

                        if (Input.GetKey(key))
                        {
                            screen.OnKey(key);
                        }
                    }
                }
            }
        }

        public void Show<T>() where T : UIScreen
        {
            var prefab = this.UIPrefabs.Find(x => x.GetType() == typeof(T));

            var screen = Instantiate(prefab);
            screen.transform.SetParent(this._mainCanvas, false);
            screen.gameObject.SetActive(true);
            this.VisibleScreens.Add(screen);

            screen.Show();
        }

        public void Hide<T>() where T : UIScreen
        {
            this.HideFirst<T>();
        }

        public void HideFirst<T>() where T : UIScreen
        {
            var screen = this.VisibleScreens.Find(x => x.GetType() == typeof(T));

            this.HideScreen(screen);
        }

        public void HideLast<T>() where T : UIScreen
        {
            var screen = this.VisibleScreens.FindLast(x => x.GetType() == typeof(T));

            this.HideScreen(screen);
        }

        public void HideAll<T>() where T : UIScreen
        {
            var screens = this.VisibleScreens.FindAll(x => x.GetType() == typeof(T));

            foreach (var screen in screens)
            {
                this.HideScreen(screen);
            }
        }

        private void HideScreen(UIScreen screen)
        {
            if (screen)
            {
                screen.Hide();

                this.VisibleScreens.Remove(screen);

                Destroy(screen.gameObject);
            }
        }
    }

}

