using System.Data;
using System.Text;
using UnityEngine;

namespace ExeProject
{
    /// <summary>
    ///     配置的规则:
    ///     1.第一列第一个是类名
    ///     2.第二列是ID,类型和字段都要填
    ///     3.以上不填不会导出脚本
    /// </summary>
    public partial class ExcelToScript
    {
        #region Prop
        //key1:表名称  value1:脚本内容
        private static Dictionary<string, CacheConfig> AllConfigs = new Dictionary<string, CacheConfig>();

        //key:表名称  value:路径
        private static Dictionary<string, IList<string>> AllPaths = new Dictionary<string, IList<string>>();//移入 ExcelToScript

        private static StringBuilder stringBuilder = new();
        #endregion

        /// <summary>
        /// Excel => Scripte
        /// </summary>
        public static void Load(List<string> files)
        {
            Debug.Log("脚步生成中...");

            int cnt = files.Count;
            for (int i = 0; i < cnt; i++)
            {
                try
                {
                    ParseConfig(files[i]);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
        }

        /// <summary>
        /// 解析配置
        /// </summary>
        private static void ParseConfig(string configPath)
        {
            var dataSet = Utility.ReadExcel(configPath);
            //判断Excel文件中是否存在数据表
            foreach (DataTable mSheet in dataSet.Tables)
            {
                //读取数据表行数和列数
                if (!Utility.CheckHasConfig(mSheet, out var rowCount, out var colCount))
                    continue;

                string configName = mSheet.Rows[0][0].ToString();
                string idName = mSheet.Rows[1][1].ToString();
                string idType = mSheet.Rows[2][1].ToString();
                bool hasId = idName != "id" && idName != "CV";
                if (string.IsNullOrEmpty(configName) || hasId)
                    continue;

                AddPath(configName, configPath);
                if (AllConfigs.ContainsKey(configName))
                    continue;

                CacheConfig config = new CacheConfig(configName, idName, idType);

                if (idName == "id")
                {
                    for (int i = 1; i < colCount; i++)
                    {
                        //读取第1行数据作为表头字段
                        string desc = mSheet.Rows[0][i].ToString();
                        string field = mSheet.Rows[1][i].ToString();
                        string type = mSheet.Rows[2][i].ToString();
                        config.AddProperty(desc, field, type);
                    }
                }
                else if (idName == "CV")
                {
                    for (int i = 3; i < rowCount; i++)
                    {
                        string desc = mSheet.Rows[i][4].ToString();
                        string field = mSheet.Rows[i][1].ToString();
                        string type = mSheet.Rows[i][2].ToString();
                        config.AddProperty(desc, field, type);
                    }
                }

                AllConfigs[configName] = config;
            }
        }

        private static string GenerateCode(CacheConfig config)
        {
            if (config.idName == "id")
            {
                return GenerateIDCode(config);
            }
            else
            {
                return GenerateCVCode(config);
            }
        }

        /// <summary>
        /// 普通表 生成代码
        /// </summary>
        private static string GenerateIDCode(CacheConfig config)
        {
            //路径
            string pathContent = "";
            if (AllPaths.TryGetValue(config.configName, out var pathList))
            {
                stringBuilder.Clear();
                foreach (var path in pathList)
                {
                    stringBuilder.Append($"//{path}\r\n\t\t");
                }
                pathContent = stringBuilder.ToString();
            }

            //字段
            int cnt = config.propertyList.Count;
            stringBuilder.Clear();
            stringBuilder.Append("\t\t");
            for (var i = 0; i < cnt; i++)
            {
                var property = config.propertyList[i];
                stringBuilder.Append($"/// <summary>\r\n\t\t/// {property.desc}\r\n\t\t/// </summary>\r\n\t\t");
                stringBuilder.Append($"public {property.type} {property.field} {{ get; set; }}\r\n\t\t");
            }
            string propertyContent = stringBuilder.ToString();

            stringBuilder.Clear();
            stringBuilder.Append(CODE_TEMPLATE.Trim());
            stringBuilder.Replace("#路径", pathContent);
            stringBuilder.Replace("#类名", config.configName);
            stringBuilder.Replace("#ID", config.idType);
            stringBuilder.Replace("#字段", propertyContent);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 常量表
        /// </summary>
        private static string GenerateCVCode(CacheConfig config)
        {
            //路径
            string pathContent = "";
            if (AllPaths.TryGetValue(config.configName, out var pathList))
            {
                stringBuilder.Clear();
                foreach (var path in pathList)
                {
                    stringBuilder.Append($"//{path}\r\n\t\t");
                }
                pathContent = stringBuilder.ToString();
            }

            //字段
            int cnt = config.propertyList.Count;
            stringBuilder.Clear();
            stringBuilder.Append("\t\t");
            for (var i = 0; i < cnt; i++)
            {
                var property = config.propertyList[i];
                stringBuilder.Append($"/// <summary>\r\n\t\t/// {property.desc}\r\n\t\t/// </summary>\r\n\t\t");
                stringBuilder.Append($"public static {property.type} {property.field} {{ get; set; }}\r\n\t\t");
            }
            string propertyContent = stringBuilder.ToString();

            stringBuilder.Clear();
            stringBuilder.Append(CV_CODE_TEMPLATE.Trim());
            stringBuilder.Replace("#路径", pathContent);
            stringBuilder.Replace("#类名", config.configName);
            stringBuilder.Replace("#字段", propertyContent);
            return stringBuilder.ToString();
        }

        private static void AddPath(string configName, string file)
        {
            if (string.IsNullOrEmpty(configName))
                return;

            if (!AllPaths.TryGetValue(configName, out var list))
            {
                list = new List<string>();
                AllPaths.Add(configName, list);
            }

            list.Add(file);
        }

        /// <summary>
        /// 输出文件
        /// </summary>
        public static void Save(string exportPath)
        {
            if (AllConfigs.Count == 0)
            {
                Release();
                Debug.Log("没有可生成数据");
                return;
            }

            Utility.DeleteAllFiles(exportPath);

            foreach (var temp in AllConfigs)
            {
                string className = $"{temp.Key}.cs";
                string code = GenerateCode(temp.Value);
                string filePath = Path.Combine(exportPath, className);
                File.WriteAllText(filePath, code);//保存
                Debug.Log(className);
            }

            Release();
        }

        private static void Release()
        {
            stringBuilder.Clear();
            AllConfigs.Clear();
            AllPaths.Clear();
        }

        private class CacheConfig
        {
            public string idType { get; private set; }
            public string idName { get; private set; }
            public string configName { get; private set; }

            public CacheConfig(string configName, string idName, string idType)
            {
                this.idName = idName;
                this.idType = idType;
                this.configName = configName;
            }

            //描述 字段 类型
            public List<(string desc, string field, string type)> propertyList = new List<(string desc, string field, string type)>();

            public void AddProperty(string desc, string field, string type)
            {
                propertyList.Add(new ValueTuple<string, string, string>(desc, field, type));
            }
        }
    }
}