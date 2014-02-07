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
        Label = "User Id"
      };

      // Define Wallet Object
      OfferObject offerObj = new OfferObject() {
        ClassId = issuerId + "." + classId,
        Id = issuerId + "." + objectId,
        Version = "1",
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
        TemplateFamily = "1.offer1_list"
      };

      RenderSpec expandedRenderSpec = new RenderSpec() {
        ViewName = "g_expanded",
        TemplateFamily = "1.offer1_expanded"
      };

      renderSpec.Add(listRenderSpec);
      renderSpec.Add(expandedRenderSpec);

      // Define Geofence locations
      IList<LatLongPoint> locations = new List<LatLongPoint>();
      locations.Add(new LatLongPoint() { Latitude = 37.442087, Longitude = -122.161446 });
      locations.Add(new LatLongPoint() { Latitude = 37.429379, Longitude = -122.122730 });
      locations.Add(new LatLongPoint() { Latitude = 37.333646, Longitude = -121.884853 });

      // Create Offer class
      OfferClass wobClass = new OfferClass() {
        Id = issuerId + "." + classId,
        Version = "1",
        IssuerName = "Baconrista Coffee",
        Title = "20% off one cup of coffee",
        Provider = "Baconrista Deals",
        Details = "20% off one cup of coffee at all Baconristas",
        TitleImage = new Image() {
          SourceUri = new Uri() {
            UriValue = "http://3.bp.blogspot.com/-AvC1agljv9Y/TirbDXOBIPI/AAAAAAAACK0/hR2gs5h2H6A/s1600/Bacon%2BWallpaper.png"
          }
        },
        RenderSpecs = renderSpec,
        RedemptionChannel = "both",
        ReviewStatus = "underReview",
        Locations = locations,
        AllowMultipleUsersPerObject = true
      };
    
      return wobClass;
    }
  }
}