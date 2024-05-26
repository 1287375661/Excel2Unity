using System.Data;
using System.Text.Json;
using UnityEngine;

namespace ExeProject
{
    public class ExcelToData
    {
        //所有表的数据
        //key1:表名称  value1:表数据(key2:ID)
        private static Dictionary<string, Dictionary<string, CacheConfig>> AllConfigs = new Dictionary<string, Dictionary<string, CacheConfig>>();

        public static void Load(List<string> files)
        {
            Debug.Log("数据生成中...");

            Release();

            int cnt = files.Count;
            for (int i = 0; i < cnt; i++)
            {
                ConvertToConfig(files[i]);
            }
        }

        private static void ConvertToConfig(string file)
        {
            var dataSet = Utility.ReadExcel(file);
            foreach (DataTable mSheet in dataSet.Tables)
            {
                //读取数据表行数和列数
                if (!Utility.CheckHasConfig(mSheet, out var rowCount, out var colCount))
                    continue;

                string configName = mSheet.Rows[0][0].ToString();
                string idName = mSheet.Rows[1][1].ToString();
                bool hasId = idName != "id" && idName != "CV";
                if (string.IsNullOrEmpty(configName) || hasId)
                    continue; //没有表头 不导入

                //缓存一张表的数据
                for (int i = 3; i < rowCount; i++)
                {
                    //存储一行的数据
                    string? first = mSheet.Rows[i][0].ToString();//第一列为NO跳过
                    if (!string.Equals("NO", first, StringComparison.OrdinalIgnoreCase))
                    {
                        var config = new CacheConfig();
                        for (int j = 1; j < colCount; j++)
                        {
                            //读取第1行数据作为表头字段
                            var field = mSheet.Rows[1][j].ToString();
                            var value = mSheet.Rows[i][j].ToString();
                            config.Set(field, value);
                        }
                        //添加到表数据中
                        if (!TryAddConfig(configName, config))
                        {
                            Debug.LogWarning($"路径:{file}, {configName}.id => {config.idValue} 重复");
                        }
                    }

                    if (string.Equals("END", first, StringComparison.OrdinalIgnoreCase))
                        break; //读取截止
                }
                mSheet.Clear();
            }
        }

        private static bool TryAddConfig(string configName, CacheConfig config)
        {
            if (!AllConfigs.TryGetValue(configName, out var dict))
            {
                dict = new Dictionary<string, CacheConfig>();
                AllConfigs.Add(configName, dict);
            }
            return dict.TryAdd(config.idValue, config);
        }

        public static void Save(string exportPath)
        {
            if (AllConfigs.Count == 0)
            {
                Release();
                Debug.Log("没有可生成数据");
                return;
            }

            string fileName = $"AllConfigs.bytes";

            string filePath = Path.Combine(exportPath, fileName);
            //Dictionary<string, Dictionary<string, CacheConfig>>
            using FileStream fs = new FileStream(filePath, FileMode.Create);
            using BinaryWriter writer = new BinaryWriter(fs);
            // 读取字典的大小
            int count = AllConfigs.Count;
            writer.Write(count);

            // 读取字典的键值对
            foreach (var temp in AllConfigs)
            {
                var configList = new List<Dictionary<string, string>>();
                var configDict = temp.Value;
                foreach (var config in configDict.Values)
                {
                    configList.Add(config.fieldDict);
                }

                writer.Write(temp.Key);
                writer.Write(JsonSerializer.Serialize(configList));
            }
            Release();
            Debug.Log($"保存成功:{fileName}");
        }

        private static void Release()
        {
            AllConfigs.Clear();
        }

        //临时储存一行配置的信息
        public class CacheConfig
        {
            public string idValue = "";

            //key 字段名称 value 字段值
            public Dictionary<string, string> fieldDict = new Dictionary<string, string>();

            public void Set(string key, string value)
            {
                // id普通表 CV常量表
                if (key == "id" || key == "CV")
                    idValue = value;

                fieldDict[key] = value;
            }
        }
    }
}