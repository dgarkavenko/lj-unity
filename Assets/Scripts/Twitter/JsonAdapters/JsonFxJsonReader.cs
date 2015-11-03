#define USE_JSON_FX
#if USE_JSON_FX

using JsonFx.Json;
using System;

public class JsonFxJsonReader : IJsonReader
{
    public void Read(string json, Type resultType, Action<object> onFinished)
    {
        throw new NotSupportedException();
    }

    public void Read<T>(string json, Action<T> onFinished)
    {
        onFinished(new JsonReader().Read<T>(json));
    }

    public bool SupportsGenericType
    {
        get { return true; }
    }
}

#endif