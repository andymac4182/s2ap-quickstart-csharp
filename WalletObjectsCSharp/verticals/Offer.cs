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

namespace WalletObjectsSample.Verticals
{
  public class Offer
  {
    public static OfferObject generateOfferObject(string issuerId, string classId, string objectId)
    {
      Barcode barcode = new Barcode() {
        Type = "upcA",
        Value = "123456789012",
        AlternateText = "12345",
        Label = "Offer Code"
      };

      // Define Wallet Object
      OfferObject offerObj = new OfferObject() {
        ClassId = issuerId + "." + classId,
        Id = issuerId + "." + objectId,
        Version = 1,
        Barcode = barcode,
        State = "active"
      };

      return offerObj;
    }

    public static OfferClass generateOfferClass(string issuerId, string classId)
    {
      // Define rendering templates per view
      IList<RenderSpec> renderSpec = new List<RenderSpec>();

      RenderSpec listRenderSpec = new RenderSpec() {
        ViewName = "g_list",
        TemplateFamily = "1.offer_list"
      };

      RenderSpec expandedRenderSpec = new RenderSpec() {
        ViewName = "g_expanded",
        TemplateFamily = "1.offer_expanded"
      };

      renderSpec.Add(listRenderSpec);
      renderSpec.Add(expandedRenderSpec);

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

      // Define Text Module Data
      IList<TextModuleData> textModulesData = new List<TextModuleData>();
      TextModuleData details = new TextModuleData() {
        Header = "Details",
        Body = "20% off one cup of coffee at all Baconrista Coffee locations.  " +
        "Only one can be used per visit."
      };
      TextModuleData finePrint = new TextModuleData() {
        Header = "About Baconrista",
        Body = "Since 2013, Baconrista Coffee has been committed to making high quality bacon coffee.  " +
        "Visit us in our stores or online at www.baconrista.com."
      };
      textModulesData.Add(details);
      textModulesData.Add(finePrint);

      // Define Geofence locations
      IList<LatLongPoint> locations = new List<LatLongPoint>();
      locations.Add(new LatLongPoint() { Latitude = 37.422601, Longitude = -122.085286 });
      locations.Add(new LatLongPoint() { Latitude = 37.424354, Longitude = -122.09508869999999 });
      locations.Add(new LatLongPoint() { Latitude = 40.7406578, Longitude = -74.00208940000002 });

      // Create Offer class
      OfferClass wobClass = new OfferClass() {
        Id = issuerId + "." + classId,
        IssuerName = "Baconrista Coffee",
        Title = "20% off one cup of coffee",
        Provider = "Baconrista Deals",
        TitleImage = new Image() {
          SourceUri = new Uri() {
            UriValue = "http://farm4.staticflickr.com/3723/11177041115_6e6a3b6f49_o.jpg"
          }
        },
        RenderSpecs = renderSpec,
        RedemptionChannel = "both",
        ReviewStatus = "underReview",
        Locations = locations,
        AllowMultipleUsersPerObject = true,
        ImageModulesData = imageModulesData,
        TextModulesData = textModulesData,
        LinksModuleData = linksModuleData
      };

      return wobClass;
    }
  }
}
