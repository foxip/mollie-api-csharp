mollie-api-csharp
=================

Mollie API client for C#

## How to use the API client ##

Initializing the Mollie API client, and setting your API key.

```c#
MollieClient mollieClient = new MollieClient("your_api_key_here");
```

Loading iDeal issuers

```c#
Issuers issuers = mollieClient.GetIssuers();
foreach (Issuer issuer in issuers.data)
{
   Console.WriteLine(issuer.name);
}
```

Creating a new payment.

```c#
Payment payment = new Payment 
{ 
   amount = 99.99M, 
   description = "Test payment", 
   redirectUrl = "http://www.myshop.net/payments/completed/?orderId=1245",
   webhookUrl = "http://www.myshop.net/webhooks/mollie/",
};
PaymentStatus paymentStatus = mollieClient.StartPayment(payment);
string molliePaymentId = paymentStatus.id;
Response.Redirect(paymentStatus.links.paymentUrl);
```

Getting payment status

```c#
PaymentStatus paymentStatus = mollieClient.GetStatus(molliePaymentId);
if (paymentStatus.status == Status.paid)
{
   Response.Write("Your order is paid");
}
```

Refunds

```c#
RefundStatus refundStatus = mollieClient.Refund(molliePaymentId);
```

PaymentMethods

```c#
PaymentMethods methods = mollieClient.GetPaymentMethods();
foreach (PaymentMethod method in methods.data)
{
	Console.WriteLine(method.image.normal);
}
```
