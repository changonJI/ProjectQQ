using QQ;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class UIShowDepth : EditorWindow
{
    private Dictionary<string, UIDepth> uiDepths = new Dictionary<string, UIDepth>();

    [MenuItem("Tools/UIShowDepth")]
    public static void ShowWindow()
    {
        GetWindow<UIShowDepth>("UIShowDepth");
    }

    private void CreateGUI()
    {
        var localTable = Assembly.GetAssembly(typeof(UI));

        foreach (var type in localTable.GetTypes())
        {
            if (type.IsClass && typeof(UI).IsAssignableFrom(type) && type.ContainsGenericParameters == false)
            {
                Type baseType = type.BaseType;

                if (baseType != null && baseType != typeof(MonoBehaviour))
                {
                    var property = type.GetProperty("uiDepth");
                    if (property != null)
                    {
                        UI instance = (UI)Activator.CreateInstance(type);
                        object depthValue = property.GetValue(instance);

                        uiDepths.Add(type.Name, (UIDepth)Enum.Parse(typeof(UIDepth), depthValue.ToString()));
                    }
                }
            }
        }
    }

    private void OnGUI()
    {
        foreach(var key in uiDepths.Keys)
        {
            GUILayout.Label($"UI : {key}, Depth : {uiDepths[key]}", EditorStyles.boldLabel);
        }
        
    }

    private void OnDestroy()
    {
    }

    /// <summary>
    /// IDataManager를 구현하는 모든 테이블을 찾아 로컬 테이블 리스트에 추가
    /// </summary>
    private void GetTableAssembly()
    {
        var localTable = Assembly.GetAssembly(typeof(IDataManager));

        foreach (var type in localTable.GetTypes())
        {
            if (type.IsClass && type.GetInterface("IDataManager") != null)
            {
                Type baseType = type.BaseType;

                if (baseType != null)
                {
                    var property = baseType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
                    if (property != null)
                    {
                        IDataManager instance = (IDataManager)property.GetValue(null);
                    }
                }
            }
        }
    }
}
