using System;
using UI.DialogSystem;
using UI.ScreenSystem;
using UnityEngine;

namespace UI
{
    public class UISystem : Singleton<UISystem>
    {
        [SerializeField] private Transform rootViews;
        [SerializeField] private DialogsController dialogsController;
        [SerializeField] private ScreensController screenSystem;

        public void Initialize()
        {
            dialogsController.Initialize(rootViews);
            screenSystem.Initialize(rootViews);
        }

        public T Show<T>() where T : UIView, new()
        {
            // ReSharper disable once Unity.IncorrectMonoBehaviourInstantiation
            switch (new T())
            {
                case DialogBase _:
                    return dialogsController.Show<T>();
                case ScreenBase _:
                    return screenSystem.Show<T>();
                default:
                    throw new Exception("Incorrect type: " + typeof(T));
            }
        }

        public void Close<T>() where T : UIView, new()
        {
            // ReSharper disable once Unity.IncorrectMonoBehaviourInstantiation
            switch (new T())
            {
                case DialogBase _:
                    dialogsController.Close<T>();
                    break;
                case ScreenBase _:
                    screenSystem.Close<T>();
                    break;
                default:
                    throw new Exception("Incorrect type: " + typeof(T));
            }
        }
    }
}