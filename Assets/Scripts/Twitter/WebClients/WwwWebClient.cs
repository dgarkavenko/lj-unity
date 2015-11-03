using System;
using System.Collections;
using UnityEngine;

public class WwwWebClient : MonoBehaviour, IWebClient
{
	public void RunRequest(Request request, Action<string> onFinished)
	{
		StartCoroutine(RequestCoroutine(request, onFinished));
	}

	private IEnumerator RequestCoroutine(Request request, Action<string> onFinished)
	{
		var www = new WWW(request.Url, request.RawData, request.Headers);
		yield return www;
		Debug.Log("result: " + www.text);
		onFinished(www.text);
	}
}