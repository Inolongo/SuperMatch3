using System;
using Gayplay.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Gayplay.GayplayGrid
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CellView : MonoBehaviour
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
        
        public void Init(CellDataModel cellDataModel, RectTransform decoyRectTransform)
        {
            rectTransform.offsetMax =
                new Vector2(decoyRectTransform.rect.width / 2, decoyRectTransform.rect.height / 2);
            rectTransform.offsetMin =
                new Vector2(-decoyRectTransform.rect.width / 2, -decoyRectTransform.rect.height / 2);
            rectTransform.localPosition = decoyRectTransform.localPosition;
            icon.sprite = cellDataModel.PieceIcon;
            face.sprite = cellDataModel.PieceFace;
        }

        public void Init(CellModel cellModel)
        {
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