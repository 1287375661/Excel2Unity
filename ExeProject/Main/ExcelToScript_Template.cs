namespace ExeProject
{
    public partial class ExcelToScript
    {
        //生成模板
        private static readonly string CODE_TEMPLATE = @"
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    #路径
    public partial class #类名 : IConfig
    {
        public static Dictionary<#ID, #类名> Alls = new Dictionary<#ID, #类名>();

#字段

        /// <summary>
        /// 查找方法
        /// </summary>
        public #类名 Find(#ID id)
        {
            return Alls.GetValueOrDefault(id, null);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init(string content)
        {
            Alls.Clear();
            var filedList = LitJson.JsonMapper.ToObject<List<Dictionary<string, string>>>(content);
            int cnt = filedList.Count;
            for (int i = 0; i < cnt; i++)
            {
                var config = new #类名();
                var objDict = filedList[i];
                foreach (var temp in objDict)
                {
                    SetProperty(config, temp.Key, temp.Value);
                }

                if (!Alls.TryAdd(config.id, config))
                {
                    Debug.LogError($""#类名 id 重复: {config.id} !"");
                }
            }
        }
    }
}
";

        private static readonly string CV_CODE_TEMPLATE = @"
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    #路径
    public partial class #类名 : IConfig
    {
#字段

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init(string content)
        {
            var filedList = LitJson.JsonMapper.ToObject<List<Dictionary<string, string>>>(content);
            int cnt = filedList.Count;
            for (int i = 0; i < cnt; i++)
            {
                var objDict = filedList[i];
                SetProperty(this, objDict[""CV""], objDict[""content""]);
            }
        }
    }
}
";
    }
}