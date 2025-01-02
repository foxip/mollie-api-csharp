mollie-api-csharp
=================

This is a partially implemented client for the Mollie API v2.

If you need the full implementation, I would suggest to use:
https://github.com/Viincenttt/MollieApi

## Implemented methods

### Payments API

CreatePayment(Payment payment)
GetPayment(string id)
ListPayments()

### Refunds API

CreateRefund(string id, Refund refund)
ListRefunds(string id)

### Methods API

GetPaymentMethod(string id)
ListPaymentMethods()

### Customers API

CreateCustomer(CreateCustomer customer)
GetCustomer(string id)
ListCustomers()

### Mandates API

ListMandates(string customerId)

## How to use the API client ##

Initializing the Mollie API client, and setting your API key.

```c#
var mollieClient = new MollieClient("your_api_key_here");
```

Loading iDeal issuers

```c#
var paymentMethod = await mollieClient.GetPaymentMethod(Method.ideal);
foreach (var issuer in paymentMethod.issuers)
{
   Console.WriteLine(issuer.name);
}
```

Creating a new payment.

```c#
var payment = new Payment 
{ 
   amount = new Amount { currency = "EUR", value = "99.99" }, 
   description = "Test payment", 
   
   redirectUrl = "http://www.myshop.net/payments/completed/?orderId=1245",
   webhookUrl = "http://www.myshop.net/webhooks/mollie/"
};
var paymentStatus = await mollieClient.CreatePayment(payment);
var molliePaymentId = paymentStatus.id;
Response.Redirect(paymentStatus._links.checkout.href);
```

Getting payment status

```c#
var paymentStatus = await mollieClient.GetStatus(molliePaymentId);
if (paymentStatus.status == Status.paid)
{
   Console.WriteLine("Your order is paid");
}
```
