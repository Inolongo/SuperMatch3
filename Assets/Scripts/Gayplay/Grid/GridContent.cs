using UnityEngine;

namespace Gayplay.Grid
{
    public class GridContent : MonoBehaviour
    {
        [SerializeField] private Transform _upDot;
        [SerializeField] private Transform _downDot;
        [SerializeField] private Transform _leftDot;
        [SerializeField] private Transform _rightDot;
        
        private GridRect _gridRect;

        public GridRect GridRect
        {
            get
            {
                if (_gridRect.IsEmpty())
                {
                    _gridRect = new GridRect(_upDot, _downDot, _leftDot, _rightDot); 
                }

                return _gridRect;
            }
        }
    }
}