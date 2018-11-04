/*
Copyright 2013 Google Inc

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Google.Apis.Auth;
using Google.Apis.Json;
using Google.Apis.Walletobjects.v1.Data;

namespace GooglePay.WalletObjects.utils
{
  public class WobUtils
  {
      readonly string _issuer;
      readonly string[] _origins;
      readonly IList<LoyaltyObject> _loyaltyObjects = new List<LoyaltyObject>();
      readonly IList<OfferObject> _offerObjects = new List<OfferObject>();
      readonly IList<GiftCardObject> _giftCardObjects = new List<GiftCardObject>();
      readonly RSA _key;

      public WobUtils(string iss, X509Certificate2 cert, string[] origins)
      {
          _issuer = iss;
          this._origins = origins;
          _key = cert.GetRSAPrivateKey();
      }

      public void AddObject(LoyaltyObject obj)
    {
        _loyaltyObjects.Add(obj);
    }

    public void AddObject(OfferObject obj)
    {
        _offerObjects.Add(obj);
    }
        
    public void AddObject(GiftCardObject obj)
    {
        _giftCardObjects.Add(obj);
    }        

    private string CreateSerializedHeader()
    {
      var header = new GoogleJsonWebSignature.Header()
      {
        Algorithm = "RS256",
        Type = "JWT"
      };

      return NewtonsoftJsonSerializer.Instance.Serialize(header);
    }

    private string CreateSerializedPayload()
    {
      var iat = (int)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
      var jwtContainer = new JsonWebToken.Payload()
      {
        Issuer = _issuer,
        Audience = "google",
        Type = "savetoandroidpay",
        IssuedAtTimeSeconds = iat,
        Objects = new JsonWebToken.Payload.Content()
        {
          LoyaltyObjects = _loyaltyObjects,
          OfferObjects = _offerObjects,
          GiftCardObjects = _giftCardObjects
        },
        Origins = _origins
        //Origins  = new []{"http://localhost:59113"}
      };

      return NewtonsoftJsonSerializer.Instance.Serialize(jwtContainer);
    }

    private string CreateWsPayload(JsonWebToken.Payload.WebserviceResponse response)
    {
      var iat = (int)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
      var jwtContainer = new JsonWebToken.Payload()
      {
        Issuer = _issuer,
        Audience = "google",
        Type = "loyaltywebservice",
        IssuedAtTimeSeconds = iat,
        Objects = new JsonWebToken.Payload.Content()
        {
          LoyaltyObjects = _loyaltyObjects,
          OfferObjects = _offerObjects,
          WebserviceResponse = response
        },
      };

      return NewtonsoftJsonSerializer.Instance.Serialize(jwtContainer);
    }

    public string GenerateJwt()
    {
        string header = UrlSafeBase64Encode(CreateSerializedHeader());
        string body = UrlSafeBase64Encode(CreateSerializedPayload());
        string content = header + "." + body;
        string signature = CreateSignature(content);
        return content + "." + signature;
    }

    public string GenerateWsJwt(JsonWebToken.Payload.WebserviceResponse response)
    {
        string header = UrlSafeBase64Encode(CreateSerializedHeader());
        string body = UrlSafeBase64Encode(CreateWsPayload(response));
        string content = header + "." + body;
        string signature = CreateSignature(content);
        return content + "." + signature;
    }
    private string CreateSignature(string content)
    {
        return UrlSafeBase64Encode(_key.SignData(Encoding.UTF8.GetBytes(content), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
    }

    /// <summary>Encodes the provided UTF8 string into an URL safe base64 string.</summary>
    /// <param name="value">Value to encode</param>
    /// <returns>The URL safe base64 string</returns>
    private string UrlSafeBase64Encode(string value)
    {
        return UrlSafeBase64Encode(Encoding.UTF8.GetBytes(value));
    }

    /// <summary>Encodes the byte array into an URL safe base64 string.</summary>
    /// <param name="bytes">Byte array to encode</param>
    /// <returns>The URL safe base64 string</returns>
    private string UrlSafeBase64Encode(byte[] bytes)
    {
        return Convert.ToBase64String(bytes).Replace("=", string.Empty).Replace('+', '-').Replace('/', '_');
    }
  }
}
