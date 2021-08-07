using UnityEngine;

namespace GridSystem.Scripts
{
    public class GenericGrid <TGridCell> : MonoBehaviour
    {
        private readonly int _row;
        private readonly int _column;
        private readonly float _cellSize;
        private readonly Vector3 _startPos;
        private readonly TGridCell[,] _gridArray;
        
        public int Row => _row;

        public int Column => _column;

        public float CellSize => _cellSize;

        public Vector3 StartPos => _startPos;
        
        public TGridCell[,] GridArray => _gridArray;


        public GenericGrid(int row, int column, float cellSize, Vector3 startPos)
        {
            _row = row;
            _column = column;
            _cellSize = cellSize;
            _startPos = startPos;
            _gridArray = new TGridCell[row, column];
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * _cellSize + _startPos;
        }

        private void GetGridCellPosition(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - _startPos).x / _cellSize);
            y = Mathf.FloorToInt((worldPosition - _startPos).y / _cellSize);
        }

        public void SetCellValue(int x, int y, TGridCell value)
        {
            if (x >= 0 && y >= 0 && x < _row && y < _column)
                _gridArray[x, y] = value;
        }

        public void SetCellValue(Vector3 worldPosition, TGridCell value)
        {
            GetGridCellPosition(worldPosition, out var x, out var y);
            
            SetCellValue(x, y, value);
        }

        public TGridCell GetValue(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < _row && y < _row)
                return _gridArray[x, y];
            
            return default;
        }

        public TGridCell GetValue(Vector3 worldPosition)
        {
            GetGridCellPosition(worldPosition, out var x, out var y);
            
            return GetValue(x, y);
        }

       
    }
}