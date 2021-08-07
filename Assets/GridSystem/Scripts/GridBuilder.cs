using System;
using GridSystem.ScriptableObjects;
using UnityEngine;

namespace GridSystem.Scripts
{
    public class GridBuilder : MonoBehaviour
    {
        [Range(.01f, .1f)]
        [SerializeField] private float cellDistance = .01f;
        [SerializeField] private GridWithMoveValuesData gridWithMoveValuesData;
        
        private GenericGrid<CellWithMoveValues> grid;

        private void Awake()
        {
            grid = 
                // ReSharper disable once Unity.IncorrectMonoBehaviourInstantiation
                new GenericGrid<CellWithMoveValues>(
                    gridWithMoveValuesData.row, 
                    gridWithMoveValuesData.column, 
                    gridWithMoveValuesData.gridCellSize, 
                    gridWithMoveValuesData.startPosition);

            for (var i = 0; i < grid.GridArray.GetLength(0); i++)
            {
                for (var j = 0; j < grid.GridArray.GetLength(1); j++)
                {
                    var index = i *gridWithMoveValuesData.column + j;
                    var cell = gridWithMoveValuesData.gridNodeValues[index];
                    
                    grid.SetCellValue(i, j, cell);
                    
                    
                    
                    var go = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    var localScale = go.transform.localScale;
                    localScale = 
                        new Vector3(1, 1, 0) * grid.CellSize - 
                        new Vector3(1, 1, 0) * cellDistance * grid.CellSize;
                    
                    var scale = localScale;
                    localScale = new Vector3(scale.x, scale.y, .1f);
                    go.transform.localScale = localScale;

                    var pos = grid.GetWorldPosition(i, j);
                    go.transform.position = pos + new Vector3(1, 1,0) * grid.CellSize / 2;
                    
                    var meshRenderer = go.GetComponent<MeshRenderer>();
                    
                    switch (grid.GetValue(i,j).cellType)
                    {
                        case CellWithMoveValues.CellType.Empty:
                            break;
                        case CellWithMoveValues.CellType.Full:
                            meshRenderer.material.color = Color.black;
                            break;
                        case CellWithMoveValues.CellType.Enemy:
                            meshRenderer.material.color = Color.red;
                            break;
                        case CellWithMoveValues.CellType.Increase:
                            meshRenderer.material.color = Color.cyan;
                            break;
                        case CellWithMoveValues.CellType.StartPoint:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            var plane = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //plane.transform.rotation = Quaternion.Euler(-90, 0, 0);
            plane.transform.position = new Vector3(grid.Row * grid.CellSize / 2, grid.Column * grid.CellSize / 2, 0);
            plane.transform.localScale = new Vector3(grid.Row * grid.CellSize, grid.Column * grid.CellSize, .05f);
            plane.GetComponent<MeshRenderer>().material.color = Color.gray;

        }
    }
}