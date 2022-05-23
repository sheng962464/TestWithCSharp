# 测试序列化Object

```C#
FileStream fs = new FileStream("test.xml", FileMode.Create);

Assembly _dll = Assembly.Load("VisionTools");
Type _type = _dll.GetType("Tools.VisionTools.GenRectangle1");
object obj = Activator.CreateInstance(_type);

Rule _rule = new Rule();
_rule._obj_list.Add(obj);

XmlSerializer sss = new XmlSerializer(typeof(Rule));
sss.Serialize(fs, _rule);

fs.Close();
```

这里首先有一个 `VisionTools.dll` ，其中有一个类 `GenRectangle1` ，

1. 通过反射机制读取该 `dll` 并新建 `GenRectangle1` 对象。
2. 将该对象加入到一个 `List<object>` 中
3. 将该 `List<object>` 保存到文件中

这里主要使用了 `XmlSerializer` 类来进行序列化保存对象。

