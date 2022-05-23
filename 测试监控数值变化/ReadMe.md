# 测试监控数值变化

这里是为了在程序中完成「当某个值发生了改变，就执行某函数」的功能。

```C#
public int mValue
{
    get
    {
        return _value;
    }
    set
    {
        if(_value != value)
        {
            if (OnMyValueChanged != null)
            {
                OnMyValueChanged();
            }
        }
        _value = value;
    }
}
```

这里是利用了 `C#` 中的 `set` 属性实现的。