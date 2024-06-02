# ExcelToUnity
工具说明:配置生成实体类和二进制数据,在Unity使用

环境:  
Unity版本2022.3.21  
VisualStudio版本2022  
SDK框架 .NET 6.0 及以上, 如果没安装SDK可能会导致工具闪退

### 1.目录里有Unity工程,运行Test场景在控制台可以看到数据打印

![image](https://github.com/1287375661/ExcelToUnity/assets/45592691/4c3c1aec-855f-4edf-9593-9e01588088ac)

### 2.生成路径UnityDemo\配置工具\SavePath.txt

![image](https://github.com/1287375661/ExcelToUnity/assets/45592691/d444a58c-ea54-49b5-8064-93171dcf5550)

### 3.运行UnityDemo\配置工具\ExeProject.exe 读取配置,生成实例类和数据

![image](https://github.com/1287375661/ExcelToUnity/assets/45592691/0f718034-6654-4724-888d-dda1de772b3c)

### 4配置填写说明

#### 普通表

![image](https://github.com/1287375661/ExcelToUnity/assets/45592691/0787d1bd-7418-4ab7-aca9-7ada90903ae2)

1.普通的配置表必须有id字段没有则不导出

2.必填部分如果没有,不会报错,但是不会导出对应的数据

#### 常量表

![image](https://github.com/1287375661/ExcelToUnity/assets/45592691/8be8fc6c-529f-4c70-89d8-780167dd69d3)
