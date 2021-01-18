using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Puzzle2DSystem
{
    [System.Serializable]
    public class TriangleData
    {
        public ContactPoint ContactPoint { get; }
        public Vector2 Position { get; }
        public Vector2[] Points { get; }
        public TriangleData[] Neighbours { get; private set; }
        public bool CheckingForPuzzlePiece;
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
                if (neighbour.Owner == null&& !neighbour.CheckingForPuzzlePiece)
                    availableNeighbours.Add(neighbour);
            }
            return availableNeighbours;
        }

        public List<TriangleData> GetNeighboursWithOwners()
        {
            var neighboursWithOwners = Neighbours.ToList();
            GetAvailableNeighbours().ForEach(n => neighboursWithOwners.Remove(n));
            return neighboursWithOwners;
        }

        public List<PuzzlePiece> GetNeighbourOwners()
        {
            var neighboursWithOwners = GetNeighboursWithOwners();
            List<PuzzlePiece> owners= new List<PuzzlePiece>();
            for (int i = 0; i < neighboursWithOwners.Count; i++)
            {
                var owner = neighboursWithOwners[i].Owner;
                if (!owners.Contains(owner))
                    owners.Add(owner);
            }
            return owners;
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

}