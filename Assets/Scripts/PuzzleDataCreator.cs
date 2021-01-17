using UnityEngine;

namespace Puzzle2DSystem
{
    public class PuzzleDataCreator 
    {
        int minBoardSize;
        int maxBoardSize;
        int minPuzzlePieceCount;
        int maxPuzzlePieceCount;

        public PuzzleDataCreator (int minBoardSize=4, int maxBoardSize=6, int minPuzzlePieceCount = 5, int maxPuzzlePieceCount = 12)
        {
            this.minBoardSize = minBoardSize;
            this.maxBoardSize = maxBoardSize;
            this.minPuzzlePieceCount = minPuzzlePieceCount;
            this.maxPuzzlePieceCount = maxPuzzlePieceCount;
        }

        public PuzzleData GetRandomPuzzleData()
        {
            int boardSize = Random.Range(minBoardSize, maxBoardSize + 1);
            return new PuzzleData(boardSize);
        }
    }

    [System.Serializable]
    public class PuzzleData
    {
        public PuzzleBoard PuzzleBoard { get; }
        public PuzzlePiece[] PuzzlePieces { get; }

        public PuzzleData(int boardSize)
        {
            PuzzleBoard  = new PuzzleBoard(boardSize);
        }

        void SetPuzzlePieces()
        {

        }

        
    }

    [System.Serializable]
    public class PuzzlePiece
    {
        public TriangleData[] TriangleDatas { get; }
        public Vector2[] ContactPoint { get; }

        public PuzzlePiece(TriangleData[] triangleDatas)
        {
            foreach (var triangleData in triangleDatas)
            {
                triangleData.Owner = this;
            }
            TriangleDatas = triangleDatas;
        }
    }

    [System.Serializable]
    public class PuzzleBoard
    {
        public int RawCount { get; }
        public int ColumnCount { get; }
        public ContactPoint[] ContactPoints { get; }

        public PuzzleBoard(int boardSize)
        {
            RawCount = boardSize * 2;
            ColumnCount = boardSize * 2;
            ContactPoints = new ContactPoint[boardSize*boardSize];
            var index = 0;
            for (int i = 1; i < RawCount; i+=2)
            {
                for (int j = 1; j < ColumnCount; j+=2)
                {
                    ContactPoints[index] = new ContactPoint(i, j);
                    Debug.Log(index + ": " + ContactPoints[index].Position);
                    index++;
                }
            }
        }

    }


    [System.Serializable]
    public class TriangleData
    {
        public ContactPoint ContactPoint { get; }
        public Vector2 Position { get; }
        public Vector2[] Points { get; }
        public PuzzlePiece Owner;

        public TriangleData(ContactPoint contactPoint, Vector2[] points)
        {
            ContactPoint = contactPoint;
            Points = points;
            if(Points.Length>0)
            {
                Position = Vector2.zero;
                for (int i = 0; i < Points.Length; i++)
                {
                    Position += Points[i];
                }
                Position /= Points.Length;
            }
        }
    }

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
                Triangles[i] = new TriangleData(this,new Vector2[] { Position, clockwisePoints[i], clockwisePoints[i % 8] });
            }
        }

    }

}