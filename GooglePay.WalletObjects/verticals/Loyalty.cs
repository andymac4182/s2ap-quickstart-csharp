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
  public class Loyalty
  {
    /// <summary>
    /// Generates a Loyalty Object
    /// </summary>
    /// <param name="issuerId"> </param>
    /// <param name="classId"> </param>
    /// <param name="objectId"> </param>
    /// <returns> loyaltyObject </returns>
    public static LoyaltyObject GenerateLoyaltyObject(string issuerId, string classId, string objectId)
    {
      // Define Barcode
      Barcode barcode = new Barcode() {
        Type = "qrCode",
        Value = "28343E3",
        AlternateText = "12345"
      };

      // Define Points
      LoyaltyPoints points = new LoyaltyPoints() {
        Label = "Points",
        PointsType = "points",
        Balance = new LoyaltyPointsBalance() { String = "500" }
      };

      // Define Text Module Data
      IList<TextModuleData> textModulesData = new List<TextModuleData>();
      TextModuleData textModuleData = new TextModuleData() {
        Header = "Jane's Baconrista Rewards",
        Body = "Save more at your local Mountain View store Jane.  " +
        "You get 1 bacon fat latte for every 5 coffees purchased.  " +
        "Also just for you, 10% off all pastries in the Mountain View store."
      };
      textModulesData.Add(textModuleData);

      // Define Links Module Data
      IList<Uri> uris = new List<Uri>();
      Uri uri1 = new Uri() {
        Description = "My Baconrista Account",
        UriValue = "http://www.baconrista.com/myaccount?id=1234567890"
      };
      uris.Add(uri1);

      LinksModuleData linksModuleData = new LinksModuleData() {
        Uris = uris
      };

      // Define Info Module
      IList<LabelValue> row0Cols = new List<LabelValue>();
      LabelValue row0Col0 = new LabelValue() { Label = "Next Reward in", Value = "2 coffees" };
      LabelValue row0Col1 = new LabelValue() { Label = "Member Since", Value = "01/15/2013" };
      row0Cols.Add(row0Col0);
      row0Cols.Add(row0Col1);

      IList<LabelValue> row1Cols = new List<LabelValue>();
      LabelValue row1Col0 = new LabelValue() { Label = "Local Store", Value = "Mountain View" };
      row1Cols.Add(row1Col0);

      IList<LabelValueRow> rows = new List<LabelValueRow>();
      LabelValueRow row0 = new LabelValueRow() {
        Columns = row0Cols
      };
      LabelValueRow row1 = new LabelValueRow() {
        Columns = row1Cols
      };

      rows.Add(row0);
      rows.Add(row1);

      InfoModuleData infoModuleData = new InfoModuleData() {
        ShowLastUpdateTime = true,
        LabelValueRows = rows
      };

      // Define general messages
      IList<WalletObjectMessage> messages = new List<WalletObjectMessage>();
      WalletObjectMessage message = new WalletObjectMessage() {
        Header = "Hi Jane!",
        Body = "Thanks for joining our program. Show this message to " +
                "our barista for your first free coffee on us!",
        ActionUri = new Uri() { UriValue = "http://baconrista.com" }
      };

      messages.Add(message);

      // Define Wallet Instance
      LoyaltyObject loyaltyObj = new LoyaltyObject() {
        ClassId = issuerId + "." + classId,
        Id = issuerId + "." + objectId,
        Version = 1,
        State = "active",
        Barcode = barcode,
        AccountName = "Jane Doe",
        AccountId = "1234567890",
        LoyaltyPoints = points,
        Messages = messages,
        InfoModuleData = infoModuleData,
        TextModulesData = textModulesData,
        LinksModuleData = linksModuleData
      };

      return loyaltyObj;
    }

    /// <summary>
    /// Generates a Loyalty Class
    /// </summary>
    /// <param name="issuerId"> </param>
    /// <param name="classId"> </param>
    /// <returns> loyaltyClass </returns>
    public static LoyaltyClass GenerateLoyaltyClass(string issuerId, string classId)
    {
        // Define the Image Module Data
        IList<ImageModuleData> imageModulesData = new List<ImageModuleData>();
        ImageModuleData image = new ImageModuleData() {
          MainImage = new Image() {
            SourceUri = new Uri() {
              UriValue = "http://farm4.staticflickr.com/3738/12440799783_3dc3c20606_b.jpg",
              Description = "Coffee beans"
            }
          }
        };
        imageModulesData.Add(image);

        // Define Text Module Data
        IList<TextModuleData> textModulesData = new List<TextModuleData>();
        TextModuleData textModuleData = new TextModuleData() {
          Header = "Rewards details",
          Body = "Welcome to Baconrista rewards.  Enjoy your rewards for being a loyal customer.  " +
          "10 points for ever dollar spent.  Redeem your points for free coffee, bacon and more!"
        };
        textModulesData.Add(textModuleData);

        // Define Links Module Data
        IList<Uri> uris = new List<Uri>();
        Uri uri1 = new Uri() {
          Description = "Nearby Locations",
          UriValue = "http://maps.google.com/maps?q=google"
        };
        Uri uri2 = new Uri() {
          Description = "Call Customer Service",
          UriValue = "tel:6505555555"
        };
        uris.Add(uri1);
        uris.Add(uri2);

        LinksModuleData linksModuleData = new LinksModuleData() {
          Uris = uris
        };

        // Define Info Module
        InfoModuleData infoModuleData = new InfoModuleData() {
          HexFontColor = "#F8EDC1",
          HexBackgroundColor = "#442905",
          ShowLastUpdateTime = true
        };

        // Define general messages
        IList<WalletObjectMessage> messages = new List<WalletObjectMessage>();
        WalletObjectMessage message = new WalletObjectMessage() {
          Header = "Welcome to Banconrista Rewards!",
          Body = "Featuring our new bacon donuts.",
          Image = new Image() {
            SourceUri = new Uri() {
              UriValue = "http://farm8.staticflickr.com/7302/11177240353_115daa5729_o.jpg"
            }
          },
          ActionUri = new Uri() { UriValue = "http://baconrista.com" }
        };
        messages.Add(message);

        // Define Geofence locations
        IList<LatLongPoint> locations = new List<LatLongPoint>();
        locations.Add(new LatLongPoint() { Latitude = 37.422601, Longitude = -122.085286 });
        locations.Add(new LatLongPoint() { Latitude = 37.424354, Longitude = -122.09508869999999 });
        locations.Add(new LatLongPoint() { Latitude = 40.7406578, Longitude = -74.00208940000002 });

        // Create Loyalty class
        LoyaltyClass wobClass = new LoyaltyClass() {
          Id = issuerId + "." + classId,
          IssuerName = "Baconrista",
          ProgramName = "Baconrista Rewards",
          ProgramLogo = new Image() {
            SourceUri = new Uri() {
              UriValue = "http://farm8.staticflickr.com/7340/11177041185_a61a7f2139_o.jpg"
            }
          },
          RewardsTierLabel = "Tier",
          RewardsTier = "Gold",
          AccountNameLabel = "Member Name",
          AccountIdLabel = "Member Id",
          Messages = messages,
          ReviewStatus = "underReview",
          AllowMultipleUsersPerObject = true,
          Locations = locations,
          ImageModulesData = imageModulesData,
          InfoModuleData = infoModuleData,
          TextModulesData = textModulesData,
          LinksModuleData = linksModuleData
        };

        return wobClass;
    }
  }
}
