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

namespace GooglePay.WalletObjects.utils
{
	public class WobCredentials
	{
        public WobCredentials()
        {
            
        }
	  public WobCredentials(string serviceAccountId, string serviceAccountPrivateKey, string applicationName, string issuerId)
	  {
		  ServiceAccountId = serviceAccountId;
		  ServiceAccountPrivateKey = serviceAccountPrivateKey;
		  ApplicationName = applicationName;
		  IssuerId = issuerId;
	  }

	  /// <returns> the serviceAccountId </returns>
	  public string ServiceAccountId { get; set; }
	  /// <returns> the serviceAccountPrivateKey </returns>
	  public string ServiceAccountPrivateKey { get; set; }
        /// <returns> the applicationName </returns>
      public string ApplicationName { get; set; }
        /// <returns> the issuerId </returns>
      public string IssuerId { get; set; }

        public string Origins { get; set; }
    }
}