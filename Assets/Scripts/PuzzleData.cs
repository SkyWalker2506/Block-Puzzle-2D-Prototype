using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Puzzle2DSystem
{
    public class PuzzleData
    {
        public PuzzleBoard PuzzleBoard { get; }
        public List<PuzzlePiece> PuzzlePieces { get; private set; }
        List<TriangleData> allTriangles = new List<TriangleData>();
        List<TriangleData> availableTriangles = new List<TriangleData>();

        int maxPuzzlePieceCount;
        int maxTriangleCount;

        public PuzzleData(int boardSize, int puzzlePieceCount)
        {
            PuzzleBoard  = new PuzzleBoard(boardSize);
            this.maxPuzzlePieceCount = puzzlePieceCount;
            this.maxTriangleCount = boardSize* boardSize;
            PuzzlePieces = new List<PuzzlePiece>();
            Debug.Log("contactPoints " + PuzzleBoard.ContactPoints.Length);
            foreach (var contactPoint in PuzzleBoard.ContactPoints)
            {
                allTriangles.AddRange(contactPoint.Triangles);
            }
            availableTriangles = allTriangles;
            var triangles = availableTriangles.ToArray();
            foreach (var triangle in triangles)
            {
                triangle.SetMyNeighbours(triangles);
            }

            //Extra 25 try for if piece is too small
            for (int i = 0; i < puzzlePieceCount+25; i++)
            {
                if (PuzzlePieces.Count >= puzzlePieceCount)
                    break;
                CreatePuzzlePiece();
            }

            AssignRestOfTheLeftAvailableTrianglesToPuzzlePiece();
        }

        void CreatePuzzlePiece()
        {
            if (availableTriangles.Count == 0)
                return;
            availableTriangles= availableTriangles.OrderBy(a => Guid.NewGuid()).ToList();
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
            availableTriangles[index].CheckingForPuzzlePiece = true;

            var randomNeighbour = triangleDatas[0].GetRandomAvailableNeighbour();

            for (int i = 0; i < maxTriangleCount; i++)
            {
                randomNeighbour = triangleDatas[i].GetRandomAvailableNeighbour();

                if (randomNeighbour == null)
                    break;
                triangleDatas.Add(randomNeighbour);
                randomNeighbour.CheckingForPuzzlePiece = true;
            }

            if(triangleDatas.Count > PuzzleBoard.BoardSize)
            {
                triangleDatas.ForEach(t => availableTriangles.Remove(t));
                var puzzlePiece = new PuzzlePiece(triangleDatas);
                PuzzlePieces.Add(puzzlePiece);
            }
            else
            {
                triangleDatas.ForEach(t => t.CheckingForPuzzlePiece = false);
            }

        }


        void AssignRestOfTheLeftAvailableTrianglesToPuzzlePiece()
        {
            while(availableTriangles.Count>0)
            {
                AssignLeftAvailableTrianglesToPuzzlePiece();
            }
        }

        void AssignLeftAvailableTrianglesToPuzzlePiece()
        {
            if (availableTriangles.Count == 0)
                return;
            availableTriangles = availableTriangles.OrderBy(a => Guid.NewGuid()).ToList();

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
            for (int i = 0; i < neighbourOwners.Count; i++)
            {
                 Debug.Log("neighbourOwners[i].TriangleDatas"+ neighbourOwners[i].TriangleDatas.Count());
            }
            AddTriangleToPuzzlePiece(availableTriangles[index], neighbourOwners[0]);

            Debug.Log("availableTriangles " + availableTriangles.Count);
        }

        void AddTriangleToPuzzlePiece(TriangleData triangleData,PuzzlePiece puzzlePiece)
        {
            puzzlePiece.AddTriangle(triangleData);
            availableTriangles.Remove(triangleData);
        }

    }

}