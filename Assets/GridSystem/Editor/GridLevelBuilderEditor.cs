using System;
using System.Diagnostics.Eventing.Reader;
using GridSystem.ScriptableObjects;
using GridSystem.Scripts;
using UnityEditor;
using UnityEngine;

namespace GridSystem.Editor
{
    [CustomEditor(typeof(GridLevelBuilder))]
    public class GridLevelBuilderEditor : UnityEditor.Editor
    {
        private GridLevelBuilder myScript;
        private SerializedProperty gridType;
        private bool gridIndexEmptyValuesCalculated;
        private GUIStyle textStyle = new GUIStyle();
        private void OnEnable()
        {
            myScript = (GridLevelBuilder)target;
            
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (myScript.GridNodeList != null && myScript.GridNodeList.Count > 0)
            {
                if(GUILayout.Button("Calculate Empty Positions")) 
                {
                    for (int i = 0; i < myScript.GridNodeList.Count; i++)
                    {
                        SetEmptyValuesToZero(i);
                        CalculateEmptyValues(i);
                    }
                    
                }
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                GUILayout.Space(20);
                
                if (GUILayout.Button("Save Values To ScriptableObject"))
                {
                    CreateAndSaveScriptableObject();
                }
            }
            
          
        }

        private void CreateAndSaveScriptableObject()
        {
            GridWithMoveValuesData asset = CreateInstance<GridWithMoveValuesData>();

            AssetDatabase.CreateAsset(asset, "Assets/GridSystem/GridDatas/GridWithMoveValuesData.asset");
            AssetDatabase.SaveAssets();

            asset.row = myScript.RowCount;
            asset.column = myScript.ColumnCount;
            asset.startPosition = myScript.StartPosition;
            asset.gridCellSize = myScript.GridCellSize;
            asset.gridNodeValues = myScript.GridNodeList;
    
            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }


        private void OnSceneGUI()
        {
            DrawGridLines();
           
            Handles.BeginGUI();
        
            GameObject go = Selection.activeObject as GameObject;

            for (int i = 0; i < myScript.RowCount; i++)
            {
                for (int j = 0; j < myScript.ColumnCount; j++)
                {
                    if(myScript.GridNodeList.Count < myScript.RowCount * myScript.ColumnCount)
                        myScript.GridNodeList.Add(new CellWithMoveValues());
                    
                   
                    var startPoint = new Vector3(i, j, 0) * myScript.GridCellSize + myScript.StartPosition;
                    startPoint += new Vector3(0, myScript.GridCellSize, 0);
                    var place = HandleUtility.WorldToGUIPoint(startPoint);
                    GUILayout.BeginArea(new Rect(place.x, place.y, 70, 350));
                    var rect = EditorGUILayout.BeginVertical();
                    GUI.color = Color.gray;
                    GUI.Box(rect, GUIContent.none);
                   
        
                    GUI.color = Color.white;
                    serializedObject.Update();
                  
                    int index = 0;
                    index = i * myScript.ColumnCount + j ;

                    ChangeTextColorByCellType(index);
                   
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    
                    GUILayout.Label($"{i}x{j} Node", textStyle);
                    
                    ShowCellTypeEnum(index);

                    ShowUpgradeTypeAndCount(index);

                    ShowEnemyCount(index);
                    
                    ShowMovementValues(index);

                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    myScript.GridNodeList[index].name = $"{i}x{j}";
                    myScript.GridNodeList[index].row = i;
                    myScript.GridNodeList[index].column = j;
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    
                    
                    EditorGUILayout.EndVertical();
                    GUILayout.EndArea();
                    
                    
                    serializedObject.ApplyModifiedProperties();
                }
            }

            
          
            Handles.EndGUI();
            
            

           
        }

        private void ShowEnemyCount(int index)
        {
            if (myScript.GridNodeList[index].cellType == CellWithMoveValues.CellType.Enemy)
            {
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                myScript.GridNodeList[index].enemyCount = EditorGUILayout.IntField(myScript.GridNodeList[index].enemyCount);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
            }
        }

