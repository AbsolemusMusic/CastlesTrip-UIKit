using UnityEngine;

namespace CT.UIKit
{
    public struct AnchorData
    {
        public Vector2 min;
        public Vector2 max;

        public AnchorData(Vector2 min, Vector2 max)
        {
            this.min = min;
            this.max = max;
        }
    }
}