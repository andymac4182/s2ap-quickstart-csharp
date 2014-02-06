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
using Google.Apis.Walletobjects.v1;
using Google.Apis.Walletobjects.v1.Data;

using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters;

namespace WalletObjectsSample.Verticals
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
        public static LoyaltyObject generateLoyaltyObject(string issuerId, string classId, string objectId)
        {
          // Define Barcode
          Barcode barcode = new Barcode() { 
            Type = "qrCode",
            Value = "28343E3" 
          };

          // Define Points
          LoyaltyPoints points = new LoyaltyPoints() {
            Label = "Points",
            Balance = new LoyaltyPointsBalance() { String = "500" }
          };

          // Define Text Module Data
          IList<TextModuleData> textModulesData = new List<TextModuleData>();

          TextModuleData textModuleData = new TextModuleData() {
            Header = "Jane's Baconrista Rewards",
            Body = "You are 5 coffees away from receiving a free bacon fat latte"
          };

          textModulesData.Add(textModuleData);          
          
          // Define Uris
          IList<Uri> uris = new List<Uri>();
          Uri uri1 = new Uri() {
            Description = "My Baconrista Account",
            UriValue = "http://www.baconrista.com/myaccount?id=1234567890"
          };
          Uri uri2 = new Uri() {
            Description = "uri 2 description",
            UriValue = "http://www.google.com"
          };

          uris.Add(uri1);
          uris.Add(uri2);

          LinksModuleData linksModuleData = new LinksModuleData() {
            Uris = uris
          };
          
          // Define Info Module
          IList<LabelValue> row0cols = new List<LabelValue>();
          LabelValue row0col0 = new LabelValue() { Label = "Member Name", Value = "Jane Doe" };
          LabelValue row0col1 = new LabelValue() { Label = "Membership #", Value = "1234567890" };          
          row0cols.Add(row0col0);
          row0cols.Add(row0col1);

          IList<LabelValue> row1cols = new List<LabelValue>();
          LabelValue row1col0 = new LabelValue() { Label = "Next Reward in", Value = "2 coffees" };
          LabelValue row1col1 = new LabelValue() { Label = "Member Since", Value = "01/15/2013" };          
          row1cols.Add(row1col0);
          row1cols.Add(row1col1);

          IList<LabelValueRow> rows = new List<LabelValueRow>();
          LabelValueRow row0 = new LabelValueRow() { HexBackgroundColor = "#BBCCFC", Columns = row0cols };
          LabelValueRow row1 = new LabelValueRow() { HexBackgroundColor = "#FFFB00", Columns = row1cols };

          rows.Add(row0);
          rows.Add(row1);

          InfoModuleData infoModuleData = new InfoModuleData() {
            HexFontColor = "#FFFFFF",
            HexBackgroundColor = "#FC058C",
            ShowLastUpdateTime = true,
            LabelValueRows = rows 
          };

          // Define Wallet Instance
          LoyaltyObject loyaltyObj = new LoyaltyObject()
          {
            ClassId = issuerId + "." + classId,
            Id = issuerId + "." + objectId,
            Version = "1",
            State = "active",
            Barcode = barcode,
            AccountName = "Jane Doe",
            AccountId = "1234567890",
            LoyaltyPoints = points,
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
        public static LoyaltyClass generateLoyaltyClass(string issuerId, string classId)
        {
            // Define general messages
            IList<WalletObjectMessage> messages = new List<WalletObjectMessage>();
            WalletObjectMessage message = new WalletObjectMessage();
            message.Header = "Welcome to Banconrista Rewards!";
            message.Body = "Featuring our new bacon donuts.";

            Uri imageUri = new Uri();
            imageUri.UriValue = "http://farm8.staticflickr.com/7302/11177240353_115daa5729_o.jpg";
            Image messageImage = new Image();
            messageImage.SourceUri = imageUri;
            message.Image = messageImage;

            Uri actionUri = new Uri();
            actionUri.UriValue = "http://baconrista.com";
            message.ActionUri = actionUri;

            messages.Add(message);

            // Define rendering templates per view
            IList<RenderSpec> renderSpec = new List<RenderSpec>();

            RenderSpec listRenderSpec = new RenderSpec();
            listRenderSpec.ViewName = "g_list";
            listRenderSpec.TemplateFamily = "1.loyaltyCard1_list";

            RenderSpec expandedRenderSpec = new RenderSpec();
            expandedRenderSpec.ViewName = "g_expanded";
            expandedRenderSpec.TemplateFamily = "1.loyaltyCard1_expanded";

            renderSpec.Add(listRenderSpec);
            renderSpec.Add(expandedRenderSpec);

            // Define Geofence locations
            IList<LatLongPoint> locations = new List<LatLongPoint>();

            LatLongPoint llp1 = new LatLongPoint();
            llp1.Latitude = 37.422601;
            llp1.Longitude = -122.085286;

            LatLongPoint llp2 = new LatLongPoint();
            llp2.Latitude = 37.429379;
            llp2.Longitude = -122.122730;

            locations.Add(llp1);
            locations.Add(llp2);

            // Create class
            LoyaltyClass wobClass = new LoyaltyClass();
            wobClass.Id = issuerId + "." + classId;
            wobClass.Version = "1";
            wobClass.IssuerName = "Baconrista";
            wobClass.ProgramName = "Baconrista Rewards";

            Uri homepageUri = new Uri();
            homepageUri.UriValue = "https://www.example.com"; 
            homepageUri.Description = "Website";
            wobClass.HomepageUri = homepageUri;

            Uri logoImageUri = new Uri();
            logoImageUri.UriValue = "http://farm8.staticflickr.com/7340/11177041185_a61a7f2139_o.jpg"; 
            Image logoImage = new Image();
            logoImage.SourceUri = logoImageUri;
            wobClass.ProgramLogo = logoImage;
                
            wobClass.RewardsTierLabel= "Tier";
            wobClass.RewardsTier = "Gold";
            wobClass.AccountNameLabel = "Member Name";
            wobClass.AccountIdLabel = "Member Id";
            wobClass.RenderSpecs = renderSpec;
            wobClass.Messages = messages;
            wobClass.ReviewStatus = "underReview";
            wobClass.AllowMultipleUsersPerObject = true;
            wobClass.Locations = locations;

            return wobClass;
        }
    }

}