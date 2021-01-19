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
            return GetRandomPuzzleDataController().PuzzleData;
        }

        PuzzleDataController GetRandomPuzzleDataController()
        {
            int boardSize = Random.Range(minBoardSize, maxBoardSize + 1);
            int pieceCount = Random.Range(minPuzzlePieceCount, maxPuzzlePieceCount + 1);
            return new PuzzleDataController(boardSize, pieceCount);
        }


    }

}