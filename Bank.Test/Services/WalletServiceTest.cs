using Bank.Client.Services;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Test.Services
{
    public class WalletServiceTest
    {
       private WalletService walletService;
        private HttpClient fakeHttpClient;
        public WalletServiceTest()
        {
            fakeHttpClient = A.Fake<HttpClient>();
            walletService = new WalletService(fakeHttpClient);
        }


    }
}
