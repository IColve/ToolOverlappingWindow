using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ToolOverlappingWindow : EditorWindow
{
    private List<ToolOverlappingData> _overlappingDataList;
    
    private GUIStyle _defaultTextField;
    
    private int _selectVerIndex = -1;
    private int _selectHorIndex = -1;
    
    [MenuItem("Tools/OverlappingWindow")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ToolOverlappingWindow), false, "OverlappingWindow");
    }

    private void Init()
    {
        if (_defaultTextField == null)
        {
            _defaultTextField = new GUIStyle(GUI.skin.box);
            _defaultTextField.normal.background = MakeTex(2, 2, new Color(1f, 0.11f, 0f)); // 设置背景颜色
        }
    }
    
    Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
    
    private void OnGUI()
    {
        Init();
        
        GUILayout.BeginVertical();

        if (GUILayout.Button("Import Data"))
        {
            ImportData();
        }

        if (GUILayout.Button("Export Data"))
        {
            ExportData();
        }
        
        GUILayout.EndVertical();
        
        if (_overlappingDataList == null || _overlappingDataList.Count == 0)
        {
            return;
        }

        Draw();
    }

    /// <summary>
    /// Rewrite to init _overlappingDataList
    /// </summary>
    private void ImportData()
    {

    }

    /// <summary>
    /// Rewrite to export _overlappingDataList
    /// </summary>
    private void ExportData()
    {
        
    }

    private void Draw()
    {
        GUILayout.BeginVertical();
        
        GUILayout.BeginHorizontal();
        GUILayout.TextArea("", GUILayout.Width(80), GUILayout.Height(80));
        
        for (int i = 0; i < _overlappingDataList.Count; i++)
        {
            var over = _overlappingDataList[i];
            
            GUILayout.TextArea(over.ShowName,GUILayout.Width(20),GUILayout.Height(80));
        }
        
        GUILayout.EndHorizontal();
        
        for (int i = 0; i < _overlappingDataList.Count; i++)
        {
            var overVer = _overlappingDataList[i];
            GUILayout.BeginHorizontal();
            GUILayout.TextArea(_overlappingDataList[i].ShowName, GUILayout.Width(80),GUILayout.Height(20));
            
            for (int j = 0; j < _overlappingDataList.Count; j++)
            {
                var overHor = _overlappingDataList[j];
                bool isToggle = overVer.OverlappingIDList.Contains(overHor.ID);
                bool isSelect = GUILayout.Toggle(isToggle, "", GUILayout.Width(20), GUILayout.Height(20));
                
                if (_overlappingDataList.IndexOf(overHor) == _selectHorIndex)
                {
                    GUI.backgroundColor = Color.red;
                    Rect rect = GUILayoutUtility.GetLastRect();
                    GUI.Box(rect, GUIContent.none);
                    GUI.backgroundColor = Color.white;
                }
                
                Event currentEvent = Event.current;

                if (currentEvent != null)
                {
                    Rect toggleRect = GUILayoutUtility.GetLastRect();
                    if (toggleRect.Contains(currentEvent.mousePosition))
                    {
                        _selectVerIndex = _overlappingDataList.IndexOf(overVer);
                        _selectHorIndex = _overlappingDataList.IndexOf(overHor);
                    }
                }
                
                if (isSelect && !overVer.OverlappingIDList.Contains(overHor.ID))
                {
                    overVer.OverlappingIDList.Add(overHor.ID);
                }
                
                if (isSelect && !overHor.OverlappingIDList.Contains(overVer.ID))
                {
                    overHor.OverlappingIDList.Add(overVer.ID);
                }

                if (!isSelect && overVer.OverlappingIDList.Contains(overHor.ID))
                {
                    overVer.OverlappingIDList.Remove(overHor.ID);
                }
                
                if (!isSelect && overHor.OverlappingIDList.Contains(overVer.ID))
                {
                    overHor.OverlappingIDList.Remove(overVer.ID);
                }
            }
            GUILayout.EndHorizontal();
            if (_overlappingDataList.IndexOf(overVer) == _selectVerIndex)
            {
                GUI.backgroundColor = Color.red;
                Rect rect = GUILayoutUtility.GetLastRect();
                GUI.Box(rect, GUIContent.none);
                GUI.backgroundColor = Color.white;
            }
        }
        
        GUILayout.EndVertical();        
    }
}

public class ToolOverlappingData
{
    public string ShowName;
    public int ID;
    public List<int> OverlappingIDList;
}
