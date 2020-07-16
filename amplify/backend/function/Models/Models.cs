using Newtonsoft.Json;
using System.Collections.Generic;

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

  public static class Extensions
  {
    public static string Default_File_Language_When_File_Is_Transcribed="en-US";
    public static int Max_Speaker_Labels = 10; 
    public static string SerializeObjectIgnoreReferenceLoopHandling(object objectToSerialize)
    {
      return JsonConvert.SerializeObject(objectToSerialize, new JsonSerializerSettings()
      {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
      });
    }
    public static string IsNullOrWhiteSpaceWithDefault(this string stringInstance, string defaultValue)
    {
      return string.IsNullOrWhiteSpace(stringInstance)? defaultValue : stringInstance;
    }
    public static bool IsNullOrWhiteSpaceWithDefault(this string stringInstance, bool defaultValue)
    {
      return string.IsNullOrWhiteSpace(stringInstance) ? defaultValue : System.Convert.ToBoolean(stringInstance);
    }
  }
}
