using UnityEngine;

namespace Puzzle2DSystem
{

    public abstract class PuzzleDataVisualizer
    {
        public GameObject[] PuzzlePieces { get; protected set; }


        public abstract GameObject CreateBoardVisual();
        public abstract GameObject[] CreatePuzzlePiecesVisual();
    }

}