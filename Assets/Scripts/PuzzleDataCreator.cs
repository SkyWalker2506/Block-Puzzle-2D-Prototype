using UnityEngine;

namespace Puzzle2DSystem
{
    public class PuzzleDataCreator 
    {
        int minBoardSize;
        int maxBoardSize;

        public PuzzleDataCreator (int minBoardSize=4, int maxBoardSize=6)
        {
            this.minBoardSize = minBoardSize;
            this.maxBoardSize = maxBoardSize;
        }



    }

    [System.Serializable]
    public class PuzzlePiece
    {
        public Vector2[] PieceData { get; }

        public PuzzlePiece(Vector2[] pieceData)
        {
            PieceData = pieceData;
        }

    }

}