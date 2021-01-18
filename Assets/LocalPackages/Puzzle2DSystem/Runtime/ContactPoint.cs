using UnityEngine;

namespace Puzzle2DSystem
{
    [System.Serializable]
    public class ContactPoint
    {
        public Vector2 Position { get; }
        public TriangleData[] Triangles { get; }
        Vector2[] clockwisePoints =
        {
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(1,0),
            new Vector2(1,-1),
            new Vector2(0,-1),
            new Vector2(-1,-1),
            new Vector2(-1,0),
            new Vector2(-1,1),
        };

        public ContactPoint (int x, int y)
        {
            Position = new Vector2(x, y);
            Triangles = new TriangleData[8];
            for (int i = 0; i < 8; i++)
            {
                Triangles[i] = new TriangleData(this,new Vector2[] { Position, Position+clockwisePoints[i], Position+clockwisePoints[(i+1) % 8] });
            }
        }

    }

}