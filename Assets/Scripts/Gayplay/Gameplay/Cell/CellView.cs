using System;
using MVC;
using UnityEngine;
using UnityEngine.UI;

namespace Gayplay.Gameplay.Cell
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CellView : MonoBehaviour, IView
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image face;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private CanvasGroup canvasGroup;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
        }
        #endif

        public void Initialize(IModel model)
        {
            if (model is not CellModel cellModel)
            {
                throw new Exception("CellView.cs only work with CellModel.cs");
            }

            SetSize(cellModel.Size);

            if (cellModel.IsEmpty)
            {
                canvasGroup.alpha = 0;
            }
            else
            {
                icon.sprite = cellModel.PieceIcon;
                face.sprite = cellModel.PieceFace;
            }
        }

        private void SetSize(Vector2 size)
        {
            rectTransform.offsetMax = new Vector2(size.x / 2, size.y / 2);
            rectTransform.offsetMin = new Vector2(-size.x / 2, -size.y / 2);
        }
    }
}