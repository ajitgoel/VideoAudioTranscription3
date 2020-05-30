/*
Copyright 2017 - 2017 Amazon.com, Inc. or its affiliates. All Rights Reserved.
Licensed under the Apache License, Version 2.0 (the "License"). You may not use this file except in compliance with the License. A copy of the License is located at
    http://aws.amazon.com/apache2.0/
or in the "license" file accompanying this file. This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and limitations under the License.
*/
var express = require('express')
var bodyParser = require('body-parser')
var awsServerlessExpressMiddleware = require('aws-serverless-express/middleware')

// declare a new express app
var app = express()
app.use(bodyParser.json())
app.use(awsServerlessExpressMiddleware.eventContext())

// Enable CORS for all methods
app.use(function(req, res, next) {
  res.header("Access-Control-Allow-Origin", "*")
  res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept")
  next()
});

app.get('/pricing', function(req, res) {
  const result= [
    {"id": 1,"priceperhour": 10,"hourmin": 0,"hourmax": 24,},
    {"id": 2,"priceperhour": 9,"hourmin": 25,"hourmax": 49,},
    {"id": 3,"priceperhour": 8,"hourmin": 50,"hourmax": 100,},
  ];
  res.json({result: result, url: req.url});
});

app.post('/postsingle', function(req, res) 
{
  try 
  {
    let customercreateRequest={name: req.body.name, email: req.body.email, source: req.body.stripeToken};
    let customer=await stripe.customers.create(customercreateRequest);
    let chargecreateRequest={amount: req.body.amount * 100, currency: 'usd', customer: customer.id, description: 'Thank you for your generous donation.'};
    await stripe.charges.create(chargecreateRequest);
    res.json({success: 'post call succeed!', url: req.url, body: req.body});
  } 
  catch (error) 
  { 
    res.send(error);
  }  
});
/* 
app.post('/postsingle/*', function(req, res) {
  // Add your code here
  res.json({success: 'post call succeed!', url: req.url, body: req.body})
});
 */
app.listen(3000, function() {
    console.log("App started")
});

// Export the app object. When executing the application local this does nothing. However,
// to port it to AWS Lambda we will create a wrapper around that will load the app from
// this file
module.exports = app
