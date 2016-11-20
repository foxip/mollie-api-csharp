using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mollie.Api;
using System.Configuration;
using Mollie.Api.Models;

namespace Mollie.Tests
{
    [TestClass]
    public class UnitTests
    {
        private static MollieClient mollieClient;

        [ClassInitialize]
        public static void InitClass(TestContext conext)
        {
            mollieClient = new MollieClient();
            mollieClient.setApiKey(ConfigurationManager.AppSettings["mollie_api_key"]);            
        }

        [TestMethod]
        public void CanMakePayment()
        {
            Payment payment = new Payment
            {                
                amount = 99.99M,
                description = "Test payment",
                redirectUrl = "http://www.foxip.net/test/12345",
                metadata = "12345678"            };

            PaymentStatus status = mollieClient.StartPayment(payment);

            Assert.AreEqual(status.amount, payment.amount);
            Assert.AreEqual(status.description, payment.description);
            Assert.AreEqual(status.links.redirectUrl, payment.redirectUrl);
            Assert.IsTrue(status.links.paymentUrl.StartsWith("https://"));
            Assert.AreEqual(status.mode, "test");
            Assert.AreEqual(status.paidDatetime, null);
            Assert.IsNull(status.cancelledDatetime, null);
            Assert.IsNotNull(status.createdDatetime);
            Assert.IsNull(status.details);
            Assert.IsNull(status.expiredDatetime);
            Assert.IsTrue(status.id!="");
            Assert.AreEqual(status.metadata,payment.metadata);
            Assert.AreEqual(status.status, Status.open);
            Assert.IsNull(status.method);
        }

        [TestMethod]
        public void CanGetPaymentMethodDetails()
        {
            PaymentMethods methods = mollieClient.GetPaymentMethods();
            Assert.IsTrue(methods.count > 0, "Found 0 payment methods");
            foreach (PaymentMethod method in methods.data)
            {
                Assert.IsTrue(method.id != "");
                Assert.IsTrue(method.description != "");
                Assert.IsTrue(method.image.bigger != "");
                Assert.IsTrue(method.image.normal != "");
                Assert.IsTrue(method.amount.minimum > -1);
                Assert.IsTrue(method.amount.maximum > -1);
            }
        }

        [TestMethod]
        public void IDEAL_CanGetIssuers()
        {
            Issuers issuers = mollieClient.GetIssuers();
            Assert.IsTrue(issuers.count > 0, "Found 0 issuers");
            foreach (Issuer issuer in issuers.data)
            {
                Assert.IsTrue(issuer.name != "");
                Assert.IsTrue(issuer.id != "");
                Assert.IsTrue(issuer.method != "");
            }
        }
        
        [TestMethod]
        public void IDEAL_CanMakePayment()
        {
            Payment payment = new Payment
            {                
                amount = 99.99M,
                description = "Test payment",
                redirectUrl = "http://www.foxip.net/test/12345",
                method = Method.ideal
            };
            PaymentStatus status = mollieClient.StartPayment(payment);
            Assert.AreEqual(status.method, Method.ideal);
        }

         [TestMethod]
         public void IDEAL_CanMakePaymentWithIssuer()
         {
             Issuers issuers = mollieClient.GetIssuers();

             Payment payment = new Payment
             {
                 amount = 99.99M,
                 description = "Test payment",
                 redirectUrl = "http://www.foxip.net/test/12345",
                 method = Method.ideal,
                 issuer = issuers.data[0].id
             };

             PaymentStatus status = mollieClient.StartPayment(payment);
             Assert.AreEqual(status.method, Method.ideal);
         }        

    }
}
