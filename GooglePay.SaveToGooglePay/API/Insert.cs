using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Walletobjects.v1;
using GooglePay.WalletObjects.utils;
using GooglePay.WalletObjects.verticals;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace GooglePay.SaveToGooglePay.API
{
    [Route("/insert")]
    [Produces("application/json")]
    [ApiController]
    public class Insert : ControllerBase
    {
        private readonly IOptions<WobCredentials> _wobCredentials;
        private readonly IOptions<PayIds> _payIds;
        private readonly IHostingEnvironment _hostingEnvironment;

        public Insert(IOptions<WobCredentials> wobCredentials, IOptions<PayIds> payIds, IHostingEnvironment hostingEnvironment)
        {
            _wobCredentials = wobCredentials;
            _payIds = payIds;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public IActionResult HandleGet([FromQuery]string type)
        {
            var certificate = new X509Certificate2(
                Path.Combine(_hostingEnvironment.ContentRootPath, _wobCredentials.Value.ServiceAccountPrivateKey),
                "notasecret",
                X509KeyStorageFlags.Exportable);

            // create service account credential
            var credential = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(_wobCredentials.Value.ServiceAccountId)
                {
                    Scopes = new[] { "https://www.googleapis.com/auth/wallet_object.issuer" }
                }.FromCertificate(certificate));

            // create the service
            var woService = new WalletobjectsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _wobCredentials.Value.ApplicationName,
            });

            switch (type)
            {
                case "loyalty":
                    var loyaltyClass = Loyalty.GenerateLoyaltyClass(_wobCredentials.Value.IssuerId, _payIds.Value.LoyaltyClassId);
                    woService.Loyaltyclass.Insert(loyaltyClass).Execute();
                    break;
                case "offer":
                    var offerClass = Offer.GenerateOfferClass(_wobCredentials.Value.IssuerId, _payIds.Value.OfferClassId);
                    woService.Offerclass.Insert(offerClass).Execute();
                    break;
                case "giftcard":
                    var giftCard = GiftCard.GenerateGiftCardClass(_wobCredentials.Value.IssuerId, _payIds.Value.GiftCardClassId);
                    woService.Giftcardclass.Insert(giftCard).Execute();
                    break;
            }

            return Ok();
        }
    }
}
