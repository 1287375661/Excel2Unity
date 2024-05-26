using System.Data;
using System.Diagnostics;
using ExcelDataReader;

namespace ExeProject
{
    public class Utility
    {
        //获取表数据
        public static DataSet ReadExcel(string excelFile)
        {
            using FileStream mStream = File.Open(excelFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
            return mExcelReader.AsDataSet(); //表格数据集合
        }

        //配置表是否有数据
        public static bool CheckHasConfig(DataTable mSheet, out int rowCount, out int colCount)
        {
            //读取数据表行数和列数
            rowCount = mSheet.Rows.Count;
            colCount = mSheet.Columns.Count;

            //判断数据表内是否存在配置 前面3行是注释,字段,类型
            if (rowCount < 3) return false;

            int maxColCount = 0;
            //第二行 列数量
            for (int i = 1; i < colCount; i++)
            {
                string fieldName = mSheet.Rows[1][i].ToString();
                if (!string.IsNullOrEmpty(fieldName))
                    maxColCount = i;
            }

            colCount = maxColCount;

            if (colCount == 0) return false;

            return true;
        }

        //代码运行时长
        public static void RunningTime(Action action)
        {
            Stopwatch stopwatch = new Stopwatch();

            // Start measuring time
            stopwatch.Start();

            // 这里放置你要测量的代码
            action.Invoke();

            // Stop measuring time
            stopwatch.Stop();

            // 获取耗时
            TimeSpan elapsedTime = stopwatch.Elapsed;

            // 输出耗时
            UnityEngine.Debug.Log($"代码执行时间: {elapsedTime.TotalSeconds} 毫秒");

        }

        public static List<string> GetAllFiles(string path)
        {
            var allFiles = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            int cnt = allFiles.Length;
            var configFiles = new List<string>();

            for (int i = 0; i < cnt; i++)
            {
                string file = allFiles[i];
                if (file.First() != '~')
                    configFiles.Add(file);
            }
            return configFiles;
        }

        public static void DeleteAllFiles(string path)
        {
            try
            {
                // 删除当前目录中的所有文件
                foreach (string file in Directory.GetFiles(path))
                {
                    File.Delete(file);
                }

                // 递归处理所有子目录
                foreach (string directory in Directory.GetDirectories(path))
                {
                    DeleteAllFiles(directory);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                UnityEngine.Debug.LogError($"访问被拒绝: {ex.Message}");
            }
            catch (PathTooLongException ex)
            {
                UnityEngine.Debug.LogError($"路径太长: {ex.Message}");
            }
            catch (IOException ex)
            {
                UnityEngine.Debug.LogError($"IO错误: {ex.Message}");
            }
        }
    }
}