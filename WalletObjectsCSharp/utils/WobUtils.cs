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
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Text;

using Google.Apis.Json;
using Google.Apis.Util;
using Google.Apis.Http;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Walletobjects.v1.Data;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace WalletObjectsSample.Utils
{
  public class WobUtils
  {
    String issuer;
    String [] origins;
    IList<LoyaltyObject> loyaltyObjects = new List<LoyaltyObject>();
    IList<OfferObject> offerObjects = new List<OfferObject>();
    RSACryptoServiceProvider key;

    public WobUtils(String iss, X509Certificate2 cert)
    {
      issuer = iss;
      origins = WebConfigurationManager.AppSettings["Origins"].Split(' ');
      RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PrivateKey;
      byte[] privateKeyBlob = rsa.ExportCspBlob(true);
      key = new RSACryptoServiceProvider();
      key.ImportCspBlob(privateKeyBlob);
    }

    public void addObject(LoyaltyObject obj)
    {
        loyaltyObjects.Add(obj);
    }

    public void addObject(OfferObject obj)
    {
        offerObjects.Add(obj);
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
        Issuer = issuer,
        Audience = "google",
        Type = "savetowallet",
        IssuedAtTimeSeconds = iat,
        Objects = new JsonWebToken.Payload.Content()
        {
          loyaltyObjects = loyaltyObjects,
          offerObjects = offerObjects
        },
        Origins = origins
        //Origins  = new []{"http://localhost:59113"}
      };

      return NewtonsoftJsonSerializer.Instance.Serialize(jwtContainer);
    }

    private string CreateWSPayload(JsonWebToken.Payload.WebserviceResponse response)
    {
      var iat = (int)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
      var jwtContainer = new JsonWebToken.Payload()
      {
        Issuer = issuer,
        Audience = "google",
        Type = "loyaltywebservice",
        IssuedAtTimeSeconds = iat,
        Objects = new JsonWebToken.Payload.Content()
        {
          loyaltyObjects = loyaltyObjects,
          offerObjects = offerObjects,
          webserviceResponse = response
        },
      };

      return NewtonsoftJsonSerializer.Instance.Serialize(jwtContainer);
    }

    public String GenerateJwt()
    {
        String header = UrlSafeBase64Encode(CreateSerializedHeader());
        String body = UrlSafeBase64Encode(CreateSerializedPayload());
        String content = header + "." + body;
        String signature = CreateSignature(content);
        return content + "." + signature;
    }

    public String GenerateWsJwt(JsonWebToken.Payload.WebserviceResponse response)
    {
        String header = UrlSafeBase64Encode(CreateSerializedHeader());
        String body = UrlSafeBase64Encode(CreateWSPayload(response));
        String content = header + "." + body;
        String signature = CreateSignature(content);
        return content + "." + signature;
    }
    private String CreateSignature(String content)
    {
        return UrlSafeBase64Encode(key.SignData(Encoding.UTF8.GetBytes(content), "SHA256"));
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
        return Convert.ToBase64String(bytes).Replace("=", String.Empty).Replace('+', '-').Replace('/', '_');
    }
  }
}