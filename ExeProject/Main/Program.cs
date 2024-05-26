using System.Text;
using System.Text.Json;
using UnityEngine;

namespace ExeProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Generate();//创建数据
            
            //Config.ConfigLoader.ReadAllConfigs();//读取数据 ，必须先执行Generate() 

            Debug.Log("执行结束");
        }

        private static void Generate()
        {
            var savePath = ReadSavePath();
            if (savePath is null)
                return;

            var files = Utility.GetAllFiles(savePath.Excel_Path);

            //脚步
            ExcelToScript.Load(files);
            ExcelToScript.Save(savePath.Scripts_Path);

            Console.WriteLine();

            //配置数据
            ExcelToData.Load(files);
            ExcelToData.Save(savePath.Data_Path);
        }

        private static SavePath ReadSavePath()
        {
            SavePath savePath;

            string fileName = "SavePath.txt";

            if (!File.Exists(fileName))
            {
                //默认路径
                string json = JsonSerializer.Serialize(new SavePath
                {
                    Excel_Path = "Config",
                    Scripts_Path = "ConfigScript",
                    Data_Path = "ConfigData"
                });
                File.WriteAllText(fileName, json);
                Console.WriteLine("配置路径文件不存在,已重新创建");
                return null;
            }
            else
            {
                var content = File.ReadAllText(fileName);
                try
                {
                    savePath = JsonSerializer.Deserialize<SavePath>(content);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.ReadKey();
                    throw;
                }
            }

            CheckDirectory(savePath);
            return savePath;
        }

        private static void CheckDirectory(SavePath savePath)
        {
            if (savePath is null) return;

            if (!Directory.Exists(savePath.Excel_Path))
                Directory.CreateDirectory(savePath.Excel_Path);

            if (!Directory.Exists(savePath.Scripts_Path))
                Directory.CreateDirectory(savePath.Scripts_Path);

            if (!Directory.Exists(savePath.Data_Path))
                Directory.CreateDirectory(savePath.Data_Path);
        }

    }
}
