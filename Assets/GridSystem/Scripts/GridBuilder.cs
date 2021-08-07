using System;
using GridSystem.ScriptableObjects;
using UnityEngine;

namespace GridSystem.Scripts
{
    public class GridBuilder : MonoBehaviour
    {

        [SerializeField] private GridWithMoveValuesData gridWithMoveValuesData;
        private void Awake()
        {
            GenericGrid<CellWithMoveValues> grid = 
                new GenericGrid<CellWithMoveValues>(
                    gridWithMoveValuesData.row, 
                    gridWithMoveValuesData.column, 
                    gridWithMoveValuesData.gridCellSize, 
                    gridWithMoveValuesData.startPosition);

            for (int i = 0; i < grid.GridArray.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GridArray.GetLength(1); j++)
                {
                    var pos = grid.GetWorldPosition(i, j);
                    var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go.transform.position = pos;
                }
            }

            Debug.Log(grid.GetValue(0, 1).cellType);
            Debug.Log(grid.GetValue(1, 1).leftEmptyCount);
            Debug.Log(grid.GetValue(2, 1).leftEmptyCount);
            
        }
    }
}