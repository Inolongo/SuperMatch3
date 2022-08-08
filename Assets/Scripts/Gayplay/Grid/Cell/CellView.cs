using System;
using MVC;
using UnityEngine;

namespace Gayplay.Gameplay.Cell
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CellView : MonoBehaviour, IView
    {
        [SerializeField] private SpriteRenderer icon;
        [SerializeField] private SpriteRenderer face;

        private SpriteRenderer _spriteRenderer;

        public void Initialize(IModel model)
        {
            if (model is not CellModel cellModel)
            {
                throw new Exception("CellView.cs only work with CellModel.cs");
            }
            
            _spriteRenderer ??= GetComponent<SpriteRenderer>();

            SetSize(cellModel.Size);

            if (cellModel.IsEmpty)
            {
                HideCell();
            }
            else
            {
                icon.sprite = cellModel.PieceIcon;
                face.sprite = cellModel.PieceFace;
            }
        }

        private void HideCell()
        {
            
        }

        private void SetSize(Vector2 size)
        {
            var bounds = _spriteRenderer.bounds;
            var xSizeCoefficient = size.x / bounds.size.x;
            var ySizeCoefficient = size.y / bounds.size.y;

            transform.localScale 
                = new Vector3(
                    transform.localScale.x * xSizeCoefficient, 
                    transform.localScale.y * ySizeCoefficient, 
                    transform.localScale.z);
        }
    }
}