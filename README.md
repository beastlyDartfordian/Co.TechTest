# Checkout Tech Test - Payment Gateway App

Simple Payment Gateway API created with DotNet Core, Entity Framework Core for data storage, and NUnit.

## Table of Contents
* [General Info](#general-info)
* [Technologies](#technologies)
* [Setup](#setup)
* [Status and Future Improvements](#status-and-future-improvements)
* [API Documentation](#api-documentation)

## General Info

This simple app / api as required by the Checkout.com Technical Test to represent a payment gateway for merchants to take payment for 
goods and link into acquiring banks.

A Merchant can POST a payment which is then sent through the gateway and "processed" (mocked process) by the acquiring bank before the result
 is stored & sent back to the merchant. The merchant can also GET a payment back from the service that it had sent previously.

By default this app runs with mocked data storage (MockMerchantRepository & MockPaymentRepository) for Merchants & Payments, although this
 can be easily switch for Entity Framework Core (SQL Server) based data storage by changing the couplings in Startup.cs

All in, this works accounts for roughly 12 hours of time in development.

## Technologies

* DotNet Core 3.1
* EntityFramework Core (SQL Server) 3.1
* NUnit 3.12
* NLog 4.6 via NLog.Web.AspNetCore 4.9

## Setup

There are 2 ways to run this code: with or without a SQL DB for data storage.

1. Out of the box it will run without the SQL DB, using lists as the data repository. 
   1. Open project in Visual Studio
   2. Run app
2. Using EF Core (SQL Server)
   1. Open project in Visual Studio
   2. Enter connection string into appsettings.json => Connection Strings => DBConnection
   3. Run the migrations bying going to Package Manager Console => Type Update-Database & hit enter
   4. Go to startup.cs and change IMerchantRepository & IPaymentRepository to use MerchantRepository & PaymentRepository instead of the Mock ones
   5. Run app

By default both methods have at least 1 merchant in seed data, and the ID of this merchant should be used when sending requests:
* Name = Adidas
* Id = bd2347d5-98b3-4d4e-8773-4eb4b33ca0d6


## Status and Future Improvements

No more improvements are planned at this time.

While I am at a point where I am happy that the codebase here represents a version that can fulfills the requirements of the checkout technical
test, there are a number of improvements that I feel could be made to the betterment and overall completeness if more time was available:
* Performance Test
  * A key component of a true customer facing API. This can be used to make sure that the code responds in a reasonable time frame and make
  sure that any code changes do not adversely effect the performance of the API.
* Authentication
  * The natural next step for the code base is to set up authentication so that only allowed merchants can use the platform, and help prevent
  bad actors from accessing the data in the system
  * With the use of Entity Framework in the project it makes sense to use Identity along EF to provide this. By presenting the merchants with
an ID (already provided), and a password an initial auth request can return a bearer token (it authed successfully) which can then be used
in future request headers to access other elements of the API (e.g. Payment).
  * Additionally this could be silo'd off into a seperate service to split the burden of the two elements. By decoupling auth from payments
it can then be used to authenticate for seperate services without having a detrimental effect on the payment gateway
* Encryption
  * As we're potentially handling sensitive data (credit card details), it would be wise to encrypt the data on the client side and then
decrypt it on the side of the gateway. This way it makes it harder to steal this data while in transit, by someone listening in on the
connection. This is also why a move to HTTPS is key, especially in production.
* Client App
  * A simple client app could be provided to showcase the abilities of the API. A simple one page app built with react or angular, could show
the ability of the app quite nicely, without the need of a tool such as Postman.
* Build Script
  * If a build script was available it could be run to set up the apps ready to just be tried out, instead of having to go through steps 
like running migrations and chosing which classes to use.

## API Documentation

There is only one controller in this project which contains a post and a get:

#### Payment
* GET: ``` /payment/{merchantId}+{reference} ```
  * E.g.: /payment/bd2347d5-98b3-4d4e-8773-4eb4b33ca0d6+reference1
  * Response: Payment (See Below)
* POST: ``` /payment ```
  *  Body: Payment (See Below)
  *  Response: Payment (See Below)

##### Model


```javascript
star (*) denotes required on POST request

{
    id: integer
    merchantTransactionReference*: string
    merchantId*: string
    merchant: 
    {
        id: string
        name: string
        payments: []
    },
    bankTransactionReference: string
    status: int
        enum:
            - Unpaid = 0
            - Paid = 1
    paymentMethod*: int
        enum:
            - Card = 0
    paymentDetailsId: 0
    paymentDetails*: 
    {
        id: 0
        cardNumber*: string,
        name*: string,
        startMonth: string
        startYear: string
        expiryMonth*: string
        expiryYear*: string
        cvv*: string
        scheme*: int
            enum:
                - Visa = 0
                - Mastercard = 1
                - AmericanExpress = 2
                - Discover = 3
                - DinersClub = 4
                - JCB = 5
                - UnionPay = 6
        type*: int
            enum:
                - Debit = 0
                - Credit = 1
        addressId: int
        address*: 
        {
            id: int
            name: string,
            number: string,
            street*: string,
            city*: string
            county*: string
            postCode*: "TE5 7PC",
        },
        "payment": null
    },
    amount*: decimal,
    currency*: int
        enum:
            - GBP = 0
            - USD = 1
            - EUR = 2
}
```