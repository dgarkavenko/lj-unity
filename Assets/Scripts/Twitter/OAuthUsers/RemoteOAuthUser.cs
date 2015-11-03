using System;
using System.Collections.Generic;

public class RemoteOAuthUser : IOAuthUser
{
    private readonly IWebClient _webClient;

    public RemoteOAuthUser(IWebClient webClient)
    {
        _webClient = webClient;
    }

    public void SignRequest(string httpMethod, string baseUrl, Dictionary<string, string> requestParams, Action<string> onFinished)
    {
        if (httpMethod == "GET") {

        } else {
            
        }

        _webClient.RunRequest(new Request(new Dictionary<string, string>(), baseUrl, null), onFinished);
    }
}
