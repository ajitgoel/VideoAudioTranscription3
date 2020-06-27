using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Models
{
  public class Constants
  {
    public const string USD_CURRENCY = "usd";
    public const string CARD_PAYMENT_METHOD = "card";
  }
  public class Pricing
  {
    public int Id { get; set; }
    public int Priceperhour { get; set; }
    public int Hourmin { get; set; }
    public int Hourmax { get; set; }
    public int DiscountPercentage { get; set; }
  }
  public class PaymentReceipt
  {
    [JsonProperty("body")]
    public string Body { get; set; }
  }
  public class ListAllChargesInput
  {
    [JsonProperty(Required = Required.Always)]
    public string PaymentIntentId { get; set; }
  }
  public class ListAllChargesOutput
  {
    public string ReceiptUrl { get; set; }
  }
  public class Methods
  {
    public Pricing GetPricePerHour(int noOfHours)
    {
      var pricings = new List<Pricing>
            {
              new Pricing{Id= 1, Priceperhour= 10, Hourmin=0, Hourmax= 24,DiscountPercentage=0},
              new Pricing{Id= 2, Priceperhour= 9, Hourmin=25, Hourmax= 49,DiscountPercentage=10},
              new Pricing{Id= 3, Priceperhour= 8, Hourmin=50, Hourmax= 100,DiscountPercentage=20}
            };
      for (var index = 0; index < pricings.Count; index++)
      {
        var element = pricings[index];
        if (element.Hourmin <= noOfHours && element.Hourmax >= noOfHours)
        {
          return element;
        }
      }
      return null;      
    }
  }
}
