using System;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    //D:\UnityProject\ExcelToUnity\Config\武器表.xlsx
		
    public partial class WeaponConfig : IConfig
    {
        public static Dictionary<string, WeaponConfig> Alls = new Dictionary<string, WeaponConfig>();

		/// <summary>
		/// 索引
		/// </summary>
		public string id { get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		public string name { get; set; }
		/// <summary>
		/// 技能
		/// </summary>
		public string skillName { get; set; }
		

        /// <summary>
        /// 查找方法
        /// </summary>
        public WeaponConfig Find(string id)
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
                var config = new WeaponConfig();
                var objDict = filedList[i];
                foreach (var temp in objDict)
                {
                    SetProperty(config, temp.Key, temp.Value);
                }

                if (!Alls.TryAdd(config.id, config))
                {
                    Debug.LogError($"WeaponConfig id 重复: {config.id} !");
                }
            }
        }
    }
}