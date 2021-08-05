using System;
using System.Diagnostics.Eventing.Reader;
using GridSystem.ScriptableObjects;
using GridSystem.Scripts;
using UnityEditor;
using UnityEngine;

namespace GridSystem.Editor
{
    [CustomEditor(typeof(CustomGrid))]
    public class CustomGridEditor : UnityEditor.Editor
    {
        private CustomGrid myScript;
        private SerializedProperty gridType;
        private bool gridIndexEmptyValuesCalculated;
        private GUIStyle textStyle = new GUIStyle();
        private void OnEnable()
        {
            myScript = (CustomGrid)target;
            
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
            GridData asset = ScriptableObject.CreateInstance<GridData>();

            AssetDatabase.CreateAsset(asset, "Assets/GridSystem/GridDatas/GridData.asset");
            AssetDatabase.SaveAssets();

            asset.gridNodeValues = myScript.GridNodeList;
    
            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
        

        private void OnSceneGUI()
        {
            DrawGridLines();
           
        
            GameObject go = Selection.activeObject as GameObject;


            for (int i = 0; i < myScript.RowCount; i++)
            {
                for (int j = 0; j < myScript.ColumnCount; j++)
                {
                    if(myScript.GridNodeList.Count < myScript.RowCount * myScript.ColumnCount)
                        myScript.GridNodeList.Add(new GridNodeValue());
                    
                   
                    var startPoint = new Vector3(i, j, 0) * myScript.GridNodeSize + myScript.StartPoint;
                    startPoint += new Vector3(0, myScript.GridNodeSize, 0);
                    var place = HandleUtility.WorldToGUIPoint(startPoint);
                    GUILayout.BeginArea(new Rect(place.x, place.y, 70, 350));
                    var rect = EditorGUILayout.BeginVertical();
                    GUI.color = Color.gray;
                    GUI.Box(rect, GUIContent.none);
                   
        
                    GUI.color = Color.white;
                    serializedObject.Update();
                  
                    int index = 0;
                    index = i * myScript.ColumnCount + j ;
                   
                    if (myScript.GridNodeList[index].gridType == GridNodeValue.GridType.Empty)
                    {
                        textStyle.normal.textColor = Color.yellow;
                    }
                    else
                    {
                        textStyle.normal.textColor = Color.red;
                    }
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.Label($"{i}x{j} Node", textStyle);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    
                    myScript.GridNodeList[index].gridType = 
                        (GridNodeValue.GridType) EditorGUILayout.EnumPopup(myScript.GridNodeList[index].gridType);
                    
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (myScript.GridNodeList[index].gridType == GridNodeValue.GridType.Full)
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
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    // if (myScript.GridNodeList[index].gridType == GridNodeValue.GridType.Full)
                    // {
                    //     Handles.color = Color.red;
                    //     Handles.DrawWireDisc(startPoint, Vector3.back, myScript.GridNodeSize);
                    // }
                    // else
                    // {
                    //     Handles.color = Color.green;
                    //     Handles.DrawSolidDisc(startPoint, Vector3.back, myScript.GridNodeSize);
                    // }
                    
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
            if (myScript.GridNodeList[gridNodeIndex].gridType == GridNodeValue.GridType.Empty)
            {
                CalculateDownCount(gridNodeIndex);
                CalculateUpCount(gridNodeIndex);
                CalculateLeftCount(gridNodeIndex);
                CalculateRightCount(gridNodeIndex);
            }

            else
            {
                SetEmptyValuesToZero(gridNodeIndex);
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
                    if (groundNode.gridType == GridNodeValue.GridType.Empty)
                    {
                        myScript.GridNodeList[gridNodeIndex].downEmptyCount += 1;
                    }
                    else
                    {
                        break;
                    }
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
                    if (groundNode.gridType == GridNodeValue.GridType.Empty)
                    {
                        myScript.GridNodeList[gridNodeIndex].upEmptyCount += 1;
                    }
                    else
                    {
                        break;
                    }
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
                    if (groundNode.gridType == GridNodeValue.GridType.Empty)
                    {
                        myScript.GridNodeList[gridNodeIndex].leftEmptyCount += 1;
                    }
                    else
                    {
                        break;
                    }
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
                    if (groundNode.gridType == GridNodeValue.GridType.Empty)
                    {
                        myScript.GridNodeList[gridNodeIndex].rightEmptyCount += 1;
                    }
                    else
                    {
                        break;
                    }
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
                    var startPointX = new Vector3(i, j, 0) * myScript.GridNodeSize + myScript.StartPoint;
                    var endPointX = new Vector3(i + 1, j, 0) * myScript.GridNodeSize + myScript.StartPoint;
                    var endPointY = new Vector3(i, j + 1, 0) * myScript.GridNodeSize + myScript.StartPoint;
                    Handles.DrawLine(startPointX, endPointX);
                    Handles.DrawLine(startPointX, endPointY);
                    
                }
            }
            var lastXLinePosStart = new Vector3(0, myScript.ColumnCount, 0) * myScript.GridNodeSize + myScript.StartPoint;
            var lastYLinePosStart = new Vector3(myScript.RowCount, 0, 0) * myScript.GridNodeSize + myScript.StartPoint;
            var lastLinePosEnd = new Vector3(myScript.RowCount, myScript.ColumnCount, 0) * myScript.GridNodeSize + myScript.StartPoint;
            Handles.DrawLine(lastYLinePosStart, lastLinePosEnd);
            Handles.DrawLine(lastXLinePosStart, lastLinePosEnd);
            
            
        }
    }
}