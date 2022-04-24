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
        
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        public void Init(CellDataModel cellDataModel, RectTransform decoyRectTransform)
        {
            _rectTransform.offsetMax = new Vector2( decoyRectTransform.rect.width / 2, decoyRectTransform.rect.height / 2);
            _rectTransform.offsetMin = new Vector2( -decoyRectTransform.rect.width / 2, -decoyRectTransform.rect.height / 2);
            _rectTransform.localPosition = decoyRectTransform.localPosition;
            icon.sprite = cellDataModel.PieceIcon;
            face.sprite = cellDataModel.PieceFace;
        }
    }
}