using System;
using NUnit.Framework;
using Mollie.Api;
using System.Configuration;
using Mollie.Api.Models;

namespace Mollie.Tests
{
    [TestFixture]
    public class UnitTests
    {
        private static MollieClient mollieClient;

        [OneTimeSetUp]
        public static void Setup()
        {
            mollieClient = new MollieClient(ConfigurationManager.AppSettings["mollie_api_key"]);
        }

        [Test]
        public void CanMakePayment()
        {
            Payment payment = new Payment
            {                
                amount = 99.99M,
                description = "Test payment",
                redirectUrl = "http://www.foxip.net/test/12345",
                metadata = "12345678"            
            };

            PaymentStatus status = mollieClient.CreatePayment(payment);

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

        [Test]
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

        [Test]
        public void CanGetIssuers()
        {
            Issuers issuers = mollieClient.ListIssuers();
            Assert.IsTrue(issuers.count > 0, "Found 0 issuers");
            foreach (Issuer issuer in issuers.data)
            {
                Assert.IsTrue(issuer.name != "");
                Assert.IsTrue(issuer.id != "");
                Assert.IsTrue(issuer.method != "");
                var issuerDetails = mollieClient.GetIssuer(issuer.id);
                Assert.AreEqual(issuerDetails.id, issuer.id);
            }
        }

        [TestCase(Method.ideal)]
        [TestCase(Method.bitcoin)]
        public void CanMakePayment(Method method)
        {
            Payment payment = new Payment
            {                
                amount = 99.99M,
                description = "Test payment",
                redirectUrl = "http://www.foxip.net/test/12345",
                method = method
            };
            PaymentStatus status = mollieClient.CreatePayment(payment);
            Assert.AreEqual(status.method, method);
        }

         [Test]
         public void CanMakePaymentWithIssuer()
         {
             Issuers issuers = mollieClient.ListIssuers();

             Payment payment = new Payment
             {
                 amount = 99.99M,
                 description = "Test payment",
                 redirectUrl = "http://www.foxip.net/test/12345",
                 method = Method.ideal,
                 issuer = issuers.data[0].id
             };

             PaymentStatus status = mollieClient.CreatePayment(payment);
             Assert.AreEqual(status.method, Method.ideal);
         }

        [Test]
        public void CanGetCreateCustomer()
        {
            CreateCustomer newCustomer = new CreateCustomer
            {
                email = "president@whitehouse.gov",
                name = "President",
                locale = "en_US",
                metadata = "something"
            };
            GetCustomer createdCustomer = mollieClient.CreateCustomer(newCustomer);

            Assert.AreEqual(newCustomer.name, createdCustomer.name);
            Assert.AreEqual(newCustomer.email, createdCustomer.email);
            Assert.AreEqual(newCustomer.metadata, createdCustomer.metadata);
            Assert.AreEqual(newCustomer.locale, createdCustomer.locale);
            Assert.AreEqual("test", createdCustomer.mode);
            Assert.AreEqual("customer", createdCustomer.resource);

            Assert.IsTrue(createdCustomer.id.Length > 1);

            GetCustomer getCustomer = mollieClient.GetCustomer(createdCustomer.id);

            Assert.AreEqual(getCustomer.name, createdCustomer.name);
            Assert.AreEqual(getCustomer.name, createdCustomer.name);
            Assert.AreEqual(getCustomer.email, createdCustomer.email);
            Assert.AreEqual(getCustomer.metadata, createdCustomer.metadata);
            Assert.AreEqual(getCustomer.locale, createdCustomer.locale);
            Assert.AreEqual("test", getCustomer.mode);
            Assert.AreEqual("customer", getCustomer.resource);

            Customers customers = mollieClient.ListCustomers();
            Assert.IsTrue(customers.data.Count>0);
        }

        [Test]
        public void CanNotGetNonExistingCustomer()
        {
            try
            {
                GetCustomer getCustomer = mollieClient.GetCustomer("non-existing-customer");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("The customer id is invalid"));
                return;
            }
            Assert.Fail("Did not throw exception");
        }

        [Test]
        public void CanGetPayments()
        {
            Payments payments = mollieClient.GetPayments();
            int count = payments.totalCount;

            PaymentStatus status = mollieClient.CreatePayment(new Payment
            {
                amount = 99.99M,
                description = "Test payment",
                redirectUrl = "http://www.foxip.net/test/12345"
            });

            payments = mollieClient.GetPayments();

            Assert.AreEqual(count + 1, payments.totalCount);

        }
       
    }
}
