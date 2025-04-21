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
        
        // 구글 시트 정보
        private List<SheetInfo> sheetInfos;
        private struct SheetInfo
        {
            public TableType type;
            public string address;
            public string range;
            public string sheetID;
        }

        // 로컬 테이블 정보(IDataManage를 구현하는)
        private List<IDataManager> localTables;

        // 구글 시트 주소 불러왔는지 여부
        private bool isReady = false;
        // 구글 시트 불러오는중 로딩
        private bool isSheetTableLoading = false;
        // 테이블 어셈블리 불러오는중 로딩
        private bool isLocalTableLoading = false;

        // Tool 버튼 bool 값
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

            // 구글 시트 임시 변수 초기화
            if (sheetInfos != null)
            {
                ListPool<SheetInfo>.Release(sheetInfos);
                sheetInfos = null;
            }
            sheetInfos = ListPool<SheetInfo>.Get();

            // 로컬 테이블 정보 임시 변수 초기화
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
                        LoadAndSaveData(sheetInfos[i].type, GetGoogleSheetAddress(sheetInfos[i].address, 
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
            using (UnityWebRequest www = UnityWebRequest.Get(GetGoogleSheetAddress(tableInfoURL, "A2:D2", "0")))
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
                        FIndTable(type).SaveData(data);
                    }
                    else
                        Debug.LogError($"{type}에 해당하는 Assembly 없음");
                }

                ActiveBtnTableSave = true;
            }
        }

        /// <summary>
        /// 복사해온 URL 에서 edit?usp=sharing 제거후 제거된 부분에 export?format=tsv&range=시트 범위&gid=시트ID값
        /// </summary>
        /// <param name="address">https:// ~ /</param>
        /// <param name="range">구글 시트 Range</param>
        /// <param name="sheetID">시트 고유 ID</param>
        /// <returns></returns>
        public static string GetGoogleSheetAddress(string address, string range, string sheetID)
        {
            if (address.Contains("edit?usp=sharing"))
            {
                return address.Replace("edit?usp=sharing", $"export?format=tsv&range={range}&gid={sheetID}");
            }
            else
                return StringBuilderPool.Get(address, "/export?format=tsv&range={range}&gid={sheetID}");
        }

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
