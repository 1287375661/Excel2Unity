using System;

namespace Config
{
    public class IConfig
    {
        //private void OnLoadEnd;//当所有配置初始化后

        /// <summary>
        /// 设置字段属性
        /// </summary>
        protected void SetProperty(object target, string propertyName, object propertyValue)
        {
            //获取属性集合
            var property = target.GetType().GetProperty(propertyName);
            if (property == null)
            {
                UnityEngine.Debug.LogError($"属性获取失败 {this}:{propertyName}");
                return;
            }
            object value = Convert.ChangeType(propertyValue, property.PropertyType);
            property.SetValue(target, value, null);
        }

    }
}