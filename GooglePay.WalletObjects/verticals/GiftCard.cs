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

using System.Collections.Generic;
using Google.Apis.Walletobjects.v1.Data;

namespace GooglePay.WalletObjects.verticals
{
  public class GiftCard
  {
    /// <summary>
    /// Generates a GiftCard Object
    /// </summary>
    /// <param name="issuerId"> </param>
    /// <param name="classId"> </param>
    /// <param name="objectId"> </param>
    /// <returns> GiftCardObject </returns>
    public static GiftCardObject GenerateGiftCardObject(string issuerId, string classId, string objectId)
    {
      // Define Barcode
      Barcode barcode = new Barcode() {
        Type = "qrCode",
        Value = "28343E3",
        AlternateText = "12345"
      };

      Money balance = new Money {
        CurrencyCode = "USD",
        Micros = 20000000L
      };

      // TODO(samstern): balanceUpdateTime

      // Define Text Module Data
      IList<TextModuleData> textModulesData = new List<TextModuleData>();
      TextModuleData textModuleData = new TextModuleData() {
        Header = "Earn double points",
        Body = "Jane, don't forget to use your Baconrista Rewards when " +
            "paying with this gift card to earn additional points"
      };
      textModulesData.Add(textModuleData);

      // Define Links Module Data
      IList<Uri> uris = new List<Uri>();
      Uri uri1 = new Uri() {
        Description = "My Baconrista Gift Card Purchases",
        UriValue = "http://www.baconrista.com/mybalance?id=1234567890"
      };
      uris.Add(uri1);

      LinksModuleData linksModuleData = new LinksModuleData() {
        Uris = uris
      };

      // Define Wallet Instance
      GiftCardObject giftCardObj = new GiftCardObject() {
        ClassId = issuerId + "." + classId,
        Id = issuerId + "." + objectId,
        State = "active",
        Version = 1,
        Barcode = barcode,
        TextModulesData = textModulesData,
        LinksModuleData = linksModuleData,
        Balance = balance,
        EventNumber = "123456",
        CardNumber = "123jkl4889",
        Pin = "1111"
      };

      return giftCardObj;
    }

    /// <summary>
    /// Generates a GiftCard Class
    /// </summary>
    /// <param name="issuerId"> </param>
    /// <param name="classId"> </param>
    /// <returns> GiftCardClass </returns>
    public static GiftCardClass GenerateGiftCardClass(string issuerId, string classId)
    {
        // Define Text Module Data
        IList<TextModuleData> textModulesData = new List<TextModuleData>();
        TextModuleData textModuleData = new TextModuleData() {
          Header = "Where to Redeem",
          Body = "All US gift cards are redeemable in any US and Puerto Rico " +
                "Baconrista retail locations, or online at Baconrista.com where" +
                "available, for merchandise or services."
        };
        textModulesData.Add(textModuleData);

        // Define Links Module Data
        IList<Uri> uris = new List<Uri>();
        Uri uri1 = new Uri() {
          Description = "Baconrista",
          UriValue = "http://www.baconrista.com/"
        };
        uris.Add(uri1);

        LinksModuleData linksModuleData = new LinksModuleData() {
          Uris = uris
        };

        // Define Info Module
        InfoModuleData infoModuleData = new InfoModuleData() {
          ShowLastUpdateTime = true
        };

        // Define Geofence locations
        IList<LatLongPoint> locations = new List<LatLongPoint>();
        locations.Add(new LatLongPoint() { Latitude = 37.422601, Longitude = -122.085286 });
        locations.Add(new LatLongPoint() { Latitude = 37.424354, Longitude = -122.09508869999999 });
        locations.Add(new LatLongPoint() { Latitude = 40.7406578, Longitude = -74.00208940000002 });

        // Create GiftCard class
        GiftCardClass wobClass = new GiftCardClass() {
          Id = issuerId + "." + classId,
          IssuerName = "Baconrista",
          MerchantName = "Baconrista",
          ProgramLogo = new Image() {
            SourceUri = new Uri() {
              UriValue = "http://farm8.staticflickr.com/7340/11177041185_a61a7f2139_o.jpg"
            }
          },
          TextModulesData = textModulesData,
          LinksModuleData = linksModuleData,
          ReviewStatus = "underReview",
          Locations = locations
        };

        return wobClass;
    }
  }
}
