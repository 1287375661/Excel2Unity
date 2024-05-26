using System;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    //D:\UnityProject\ExcelRead\Config\常量表.xlsx
		
    public partial class ValueConfig : IConfig
    {
		/// <summary>
		/// 测试1
		/// </summary>
		public static string OPEN_NAME { get; set; }
		/// <summary>
		/// 测试2
		/// </summary>
		public static int TIME { get; set; }
		

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
                SetProperty(this, objDict["CV"], objDict["content"]);
            }
        }
    }
}