        private void ShowUpgradeTypeAndCount(int index)
        {
            if (myScript.GridNodeList[index].cellType == CellWithMoveValues.CellType.Increase)
            {
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                myScript.GridNodeList[index].upgradeType =
                    (CellWithMoveValues.UpgradeType)EditorGUILayout.EnumPopup(myScript.GridNodeList[index].upgradeType);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                myScript.GridNodeList[index].upgradeCount = EditorGUILayout.IntField(myScript.GridNodeList[index].upgradeCount);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
            }
        }

        private void ChangeTextColorByCellType(int index)
        {
            switch (myScript.GridNodeList[index].cellType)
            {
                case CellWithMoveValues.CellType.Empty:
                    textStyle.normal.textColor = Color.yellow;
                    break;
                case CellWithMoveValues.CellType.Full:
                    textStyle.normal.textColor = Color.magenta;
                    break;
                case CellWithMoveValues.CellType.Enemy:
                    textStyle.normal.textColor = Color.red;
                    break;
                case CellWithMoveValues.CellType.Increase:
                    textStyle.normal.textColor = Color.cyan;
                    break;
                case CellWithMoveValues.CellType.StartPoint:
                    textStyle.normal.textColor = Color.green;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ShowCellTypeEnum(int index)
        {
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            myScript.GridNodeList[index].cellType =
                (CellWithMoveValues.CellType)EditorGUILayout.EnumPopup(myScript.GridNodeList[index].cellType);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
        }

        private void ShowMovementValues(int index)
        {
            if (myScript.GridNodeList[index].cellType == CellWithMoveValues.CellType.Full)
            {
                textStyle.normal.textColor = Color.gray;
            }
            else
            {
                textStyle.normal.textColor = Color.white;
            }

            GUILayout.Label($"L: {myScript.GridNodeList[index].leftEmptyCount} " +
                            $"R: {myScript.GridNodeList[index].rightEmptyCount} {Environment.NewLine}" +
                            $"U: {myScript.GridNodeList[index].upEmptyCount} " +
                            $"D: {myScript.GridNodeList[index].downEmptyCount}", textStyle);
        }

        private void SetEmptyValuesToZero(int i)
        {
            var activeNode = myScript.GridNodeList[i];
            activeNode.downEmptyCount = 0;
            activeNode.leftEmptyCount = 0;
            activeNode.rightEmptyCount = 0;
            activeNode.upEmptyCount = 0;
        }

        private void CalculateEmptyValues(int gridNodeIndex)
        {
            if(myScript.GridNodeList[gridNodeIndex].cellType == CellWithMoveValues.CellType.Full)
            {
                SetEmptyValuesToZero(gridNodeIndex);
            }
            else
            {
                CalculateDownCount(gridNodeIndex);
                CalculateUpCount(gridNodeIndex);
                CalculateLeftCount(gridNodeIndex);
                CalculateRightCount(gridNodeIndex);
            }
        }

        private void CalculateDownCount(int gridNodeIndex)
        {
            if (myScript.GridNodeList[gridNodeIndex].column == 0)
            {
                myScript.GridNodeList[gridNodeIndex].downEmptyCount = 0;
            }
            else
            {
                for (int i = 1; i < myScript.GridNodeList[gridNodeIndex].column + 1; i++)
                {
                    var groundNode = myScript.GridNodeList.Find(gn =>
                        gn.row == myScript.GridNodeList[gridNodeIndex].row &&
                        gn.column == myScript.GridNodeList[gridNodeIndex].column - i);
                    
                    if(groundNode.cellType == CellWithMoveValues.CellType.Full)
                        break;
                    
                    myScript.GridNodeList[gridNodeIndex].downEmptyCount += 1;
                }
            }
        }

        private void CalculateUpCount(int gridNodeIndex)
        {
            if (myScript.GridNodeList[gridNodeIndex].column == myScript.ColumnCount - 1)
            {
                myScript.GridNodeList[gridNodeIndex].upEmptyCount = 0;
            }
            else
            {
                for (int i = myScript.GridNodeList[gridNodeIndex].column + 1; i < myScript.RowCount; i++)
                {
                    if(i == myScript.RowCount) break;
                    var groundNode = myScript.GridNodeList.Find(gn =>
                        gn.row == myScript.GridNodeList[gridNodeIndex].row &&
                        gn.column == i);
                    
                    if(groundNode.cellType == CellWithMoveValues.CellType.Full)
                        break;
                    
                    myScript.GridNodeList[gridNodeIndex].upEmptyCount += 1;
                }
            }
        }

        private void CalculateLeftCount(int gridNodeIndex)
        {
            if (myScript.GridNodeList[gridNodeIndex].row == 0)
            {
                myScript.GridNodeList[gridNodeIndex].leftEmptyCount = 0;
            }
            else
            {
                for (int i = 1; i < myScript.GridNodeList[gridNodeIndex].row + 1; i++)
                {
                    var groundNode = myScript.GridNodeList.Find(gn =>
                        gn.column == myScript.GridNodeList[gridNodeIndex].column &&
                        gn.row == myScript.GridNodeList[gridNodeIndex].row - i);
                    
                    if(groundNode.cellType == CellWithMoveValues.CellType.Full)
                        break;

                    myScript.GridNodeList[gridNodeIndex].leftEmptyCount += 1;
                }
            }
        }

        private void CalculateRightCount(int gridNodeIndex)
        {
            if (myScript.GridNodeList[gridNodeIndex].row == myScript.RowCount - 1)
            {
                myScript.GridNodeList[gridNodeIndex].rightEmptyCount = 0;
            }
            else
            {
                for (int i = myScript.GridNodeList[gridNodeIndex].row + 1; i < myScript.ColumnCount; i++)
                {
                    if(i == myScript.ColumnCount) break;
                    var groundNode = myScript.GridNodeList.Find(gn =>
                        gn.column == myScript.GridNodeList[gridNodeIndex].column &&
                        gn.row == i);
                    
                    if(groundNode.cellType == CellWithMoveValues.CellType.Full)
                        break;
                    
                    myScript.GridNodeList[gridNodeIndex].rightEmptyCount += 1;
                }
            }
        }

        private void DrawGridLines()
        {
            Vector3[] lineSegmentsX = new Vector3[myScript.RowCount * myScript.ColumnCount];
            Vector3[] lineSegmentsY = new Vector3[myScript.RowCount * myScript.ColumnCount];
            for (int i = 0; i < myScript.RowCount; i++)
            {
                for (int j = 0; j < myScript.ColumnCount; j++)
                {
                    var startPointX = new Vector3(i, j, 0) * myScript.GridCellSize + myScript.StartPosition;
                    var endPointX = new Vector3(i + 1, j, 0) * myScript.GridCellSize + myScript.StartPosition;
                    var endPointY = new Vector3(i, j + 1, 0) * myScript.GridCellSize + myScript.StartPosition;
                    Handles.DrawLine(startPointX, endPointX);
                    Handles.DrawLine(startPointX, endPointY);
                    
                }
            }
            var lastXLinePosStart = new Vector3(0, myScript.ColumnCount, 0) * myScript.GridCellSize + myScript.StartPosition;
            var lastYLinePosStart = new Vector3(myScript.RowCount, 0, 0) * myScript.GridCellSize + myScript.StartPosition;
            var lastLinePosEnd = new Vector3(myScript.RowCount, myScript.ColumnCount, 0) * myScript.GridCellSize + myScript.StartPosition;
            Handles.DrawLine(lastYLinePosStart, lastLinePosEnd);
            Handles.DrawLine(lastXLinePosStart, lastLinePosEnd);
            
            
        }
    }
}