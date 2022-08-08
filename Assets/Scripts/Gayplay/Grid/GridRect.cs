using UnityEngine;

namespace Gayplay.Grid
{
    public readonly struct GridRect
    {
        public Transform UpDot { get; }
        public Transform DownDot { get; }
        public Transform LeftDot { get; }
        public Transform RightDot { get; }

        public float Height => UpDot.position.y - DownDot.position.y;
        public float Weight => RightDot.position.y - LeftDot.position.y;

        public GridRect(Transform upDot, Transform downDot, Transform leftDot, Transform rightDot)
        {
            UpDot = upDot;
            DownDot = downDot;
            LeftDot = leftDot;
            RightDot = rightDot;
        }

        public bool IsEmpty()
        {
            var isEmpty = UpDot == null || DownDot == null || LeftDot == null || RightDot == null;

            return isEmpty;
        }
    }
}