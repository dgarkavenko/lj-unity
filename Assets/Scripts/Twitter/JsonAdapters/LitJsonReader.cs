//#define USE_LIT_JSON
#if USE_LIT_JSON

using LitJson;
using System;

public class LitJsonReader : MonoBehaviour, IJsonReader
{
	public void Read(string json, Type resultType, Action<object> onFinished)
	{
		throw new NotSupportedException();
	}

	public void Read<T>(string json, Action<T> onFinished)
	{
		onFinished(JsonMapper.ToObject<T>(json));
	}

	public bool SupportsGenericType
	{
		get { return true; }
	}
}

#endif