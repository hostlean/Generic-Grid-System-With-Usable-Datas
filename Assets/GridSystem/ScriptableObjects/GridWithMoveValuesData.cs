using System.Collections.Generic;
using GridSystem.Scripts;
using UnityEngine;

namespace GridSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/GridData", order = 0)]
    public class GridWithMoveValuesData : ScriptableObject
    {
        public int row;
        public int column;
        public Vector3 startPosition;
        public float gridCellSize;
        public List<CellWithMoveValues> gridNodeValues = new List<CellWithMoveValues>();
    }
}