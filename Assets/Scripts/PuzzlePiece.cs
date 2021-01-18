using System.Collections.Generic;

namespace Puzzle2DSystem
{
    [System.Serializable]
    public class PuzzlePiece
    {
        public List<TriangleData> TriangleDatas { get; private set; }
        public List<ContactPoint> ContactPoints { get; private set; }

        public PuzzlePiece(List<TriangleData> triangleDatas)
        {
            TriangleDatas = new List<TriangleData>();
            ContactPoints = new List<ContactPoint>();

            foreach (var triangleData in triangleDatas)
            {
                AddTriangle(triangleData);
            }
        }

        public void AddTriangle(TriangleData triangleData)
        {
            triangleData.Owner = this;
            triangleData.CheckingForPuzzlePiece = false;
            TriangleDatas.Add(triangleData);
            if (!ContactPoints.Contains(triangleData.ContactPoint))
                ContactPoints.Add(triangleData.ContactPoint);
        }
    }

}