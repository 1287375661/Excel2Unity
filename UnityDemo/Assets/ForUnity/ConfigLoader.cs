using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Config
{
    /// <summary>
    /// 在Unity 通过生成的AllConfigs.bytes二进制文件,赋值给Config脚本
    /// </summary>
    public class ConfigLoader
    {
        static string fileName = "D:\\UnityProject\\ExcelToUnity\\UnityDemo\\Assets\\ConfigData\\AllConfigs.bytes";

        public static void ReadAllConfigs()
        {
            if (!File.Exists(fileName))
            {
                Debug.Log($"路径不存在:{fileName}");
                return;
            }

            var tempList = new List<IConfig>();
            using (FileStream mStream = new FileStream(fileName, FileMode.Open))
            {
                using (BinaryReader mReader = new BinaryReader(mStream))
                {
                    // 读取字典的大小
                    int count = mReader.ReadInt32();

                    // 读取字典的键值对
                    for (int i = 0; i < count; i++)
                    {
                        string className = mReader.ReadString();
                        string contentJson = mReader.ReadString();
                        var config = CreateInstance<IConfig>($"Config.{className}");
                        var method = config.GetType().GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Instance);
                        method?.Invoke(config, new object[] { contentJson });
                        tempList.Add(config);
                    }

                    // 所有配置加载完后
                    int cnt = tempList.Count;
                    for (int i = 0; i < cnt; i++)
                    {
                        var config = tempList[i];
                        var method = config.GetType().GetMethod("OnLoadEnd", BindingFlags.NonPublic | BindingFlags.Instance);
                        method?.Invoke(config, null);
                    }
                }
            }
        }

        //创建实例
        static T CreateInstance<T>(string className) where T : class
        {
            Type? type = Type.GetType(className);
            return Activator.CreateInstance(type) as T;
        }
    }
}
