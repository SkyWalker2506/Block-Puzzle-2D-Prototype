using System.Collections.Generic;
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
            return new PuzzleData(boardSize,maxPuzzlePieceCount);
        }
    }

   // [System.Serializable]
    public class PuzzleData
    {
        public PuzzleBoard PuzzleBoard { get; }
        public List<PuzzlePiece> PuzzlePieces { get; private set; }
        List<TriangleData> availableTriangles; 
        List<TriangleData> notAvailableNeighbourTriangles;
        List<TriangleData> usedTriangles;
        int maxPuzzlePieceCount;
        int maxTriangleCount;

        public PuzzleData(int boardSize, int maxPuzzlePieceCount, int maxTriangleCount = 25)
        {
            PuzzleBoard  = new PuzzleBoard(boardSize);
            this.maxPuzzlePieceCount = maxPuzzlePieceCount;
            this.maxTriangleCount = boardSize* boardSize*4;
            PuzzlePieces = new List<PuzzlePiece>();
            availableTriangles = new List<TriangleData>();
            notAvailableNeighbourTriangles = new List<TriangleData>();
            usedTriangles = new List<TriangleData>();
            Debug.Log("contactPoints " + PuzzleBoard.ContactPoints.Length);
            foreach (var contactPoint in PuzzleBoard.ContactPoints)
            {
                availableTriangles.AddRange(contactPoint.Triangles);
            }
            var triangles = availableTriangles.ToArray();
            foreach (var triangle in triangles)
            {
                triangle.SetMyNeighbours(triangles);
            }

            for (int i = 0; i < maxPuzzlePieceCount; i++)
            {
                CreatePuzzlePiece();
            }
        }

        void CreatePuzzlePiece()
        {
            Debug.Log("availableTriangles "+ availableTriangles.Count);
            if (availableTriangles.Count == 0)
                return;

            List<TriangleData> triangleDatas = new List<TriangleData>();
            triangleDatas.Add(availableTriangles[0]);
            var randomNeighbour = triangleDatas[0].GetRandomAvailableNeighbour();
            if (randomNeighbour == null)
                return;
            triangleDatas.Add(randomNeighbour);

            for (int i = 1; i < maxTriangleCount; i++)
            {
                randomNeighbour = triangleDatas[i].GetRandomAvailableNeighbour();
                if (randomNeighbour == null)
                    break;
                triangleDatas.Add(randomNeighbour);
            }

            var puzzlePiece = new PuzzlePiece(triangleDatas.ToArray());
            PuzzlePieces.Add(puzzlePiece);

            foreach (var triangleData in triangleDatas)
            {
                availableTriangles.Remove(triangleData);
                usedTriangles.Add(triangleData);
            }
            SetNoAvailableNeighbourTriangles();
        }

        void SetNoAvailableNeighbourTriangles()
        {
            var newNotAvailableNeighbourTriangles = new List<TriangleData>();
            foreach (var triangle in availableTriangles)
            {
                if(triangle.GetAvailableNeighbours().Count==0)
                {
                    newNotAvailableNeighbourTriangles.Add(triangle);
                }
            }
                    notAvailableNeighbourTriangles.AddRange(newNotAvailableNeighbourTriangles);
                    newNotAvailableNeighbourTriangles.ForEach(t=>availableTriangles.Remove(t));
        }


    }

    [System.Serializable]
    public class PuzzlePiece
    {
        public TriangleData[] TriangleDatas { get; }
        public ContactPoint[] ContactPoints { get; }

        public PuzzlePiece(TriangleData[] triangleDatas)
        {
            foreach (var triangleData in triangleDatas)
            {
                triangleData.Owner = this;
            }
            TriangleDatas = triangleDatas;
            var contactPoints = new List<ContactPoint>();
            foreach (var triangleData in TriangleDatas)
            {
                if (!contactPoints.Contains(triangleData.ContactPoint))
                    contactPoints.Add(triangleData.ContactPoint);
            }
            ContactPoints = contactPoints.ToArray();
        }
    }

    [System.Serializable]
    public class PuzzleBoard
    {
        public ContactPoint[] ContactPoints { get; }
        public int BoardSize { get; }

        public PuzzleBoard(int boardSize)
        {
            BoardSize = boardSize * 2;
            var rawCount = BoardSize;
            var columnCount = BoardSize;
            ContactPoints = new ContactPoint[boardSize*boardSize];
            var index = 0;
            for (int i = 1; i < rawCount; i+=2)
            {
                for (int j = 1; j < columnCount; j+=2)
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
        public TriangleData[] Neighbours { get; private set; }
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

        public void SetMyNeighbours(TriangleData[] neighbourCandidates)
        {
            List<TriangleData> neighbours = new List<TriangleData>();

            foreach (var candidate in neighbourCandidates)
            {
                if (IsMyNeighbour(candidate))
                    neighbours.Add(candidate);
            }
            Neighbours = neighbours.ToArray();
        }

        public List<TriangleData> GetAvailableNeighbours()
        {
            var availableNeighbours = new List<TriangleData>();
            foreach (var neighbour in Neighbours)
            {
                if (neighbour.Owner == null)
                    availableNeighbours.Add(neighbour);
            }
            return availableNeighbours;
        }

        public TriangleData GetRandomAvailableNeighbour()
        {
            TriangleData randomNeighbour = null;
            var availableNeighbours = GetAvailableNeighbours();
            if (availableNeighbours.Count > 0)
                randomNeighbour = availableNeighbours[Random.Range(0, availableNeighbours.Count)];
                return randomNeighbour;
        }


        bool IsMyNeighbour(TriangleData neighbourCandidate)
        {
            int commonPointCount = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Points[i] == neighbourCandidate.Points[j])
                    {
                        commonPointCount++;
                        break;
                    }
                }
            }
            return commonPointCount == 2;
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
                Triangles[i] = new TriangleData(this,new Vector2[] { Position, Position+clockwisePoints[i], Position+clockwisePoints[(i+1) % 8] });
            }
        }

    }

}