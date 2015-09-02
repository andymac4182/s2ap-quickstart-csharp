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
using System.Web;
using System.Web.Configuration;
using System.Security.Cryptography.X509Certificates;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;

using Google.Apis.Walletobjects.v1;
using Google.Apis.Walletobjects.v1.Data;
using WalletObjectsSample.Utils;
using WalletObjectsSample.Verticals;

namespace WalletObjectsSample.Handlers
{
  public class WobGenerateJwtHandler : System.Web.IHttpHandler
  {
    public bool IsReusable
    {
      get { return true; }
    }

    public virtual void ProcessRequest(HttpContext context)
    {
      try {
        HttpRequest request = context.Request;

        // get credentials and settings
        WobCredentials credentials = new WobCredentials(
        WebConfigurationManager.AppSettings["ServiceAccountId"],
        WebConfigurationManager.AppSettings["ServiceAccountPrivateKey"],
        WebConfigurationManager.AppSettings["ApplicationName"],
        WebConfigurationManager.AppSettings["IssuerId"]);

        string loyaltyClassId = WebConfigurationManager.AppSettings["LoyaltyClassId"];
        string loyaltyObjectId = WebConfigurationManager.AppSettings["LoyaltyObjectId"];
        string offerClassId = WebConfigurationManager.AppSettings["OfferClassId"];
        string offerObjectId = WebConfigurationManager.AppSettings["OfferObjectId"];
        string giftCardClassId = WebConfigurationManager.AppSettings["GiftCardClassId"];
        string giftCardObjectId = WebConfigurationManager.AppSettings["GiftCardObjectId"];

        // OAuth - setup certificate based on private key file
        X509Certificate2 certificate = new X509Certificate2(
          AppDomain.CurrentDomain.BaseDirectory + credentials.serviceAccountPrivateKey,
          "notasecret",
          X509KeyStorageFlags.Exportable);

        WobUtils utils = new WobUtils(credentials.serviceAccountId, certificate);

        // get the object type
        string type = request.Params["type"];

        if (type.Equals("loyalty")) {
          LoyaltyObject loyaltyObject = Loyalty.generateLoyaltyObject(credentials.IssuerId, loyaltyClassId, loyaltyObjectId);
          utils.addObject(loyaltyObject);
        }
        else if (type.Equals("offer")) {
          OfferObject offerObject = Offer.generateOfferObject(credentials.IssuerId, offerClassId, offerObjectId);
          utils.addObject(offerObject);
        } 
        else if (type.Equals("giftcard")) {
          GiftCardObject giftCardObject = GiftCard.generateGiftCardObject(credentials.IssuerId, giftCardClassId, giftCardObjectId);
          utils.addObject(giftCardObject);
        }

        // generate the JWT
        string jwt = utils.GenerateJwt();

        HttpResponse response = context.Response;             
        response.Write(jwt);
        //response.ContentType = "text/xml";
      }
      catch (Exception e) {
        Console.Write(e.StackTrace);
      }
    }
  }
}
