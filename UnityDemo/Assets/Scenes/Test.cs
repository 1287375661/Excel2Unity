using System;
using System.Reflection;
using Config;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        ConfigLoader.ReadAllConfigs();

        //测试
        foreach (var temp in WeaponConfig.Alls)
        {
            PrintClass(temp.Value);
        }
        Debug.Log("ValueConfig.TIME:" + ValueConfig.TIME);
        Debug.Log("ValueConfig.OPEN_NAME:" + ValueConfig.OPEN_NAME);
    }

    /// <summary>
    /// 输出字段名称
    /// </summary>
    static void PrintClass(object obj)
    {
        Type type = obj.GetType();
        FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        string result = "";
        foreach (FieldInfo field in fields)
        {
            string fieldName = field.Name;
            object fieldValue = field.GetValue(obj);
            result += $"{fieldName} = {fieldValue} \n";
        }
        Debug.Log($"Data From {type.Name}: \n {result}");
    }
}
