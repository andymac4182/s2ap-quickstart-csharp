using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Google.Apis.Walletobjects.v1.Data;
using GooglePay.WalletObjects.utils;
using GooglePay.WalletObjects.verticals;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GooglePay.SaveToGooglePay.API
{
    [Route("/jwt")]
    [Produces("application/json")]
    [ApiController]
    public class Jwt : ControllerBase
    {
        private readonly IOptions<WobCredentials> _wobCredentials;
        private readonly IOptions<PayIds> _payIds;
        private readonly IHostingEnvironment _hostingEnvironment;

        public Jwt(IOptions<WobCredentials> wobCredentials, IOptions<PayIds> payIds, IHostingEnvironment hostingEnvironment)
        {
            _wobCredentials = wobCredentials;
            _payIds = payIds;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult HandleGet([FromQuery] string type)
        {
            // OAuth - setup certificate based on private key file
            var certificate = new X509Certificate2(
                Path.Combine(_hostingEnvironment.ContentRootPath, _wobCredentials.Value.ServiceAccountPrivateKey),
                "notasecret",
                X509KeyStorageFlags.Exportable);

            var utils = new WobUtils(_wobCredentials.Value.ServiceAccountId, certificate, _wobCredentials.Value.Origins.Split(' '));

            // get the object type
            if (type.Equals("loyalty"))
            {
                var loyaltyObject = Loyalty.GenerateLoyaltyObject(_wobCredentials.Value.IssuerId, _payIds.Value.LoyaltyClassId, _payIds.Value.LoyaltyObjectId);
                utils.AddObject(loyaltyObject);
            }
            else if (type.Equals("offer"))
            {
                var offerObject = Offer.GenerateOfferObject(_wobCredentials.Value.IssuerId, _payIds.Value.OfferClassId, _payIds.Value.OfferObjectId);
                utils.AddObject(offerObject);
            }
            else if (type.Equals("giftcard"))
            {
                var giftCardObject = GiftCard.GenerateGiftCardObject(_wobCredentials.Value.IssuerId, _payIds.Value.GiftCardClassId, _payIds.Value.GiftCardObjectId);
                utils.AddObject(giftCardObject);
            }

            // generate the JWT
            var jwt = utils.GenerateJwt();

            return Ok(jwt);
        }
    }
}
