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
using System.Web;
using System.Web.Configuration;
using System.Security.Cryptography.X509Certificates;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;

using Google.Apis.Walletobjects.v1;
using Google.Apis.Walletobjects.v1.Data;
using GooglePay.WalletObjects.utils;
using GooglePay.WalletObjects.verticals;

namespace WalletObjectsSample.Handlers
{
	public class WobInsertHandler : System.Web.IHttpHandler
	{
		public bool IsReusable {
			get {
				return true;
			}
	  }

    public virtual void ProcessRequest(HttpContext context)
    {
      try
      {
        HttpRequest request = context.Request;

        // get settings
        WobCredentials credentials = new WobCredentials(
          WebConfigurationManager.AppSettings["ServiceAccountId"],
          WebConfigurationManager.AppSettings["ServiceAccountPrivateKey"],
          WebConfigurationManager.AppSettings["ApplicationName"],
          WebConfigurationManager.AppSettings["IssuerId"]);

        string loyaltyClassId = WebConfigurationManager.AppSettings["LoyaltyClassId"];
        string offerClassId = WebConfigurationManager.AppSettings["OfferClassId"];

        // OAuth - setup certificate based on private key file
        X509Certificate2 certificate = new X509Certificate2(
          AppDomain.CurrentDomain.BaseDirectory + credentials.ServiceAccountPrivateKey,
          "notasecret",
          X509KeyStorageFlags.Exportable);

        // create service account credential
        ServiceAccountCredential credential = new ServiceAccountCredential(
        new ServiceAccountCredential.Initializer(credentials.ServiceAccountId)
        {
          Scopes = new[] { "https://www.googleapis.com/auth/wallet_object.issuer" }
        }.FromCertificate(certificate));

        // create the service
        var woService = new WalletobjectsService(new BaseClientService.Initializer()
        {
          HttpClientInitializer = credential,
          ApplicationName = "Wallet Objects API Sample",
        });

        // get the class type
        string type = request.Params["type"];

        // insert the class
        if (type.Equals("loyalty"))
        {
          LoyaltyClass loyaltyClass = Loyalty.GenerateLoyaltyClass(credentials.IssuerId, loyaltyClassId);
          woService.Loyaltyclass.Insert(loyaltyClass).Execute();
        }
        else if (type.Equals("offer"))
        {
          OfferClass offerClass = Offer.GenerateOfferClass(credentials.IssuerId, offerClassId);
          woService.Offerclass.Insert(offerClass).Execute();
        }
      }
      catch (Exception e)
      {
        Console.Write(e.StackTrace);
      }
    }
	}
}
