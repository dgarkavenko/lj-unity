using UnityEngine;

public class TwitterTest : MonoBehaviour
{
    [SerializeField]
    private WwwWebClient _wwwWebClient;
    private readonly JsonFxJsonReader _jsonFxJsonReader = new JsonFxJsonReader();

    public Twitter.SearchResult searchResult;

    private void Start()
    {
        var user = new LocalOAuthUser(
            consumerKey: "Zca7e4rF28G1dFMDpz6vKYN8K",
            consumerSecret: "0WpzWwBwgk3nK4fMxhiyMVQlIjTHee5mpgh0PbWs1OPfxIyvxE",
            token: "4068030196-8G67m4gQGIXwUzAuo3Dvq640nZhFLunhnPcbSoQ",
            tokenSecret: "AcAaqeU8lHl4BT9yzl8DFm73hM0Yv7LGCrFMoc3dkBXUD"
        );

        var twitter = new Twitter(user, _wwwWebClient, _jsonFxJsonReader);

        twitter.SearchTweets("to:lj_bluebird", onFinished: result => {
            searchResult = result;

            foreach (var status in result.statuses)
                Debug.Log(status.text);
        });
    }
}
