using UnityEngine;

namespace Puzzle2DSystem
{

    public abstract class PuzzleDataVisualizer
    {
        public GameObject[] PuzzlePieces { get; protected set; }

        public abstract void SetPuzzleData(PuzzleData data);
        public abstract GameObject CreateBoardVisual();
        public abstract GameObject[] CreatePuzzlePiecesVisual();
    }

}