using System;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem.Scripts
{
    public class CustomGrid : MonoBehaviour
    {
        [SerializeField] private Vector3 startPoint;
        [SerializeField] private int rowCount;
        [SerializeField] private int columnCount;
        [SerializeField] private float gridNodeSize;
        [SerializeField] private List<GridNodeValue> gridNodeList = new List<GridNodeValue>();
        public int RowCount => rowCount;
        public int ColumnCount => columnCount;
        public float GridNodeSize => gridNodeSize;
        public Vector3 StartPoint => startPoint;
        public List<GridNodeValue> GridNodeList => gridNodeList;

        private void Awake()
        {
            
        }
    }
}
