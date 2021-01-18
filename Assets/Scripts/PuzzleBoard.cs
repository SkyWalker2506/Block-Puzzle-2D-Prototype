using UnityEngine;

namespace Puzzle2DSystem
{
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

}