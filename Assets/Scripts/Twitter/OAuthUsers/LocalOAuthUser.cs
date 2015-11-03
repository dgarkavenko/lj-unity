using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public class LocalOAuthUser : IOAuthUser
{
    public LocalOAuthUser(string consumerKey, string consumerSecret, string token, string tokenSecret)
    {
        ConsumerKey = consumerKey;
        ConsumerSecret = consumerSecret;
        Token = token;
        TokenSecret = tokenSecret;

        var signingKey = Uri.EscapeDataString(consumerSecret)
            + "&"
            + Uri.EscapeDataString(tokenSecret);

        _hmac = new HMACSHA1(Encoding.UTF8.GetBytes(signingKey));
    }

    public readonly string ConsumerKey;
    public readonly string ConsumerSecret;
    public readonly string Token;
    public readonly string TokenSecret;

    private readonly HMACSHA1 _hmac;

    private const string SignatureMethod = "HMAC-SHA1";
    private const string Version = "1.0";

    public void SignRequest(string httpMethod, string baseUrl, Dictionary<string, string> requestParams, Action<string> onFinished)
    {
        var timestamp = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
        var nonce = Guid.NewGuid().ToString("N");

        var oauthParams = new Dictionary<string, string> {
			{ "oauth_consumer_key", ConsumerKey },
			{ "oauth_nonce", nonce },
			{ "oauth_signature_method", SignatureMethod },
			{ "oauth_timestamp", timestamp },
			{ "oauth_token", Token },
			{ "oauth_version", Version }
		};

        var sortedParams = oauthParams
            .Concat(requestParams)
            .ToDictionary(
                x => Uri.EscapeDataString(x.Key),
                x => Uri.EscapeDataString(x.Value))
            .OrderBy(x => x.Key)
            .Select(x => x.Key + "=" + x.Value)
            .ToArray();

        var parameterString = string.Join("&", sortedParams);

        var signatureBaseString = httpMethod
            + "&"
            + Uri.EscapeDataString(baseUrl)
            + "&"
            + Uri.EscapeDataString(parameterString);

        var signature = Uri.EscapeDataString(Convert.ToBase64String(_hmac.ComputeHash(Encoding.UTF8.GetBytes(signatureBaseString))));

        var authorizationDict = new Dictionary<string, string> {
			{ "oauth_consumer_key", ConsumerKey },
			{ "oauth_nonce", nonce },
			{ "oauth_signature", signature },
			{ "oauth_signature_method", SignatureMethod },
			{ "oauth_timestamp", timestamp },
			{ "oauth_token", Token },
			{ "oauth_version", Version }
		};

        var authorizationHeader = "OAuth "
            + string.Join(", ", authorizationDict.Select(x => x.Key + "=\"" + x.Value + "\"").ToArray());

        onFinished(authorizationHeader);
    }
}