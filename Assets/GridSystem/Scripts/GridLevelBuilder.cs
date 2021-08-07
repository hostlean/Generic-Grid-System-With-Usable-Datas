using System;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem.Scripts
{
    public class GridLevelBuilder : MonoBehaviour
    {
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private int rowCount;
        [SerializeField] private int columnCount;
        [SerializeField] private float gridCellSize;
        [SerializeField] private List<CellWithMoveValues> gridNodeList = new List<CellWithMoveValues>();
        public int RowCount => rowCount;
        public int ColumnCount => columnCount;
        public float GridCellSize => gridCellSize;
        public Vector3 StartPosition => startPosition;
        public List<CellWithMoveValues> GridNodeList => gridNodeList;
        
    }
}
