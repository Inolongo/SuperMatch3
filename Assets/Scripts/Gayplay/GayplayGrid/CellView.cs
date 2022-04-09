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

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        public void Init(CellDataModel cellDataModel)
        {
            if (!cellDataModel.IsActive)
            {
                _canvasGroup.alpha = 0;
                _canvasGroup.interactable = false;
            }

            icon.sprite = cellDataModel.PieceIcon;
            face.sprite = cellDataModel.PieceFace;
        }
    }
}