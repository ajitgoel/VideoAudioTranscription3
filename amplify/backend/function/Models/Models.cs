using Newtonsoft.Json;
using System;

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
  }
  public class CreatePaymentIntentInput
  {
    [JsonProperty(Required = Required.Always)]
    public int NoOFHours { get; set; }
    [JsonProperty(Required = Required.Always)]
    public bool AutoRecharge { get; set; } = false;
    [JsonProperty(Required = Required.Always)]
    public string Email { get; set; }
  }
  public class CreatePaymentIntentOutput
  {
    [JsonProperty(Required = Required.Always)]
    public string ClientSecret { get; set; }
  }
}
