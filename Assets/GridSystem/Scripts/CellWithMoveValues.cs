using System;
using UnityEngine;

namespace GridSystem.Scripts
{
    [Serializable]
    public class CellWithMoveValues
    {
        [HideInInspector] public string name;
       
        
        public int row, column;
        public CellType cellType;
        public UpgradeType upgradeType;
        public int enemyCount;
        public int upgradeCount;
        public int leftEmptyCount;
        public int rightEmptyCount;
        public int upEmptyCount;
        public int downEmptyCount;
      
    
        public enum CellType
        {
            Empty,
            Full,
            Enemy,
            Increase,
            StartPoint
        }

        public enum UpgradeType
        {
            Additive,
            Multiple
        }

    }
}