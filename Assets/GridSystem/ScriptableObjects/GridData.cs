using System.Collections.Generic;
using GridSystem.Scripts;
using UnityEngine;

namespace GridSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/GridData", order = 0)]
    public class GridData : ScriptableObject
    {
        public Vector3 startPosition;
        public List<GridNodeValue> gridNodeValues = new List<GridNodeValue>();
    }
}