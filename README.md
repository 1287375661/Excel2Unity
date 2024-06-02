# ExcelToUnity
工具说明:配置生成实体类和二进制数据,在Unity使用

环境:  
Unity版本2022.3.21  
VisualStudio版本2022  
SDK框架 .NET 6.0 及以上, 如果没安装SDK可能会导致工具闪退

### 1.目录里有[Unity工程](https://github.com/1287375661/ExcelToUnity/tree/main/UnityDemo)

运行场景在控制台可以看到数据打印

![image](https://github.com/1287375661/ExcelToUnity/assets/45592691/85d70472-ea74-4ba3-a335-3f6597e64c6e)

### 2.运行UnityDemo\配置工具\ExeProject.exe 读取配置,生成实例类和数据

![image](https://github.com/1287375661/ExcelToUnity/assets/45592691/0f718034-6654-4724-888d-dda1de772b3c)

### 3.生成路径UnityDemo\配置工具\SavePath.txt

![image](https://github.com/1287375661/ExcelToUnity/assets/45592691/d444a58c-ea54-49b5-8064-93171dcf5550)

### 4.配置填写规则

#### 普通表

![image](https://github.com/1287375661/ExcelToUnity/assets/45592691/0787d1bd-7418-4ab7-aca9-7ada90903ae2)

1.普通的配置表必须有id字段没有则不导出

2.必填部分如果没有,不会报错,但是不会导出对应的数据

#### 常量表

![image](https://github.com/1287375661/ExcelToUnity/assets/45592691/8be8fc6c-529f-4c70-89d8-780167dd69d3)

### 补充说明

生成的实体类加了[partial](https://learn.microsoft.com/zh-cn/previous-versions/wbx7zzdd(v=vs.80))修饰符,方便扩展
