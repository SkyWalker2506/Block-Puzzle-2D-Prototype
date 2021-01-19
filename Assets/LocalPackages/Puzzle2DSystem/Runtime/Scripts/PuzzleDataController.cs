using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Puzzle2DSystem
{
    public class PuzzleDataController
    {
        public PuzzleData PuzzleData { get; private set; }
        List<TriangleData> allTriangles = new List<TriangleData>();
        List<TriangleData> availableTriangles = new List<TriangleData>();

        int maxPuzzlePieceCount;
        int maxTriangleCount;

        public PuzzleDataController(int boardSize, int puzzlePieceCount)
        {
            PuzzleData = new PuzzleData();
            PuzzleData.PuzzleBoard = new PuzzleBoard(boardSize);
            this.maxPuzzlePieceCount = puzzlePieceCount;
            this.maxTriangleCount = boardSize * boardSize;
            PuzzleData.PuzzlePieces = new List<PuzzlePiece>();
            foreach (var contactPoint in PuzzleData.PuzzleBoard.ContactPoints)
            {
                allTriangles.AddRange(contactPoint.Triangles);
            }
            CreateMainPuzzlePieces(puzzlePieceCount);

            AssignRestOfTheLeftAvailableTrianglesToPuzzlePiece();
        }

        void CreateMainPuzzlePieces(int puzzlePieceCount)
        {
            availableTriangles = allTriangles;
            var triangles = availableTriangles.ToArray();
            foreach (var triangle in triangles)
            {
                triangle.SetMyNeighbours(triangles);
            }

            //Extra 25 try for if piece is too small.
            for (int i = 0; i < puzzlePieceCount + 25; i++)
            {
                if (PuzzleData.PuzzlePieces.Count >= puzzlePieceCount)
                    break;
                CreatePuzzlePiece();
            }
        }

        void CreatePuzzlePiece()
        {
            if (availableTriangles.Count == 0)
                return;
            ReOrderAvailableTriangles();
            int index = -1;
            for (int i = 0; i < availableTriangles.Count; i++)
            {
                if(availableTriangles[i].GetAvailableNeighbours().Count>0)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
                return;
            List<TriangleData> triangleDatas = new List<TriangleData>();
            triangleDatas.Add(availableTriangles[index]);

            var randomNeighbour = triangleDatas[0].GetRandomAvailableNeighbour();

            for (int i = 0; i < maxTriangleCount; i++)
            {
                randomNeighbour = triangleDatas[i].GetRandomAvailableNeighbour();

                if (randomNeighbour == null)
                    break;
                triangleDatas.Add(randomNeighbour);
            }

            if(triangleDatas.Count > PuzzleData.PuzzleBoard.BoardSize)
            {
                triangleDatas.ForEach(t => availableTriangles.Remove(t));
                var puzzlePiece = new PuzzlePiece(triangleDatas);
                PuzzleData.PuzzlePieces.Add(puzzlePiece);
            }

        }

        void AssignRestOfTheLeftAvailableTrianglesToPuzzlePiece()
        {
            while(availableTriangles.Count>0)
            {
                AssignLeftAvailableTrianglesToTheSmallestAvailablePuzzlePiece();
            }
        }

        void AssignLeftAvailableTrianglesToTheSmallestAvailablePuzzlePiece()
        {
            if (availableTriangles.Count == 0)
                return;
            ReOrderAvailableTriangles();

            int index = -1;
            for (int i = 0; i < availableTriangles.Count; i++)
            {
                if (availableTriangles[i].GetNeighboursWithOwners().Count > 0)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
                return;

            var neighbourOwners = availableTriangles[index].GetNeighbourOwners().OrderBy(owner => owner.TriangleDatas.Count()).ToList();
            AddTriangleToPuzzlePiece(availableTriangles[index], neighbourOwners[0]);

        }

        private void ReOrderAvailableTriangles()
        {
            availableTriangles = availableTriangles.OrderBy(a => Guid.NewGuid()).ToList();
        }

        void AddTriangleToPuzzlePiece(TriangleData triangleData,PuzzlePiece puzzlePiece)
        {
            puzzlePiece.AddTriangle(triangleData);
            availableTriangles.Remove(triangleData);
        }

    }

}