using System;
using UnityEngine;

namespace GridSystem.Scripts
{
    [Serializable]
    public class GridNodeValue
    {
        [HideInInspector] public string name;
        public int row, column;
        public GridType gridType;
        public int leftEmptyCount;
        public int rightEmptyCount;
        public int upEmptyCount;
        public int downEmptyCount;
    
        public enum GridType
        {
            Empty,
            Full,
            Enemy,
            Increase
        }

    }
}