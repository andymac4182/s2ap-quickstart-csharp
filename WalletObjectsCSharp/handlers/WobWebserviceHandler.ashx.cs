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
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Security.Cryptography.X509Certificates;

using Google.Apis.Json;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;

using Google.Apis.Walletobjects.v1;
using Google.Apis.Walletobjects.v1.Data;
using GooglePay.WalletObjects.utils;
using GooglePay.WalletObjects.verticals;
using WalletObjectsSample.Webservice;

namespace WalletObjectsSample.Handlers
{
  public class WobWebserviceHandler : System.Web.IHttpHandler
  {
    public bool IsReusable
    {
      get { return true; }
    }

    public virtual void ProcessRequest(HttpContext context)
    {
      try {
        HttpRequest request = context.Request;

        WobCredentials credentials = new WobCredentials(
        WebConfigurationManager.AppSettings["ServiceAccountId"],
        WebConfigurationManager.AppSettings["ServiceAccountPrivateKey"],
        WebConfigurationManager.AppSettings["ApplicationName"],
        WebConfigurationManager.AppSettings["IssuerId"]);

        // OAuth - setup certificate based on private key file
        X509Certificate2 certificate = new X509Certificate2(
          AppDomain.CurrentDomain.BaseDirectory + credentials.ServiceAccountPrivateKey,
          "notasecret",
          X509KeyStorageFlags.Exportable);

        WobUtils utils = null;
        WebserviceRequest webRequest = null;
        JsonWebToken.Payload.WebserviceResponse webResponse = null;
        string jwt = null;

        ReadEntityBodyMode read = request.ReadEntityBodyMode;
        Stream inputStream = null;

        if (read == ReadEntityBodyMode.None)
          inputStream = request.GetBufferedInputStream();
        else
          inputStream = request.InputStream;

        webRequest = NewtonsoftJsonSerializer.Instance.Deserialize<WebserviceRequest>(inputStream);

        if (webRequest.Method.Equals("signup")) {
          webResponse = new JsonWebToken.Payload.WebserviceResponse() {
            Message = "Welcome to baconrista",
            Result = "approved"
          };
        }
        else {
          webResponse = new JsonWebToken.Payload.WebserviceResponse() {
            Message = "Thanks for linking to baconrista",
            Result = "approved"
          };
        }

        utils = new WobUtils(credentials.IssuerId, certificate, WebConfigurationManager.AppSettings["Origins"].Split(' '));

        string linkId = webRequest.Params.LinkingId;
        LoyaltyObject loyaltyObject = Loyalty.GenerateLoyaltyObject(credentials.IssuerId, "LoyaltyClass", (linkId != null) ? linkId : "LoyaltyObject");
        utils.AddObject(loyaltyObject);

        jwt = utils.GenerateWsJwt(webResponse);

        HttpResponse response = context.Response;             
        response.Write(jwt);
      }
      catch (Exception e) {
        Console.Write(e.StackTrace);
      }
    }
  }
}

