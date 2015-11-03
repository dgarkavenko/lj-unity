using System;
using System.Collections.Generic;
using System.Linq;

public interface IWebClient
{
    void RunRequest(Request request, Action<string> onFinished);
}

public class Request
{
    public readonly Dictionary<string, string> Headers;
    public readonly string Url;
    public readonly byte[] RawData;

    public Request(Dictionary<string, string> headers, string url, byte[] rawData)
    {
        Headers = headers;
        Url = url;
        RawData = rawData;
    }
}

public interface IOAuthUser
{
    void SignRequest(string httpMethod, string baseUrl, Dictionary<string, string> requestParams, Action<string> onFinished);
}

public interface IJsonReader
{
    void Read(string json, Type resultType, Action<object> onFinished);
    void Read<T>(string json, Action<T> onFinished);
    bool SupportsGenericType { get; }
}

public class Twitter
{
    [Serializable]
    public class UserMention
    {
        public string screen_name;
        public string name;
        public long id;
        public string id_str;
        public int[] indices;
    }

    [Serializable]
    public class Entities
    {
        public object[] hashtags;
        public object[] symbols;
        public UserMention[] user_mentions;
        public object[] urls;
    }

    [Serializable]
    public class SearchMetadata
    {
        public double completed_in;
        public long max_id;
        public string max_id_str;
        public string query;
        public string refresh_url;
        public int count;
        public long since_id;
        public string since_id_str;
    }

    [Serializable]
    public class SearchResult
    {
        public Tweet[] statuses;
        public SearchMetadata search_metadata;
    }

    [Serializable]
    public class Tweet
    {
        public object metadata;
        public string created_at;
        public long id;
        public string id_str;
        public string text;
        public string source;
        public bool truncated;
        public long? in_reply_to_status_id;
        public string in_reply_to_status_id_str;
        public long? in_reply_to_user_id;
        public string in_reply_to_user_id_str;
        public string in_reply_to_screen_name;
        public object user;
        public object geo;
        public object coordinates;
        public object place;
        public object contributors;
        public bool is_quote_status;
        public int retweet_count;
        public int favorite_count;
        public Entities entities;
        public bool favorited;
        public bool retweeted;
        public string lang;
    }

    private readonly IOAuthUser _user;
    private readonly IWebClient _webClient;
    private readonly IJsonReader _jsonReader;

    public Twitter(IOAuthUser user, IWebClient webClient, IJsonReader jsonReader)
    {
        _user = user;
        _webClient = webClient;
        _jsonReader = jsonReader;
    }

    public void MakeRequest(string httpMethod, string baseUrl, Dictionary<string, string> requestParams, Action<Request> onFinished)
    {
        _user.SignRequest(
            httpMethod,
            baseUrl,
            requestParams,
            onFinished: authorizationHeader => {
                var encodedRequestParams = requestParams
                    .ToDictionary(
                        x => Uri.EscapeDataString(x.Key),
                        x => Uri.EscapeDataString(x.Value))
                    .Select(x => x.Key + "=" + x.Value)
                    .ToArray();

                var url = baseUrl
                    + "?"
                    + string.Join("&", encodedRequestParams);

                var headers = new Dictionary<string, string>() {
						{ "Accept", "*/*" },
						{ "Connection", "close" },
						{ "User-Agent", "OAuth gem v0.4.4" },
						{ "Content-Type", "application/x-www-form-urlencoded" },
						{ "Authorization", authorizationHeader }
					};

                onFinished(new Request(headers, url, rawData: null));
            }
        );
    }

    public void SearchTweets(string searchQuery, Action<SearchResult> onFinished)
    {
        MakeRequest(
            httpMethod: "GET",
            baseUrl: @"https://api.twitter.com/1.1/search/tweets.json",
            requestParams: new Dictionary<string, string>() {
					{ "q", searchQuery }
				},
            onFinished: request =>
                _webClient.RunRequest(request, onFinished: response =>
                    ParseJson<SearchResult>(response, onFinished)
                )
        );
    }

    private void ParseJson<T>(string json, Action<T> onFinished)
    {
        if (_jsonReader.SupportsGenericType)
            _jsonReader.Read<T>(json, onFinished);
        else
            _jsonReader.Read(json, typeof(T), onFinished: result => onFinished((T)result));
    }
}