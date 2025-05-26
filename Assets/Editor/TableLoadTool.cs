using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Pool;

namespace QQ
{
    public class TableLoadTool : EditorWindow
    {
        private const string tableInfoURL = "https://docs.google.com/spreadsheets/d/1olWDigkJMlbBc2eDZrFEG93oQJJ1n9OnsdghVSHp2_s/edit?usp=sharing";
        
        // ���� ��Ʈ ����
        private List<SheetInfo> sheetInfos;
        private struct SheetInfo
        {
            public TableType type;
            public string address;
            public string range;
            public string sheetID;
        }

        // ���� ���̺� ����(IDataManage�� �����ϴ�)
        private List<IDataManager> localTables;

        // ���� ��Ʈ �ּ� �ҷ��Դ��� ����
        private bool isReady = false;
        // ���� ��Ʈ �ҷ������� �ε�
        private bool isSheetTableLoading = false;
        // ���̺� ����� �ҷ������� �ε�
        private bool isLocalTableLoading = false;

        // Tool ��ư bool ��
        private bool ActiveBtnTableLocalLoad = true;
        private bool ActiveBtnTableSave = true;

        [MenuItem("Tools/Load Table Data")]
        public static void ShowWindow()
        {
            GetWindow<TableLoadTool>("Table Load Tool");
        }

        private void CreateGUI()
        {
            isReady = false;
            isSheetTableLoading = false;
            isLocalTableLoading = false;

            // ���� ��Ʈ �ӽ� ���� �ʱ�ȭ
            if (sheetInfos != null)
            {
                ListPool<SheetInfo>.Release(sheetInfos);
                sheetInfos = null;
            }
            sheetInfos = ListPool<SheetInfo>.Get();

            // ���� ���̺� ���� �ӽ� ���� �ʱ�ȭ
            if(localTables != null)
            {
                ListPool<IDataManager>.Release(localTables);
                localTables = null;
            }
            localTables = ListPool<IDataManager>.Get();

            GetTableAssembly();

        }

        private void OnGUI()
        {
            if (GUILayout.Button("TEST"))
            {
                LanguageDataManager.Instance.LoadData();
            }

            GUILayout.Label($"Current State <<isLocalTableLoading : {isLocalTableLoading}>> <<IsReady : {isReady}>> <<IsTableLoading :{isSheetTableLoading}>>", EditorStyles.boldLabel);

            GUI.enabled = ActiveBtnTableLocalLoad;
            if (GUILayout.Button("GetTableDataUrl") && !isSheetTableLoading && !isLocalTableLoading)
            {
                isSheetTableLoading = true;
                ActiveBtnTableLocalLoad = false;

                TableLoadData().Forget();
            }

            if (isReady)
            {
                for (int i = 0; i < sheetInfos.Count; i++)
                {
                    GUILayout.Label($"Type : {sheetInfos[i].type.ToString()}", EditorStyles.boldLabel);

                    GUI.enabled = ActiveBtnTableSave;
                    if (GUILayout.Button("Load And Save Data"))
                    {
                        ActiveBtnTableLocalLoad = false;
                        LoadAndSaveData(sheetInfos[i].type, TableDataManager.GetGoogleSheetAddress(sheetInfos[i].address, 
                                                       sheetInfos[i].range,
                                                       sheetInfos[i].sheetID)).Forget();
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (sheetInfos != null)
            {
                ListPool<SheetInfo>.Release(sheetInfos);
                sheetInfos = null;
            }
        }

        public async UniTaskVoid TableLoadData()
        {
            using (UnityWebRequest www = UnityWebRequest.Get(TableDataManager.GetGoogleSheetAddress(tableInfoURL, "A2:D2", "0")))
            {
                www.timeout = 60;

                await www.SendWebRequest();

                while (!www.isDone)
                {
                    await UniTask.Yield();
                }

                if (www.result == UnityWebRequest.Result.ConnectionError ||
                    www.result == UnityWebRequest.Result.ProtocolError)
                {
                    ActiveBtnTableLocalLoad = true;

                    Debug.LogError(www.error);
                }
                else
                {
                    string data = www.downloadHandler.text;

                    string[] rows = data.Split('\n');

                    for (int i = 0; i < rows.Length; i++)
                    {
                        string[] columns = rows[i].Split('\t');

                        sheetInfos.Add(new SheetInfo
                        {
                            type = (TableType)int.Parse(columns[0]),
                            address = columns[1],
                            range = columns[2],
                            sheetID = columns[3],
                        });
                    }

                    isReady = true;
                }

                isSheetTableLoading = false;
            }
        }

        public async UniTaskVoid LoadAndSaveData(TableType type, string url)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                www.timeout = 60;

                await www.SendWebRequest();

                while (!www.isDone)
                {
                    await UniTask.Yield();
                }

                if (www.result == UnityWebRequest.Result.ConnectionError ||
                    www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(www.error);
                }
                else
                {
                    string data = www.downloadHandler.text;

                    if (FIndTable(type) != null)
                    {
                        FIndTable(type).SaveData(type, data);
                    }
                    else
                        Debug.LogError($"{type}�� �ش��ϴ� Assembly ����");
                }

                ActiveBtnTableSave = true;
            }
        }

        /// <summary>
        /// IDataManager�� �����ϴ� ��� ���̺��� ã�� ���� ���̺� ����Ʈ�� �߰�
        /// </summary>
        private void GetTableAssembly()
        {
            isLocalTableLoading = true;

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
                            localTables.Add(instance);
                        }
                    }
                }
            }

            isLocalTableLoading = false;
        }

        private IDataManager FIndTable(TableType type)
        {
            foreach(IDataManager data in localTables)
            {
                if (data.GetTableType() == type)
                {
                    return data;
                }
            }

            return null;
        }
    }
}
