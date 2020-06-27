<template>  
  <div>    
    <!--Invoice: Start-->
    <div id="invoice-page" v-if="showReceiptReceipt">
        <div class="flex flex-wrap items-center justify-between">
          <!-- <vx-input-group class="mb-base mr-3">
            <vs-input v-model="mailTo" placeholder="Email" />
            <template slot="append">
              <div class="append-text btn-addon">
                <vs-button type="border" @click="mailTo = ''" class="whitespace-no-wrap">Send Invoice</vs-button>
              </div>
            </template>
          </vx-input-group> -->
          <div class="flex items-center">
            <vs-button class="mb-base mr-3" type="border" icon-pack="feather" icon="icon icon-download" @click="downloadInvoice">Download</vs-button>
            <vs-button class="mb-base mr-3" icon-pack="feather" icon="icon icon-file" v-print="printInvoice">Print</vs-button>
          </div>
        </div>

        <vx-card id="invoice-container">

          <div class="vx-row leading-loose p-base">
            <div class="vx-col w-full md:w-1/2 mt-base">
                <img src="@/assets/images/logo/logo.png" alt="vuexy-logo">
            </div>
            <div class="vx-col w-full md:w-1/2 text-right">
                <h1>Invoice</h1>
                <div class="invoice__invoice-detail mt-6">
                    <h6 v-if="paymentReceipt.receiptNumber">INVOICE NO.</h6>
                    <p v-if="paymentReceipt.receiptNumber">{{paymentReceipt.receiptNumber}}</p>

                    <h6 class="mt-4">INVOICE DATE</h6>
                    <p>{{paymentReceipt.invoiceDate}}</p>

                    <h6 class="mt-4">Payment Method</h6>
                    <p>{{paymentReceipt.paymentMethod}}</p>
                </div>
            </div>
            <div class="vx-col w-full md:w-1/2 mt-12">
                <h5>Recipient</h5>
                <div class="invoice__recipient-info my-4">
                    <p>{{ paymentReceipt.name }}</p>
                    <p>{{ paymentReceipt.billingDetailsAddress.line1 }}</p>
                    <p>{{ paymentReceipt.billingDetailsAddress.line2 }}</p>
                    <p>{{ paymentReceipt.billingDetailsAddress.city }} {{ paymentReceipt.billingDetailsAddress.state }} {{ paymentReceipt.billingDetailsAddress.postal_code }} {{ paymentReceipt.billingDetailsAddress.country }}</p>
                </div>
                <div class="invoice__recipient-contact ">
                    <p class="flex items-center" v-if="paymentReceipt.receiptEmail">
                      <feather-icon icon="MailIcon" svgClasses="h-4 w-4"></feather-icon>
                      <span class="ml-2">{{ paymentReceipt.receiptEmail }}</span>
                    </p>
                    <p class="flex items-center" v-if="paymentReceipt.phone">
                      <feather-icon icon="PhoneIcon" svgClasses="h-4 w-4"></feather-icon>
                      <span class="ml-2">{{ paymentReceipt.phone }}</span>
                    </p>
                </div>
            </div>
             <!--  <div class="vx-col w-full md:w-1/2 mt-base text-right mt-12">
                  <h5>{{ companyDetails.name }}</h5>
                  <div class="invoice__company-info my-4">
                    <p>{{ companyDetails.addressLine1 }}</p>
                    <p>{{ companyDetails.addressLine2 }}</p>
                    <p>{{ companyDetails.zipcode }}</p>
                  </div>
                  <div class="invoice__company-contact">
                      <p class="flex items-center justify-end">
                        <feather-icon icon="MailIcon" svgClasses="h-4 w-4"></feather-icon>
                        <span class="ml-2">{{ companyDetails.mailId }}</span>
                      </p>
                      <p class="flex items-center justify-end">
                        <feather-icon icon="PhoneIcon" svgClasses="h-4 w-4"></feather-icon>
                        <span class="ml-2">{{ companyDetails.mobile }}</span>
                      </p>
                  </div>
              </div> -->
          </div>
          <div class="p-base">
            <vs-table hoverFlat :data="paymentReceipt.invoiceData.tasks">
              <template slot="thead">
                <vs-th>DESCRIPTION</vs-th>
                <vs-th>NO OF HOURS</vs-th>
                <vs-th>PRICE PER HOUR</vs-th>
                <vs-th>DISCOUNT PERCENTAGE</vs-th>             
              </template>

              <template slot-scope="{data}">
                <vs-tr v-for="(tr, index) in data" :key="index">
                  <vs-td :data="data[index].description">{{ data[index].description }}</vs-td>
                  <vs-td :data="data[index].noOFHours">{{ data[index].noOFHours }}</vs-td>
                  <vs-td :data="data[index].pricePerHour">{{ data[index].pricePerHour }} USD</vs-td>
                  <vs-td :data="data[index].discountPercentage">{{ data[index].discountPercentage }} %</vs-td>
                </vs-tr>
            	</template>
            </vs-table>

            <vs-table hoverFlat class="w-1/2 ml-auto mt-4" :data="paymentReceipt.invoiceData">
                <!-- <vs-tr>
                  <vs-th>SUBTOTAL</vs-th>
                  <vs-td>{{paymentReceipt.invoiceData.subtotal}} USD</vs-td>
                </vs-tr>
                <vs-tr>
                  <vs-th>DISCOUNT ({{paymentReceipt.invoiceData.discountPercentage}}%)</vs-th>
                  <vs-td>{{paymentReceipt.invoiceData.discountedAmount}} USD</vs-td>
                </vs-tr> -->
                <vs-tr>
                  <th>TOTAL</th>
                  <td>{{ paymentReceipt.invoiceData.total }} USD</td>
                </vs-tr>
            </vs-table>
          </div>
          <div class="invoice__footer text-left p-base">
            <p class="mb-4">If you have any questions, contact us at 
              <a href="mailto:ajitgoel@gmail.com" target="_blank" rel="nofollow">
                <feather-icon icon="MailIcon" svgClasses="h-4 w-4"></feather-icon> ajitgoel@gmail.com
              </a> or call at 
                <a href="tel:+12146065700" target="_blank" rel="nofollow">
                  <feather-icon icon="PhoneIcon" svgClasses="h-4 w-4"></feather-icon>+1 214-606-5700.
                </a></p>
            <p class="mb-4">Something wrong with the email? View it in your <a :href="paymentReceipt.receiptUrl" target="_blank" rel="nofollow">browser</a>.</p>
            <p class="mb-4">You're receiving this email because you made a purchase at 
              <a href="http://www.VideoAudioTranscription.com" target="_blank" rel="nofollow">Video Audio Transcription</a>, which partners with 
              <a href="http://www.stripe.com" target="_blank" rel="nofollow">Stripe</a> to provide invoicing and payment processing.</p>              
          </div>
          <!-- <div class="invoice__footer text-right p-base">
              <p class="mb-4">Transfer the amounts to the business amount below. Please include invoice number on your check.</p>
              <p>
                  <span class="mr-8">BANK: <span class="font-semibold">FTSBUS33</span></span>
                  <span>IBAN: <span class="font-semibold"> G882-1111-2222-3333 </span></span>
              </p>
          </div> -->
        </vx-card>
    </div>   
    <!--Invoice: End-->
    <div class="vx-row" v-else>
      <div class="vx-col lg:w-2/3 w-full">
        
          <vx-card title="Select no of hours to prepay">
            <vs-input-number id="userNoOfHours" name="userNoOfHours" ref="userNoOfHours" v-model="noOfHours" min="1" max="100"/>
            <vs-table :data="pricing">
              <template slot="thead">                    
                <vs-th>Pricing per hour</vs-th>
                <vs-th>No of hours</vs-th>
                <vs-th>Discount percentage</vs-th>
              </template>
              <template slot-scope="{data}">
                <vs-tr :key="indextr" v-for="(tr, indextr) in data">
                  <vs-td :data="data[indextr].priceperhour">
                    ${{ data[indextr].priceperhour }}/hour
                  </vs-td>
                  <vs-td :data="data[indextr].hourmin">
                    {{data[indextr].hourmin}} to {{data[indextr].hourmax}} hours
                  </vs-td>

                  <vs-td :data="data[indextr].discountPercentage">
                    {{ data[indextr].discountPercentage}} %
                  </vs-td>
                  
                </vs-tr>
              </template>
            </vs-table>               
          </vx-card>
          
        <vs-divider />

        <vx-card title="Billing details">
          <vs-input class="w-3/4 mb-base" name="fullname" ref="fullname" icon-no-border icon="icon icon-lock" icon-pack="feather" label-placeholder="Full name(Individual or company)" 
            v-model="general.fullName"></vs-input>
          <span class="text-danger text-sm">{{ errors.first('fullname') }}</span>                
          <vs-input class="w-full my-base"  name="billingaddress" ref="billingaddress" icon-no-border icon="icon icon-lock" icon-pack="feather" label-placeholder="Full Billing address" 
            v-model="general.billingAddress"></vs-input>
          <span class="text-danger text-sm">{{ errors.first('billingaddress') }}</span>              
          <vs-input class="w-1/4 my-base"  label-placeholder="Country" name="country" ref="country" icon-no-border icon="icon icon-lock" icon-pack="feather" v-model="general.country"></vs-input>
          <span class="text-danger text-sm">{{ errors.first('country') }}</span>
          
          <!-- <vs-input class="w-1/2 my-base"  label-placeholder="VAT number(if applicable)" name="vatnumber" ref="vatnumber" 
              icon-no-border icon="icon icon-lock" icon-pack="feather" v-model="general.vatNumber"></vs-input>
          <span class="text-danger text-sm">{{ errors.first('vatnumber') }}</span> -->
        </vx-card>

        <vs-divider />
        <vx-card title="Payment details">
          <div ref="stripe"/>
          <div class="flex items-center my-4">
            <vs-switch v-model="paymentSettings.autoRecharge" />
            <span class="ml-4">Auto Recharge</span>
          </div>
        </vx-card>

        <vs-button class="float-right mt-6" @click="PlaceOrder" :disabled="!validateForm">Place your order</vs-button> 
      </div>

      <div class="vx-col lg:w-1/3 w-full">
        <vx-card title="Order summary">
          <div class="flex justify-between mb-2">
              <span>No of hours</span>
              <span class="font-semibold">{{noOfHours}}</span>
          </div>
          <div class="flex justify-between mb-2">
              <span>Price per hour</span>
              <span class="text-success">${{selectedpriceperhour}}</span>
          </div>
          <div class="flex justify-between mb-2">
              <span>Auto recharge</span>
              <span class="text-success">{{autorechargetext}}</span>
          </div>
          <vs-divider />
          <div class="flex justify-between">
              <span>Total</span>
              <span class="font-semibold">${{noOfHours * selectedpriceperhour}}</span>
          </div>
        </vx-card>
      </div>    
    </div>
  </div>
</template>
<style lang="scss">
@media print {
  .invoice-page {
    * {
      visibility: hidden;
    }

    #content-area {
      margin: 0 !important;
    }

    #invoice-container,
    #invoice-container * {
      visibility: visible;
    }
    #invoice-container {
      position: absolute;
      left: 0;
      top: 0;
      box-shadow: none;
    }
  }
}
</style>
<script>
import { Auth } from 'aws-amplify';
import { createUserProfile, updateUserProfile} from '@/graphql/mutations';
import {listUserProfilesForPaymentSettings} from '@/graphql/customQueries';
import API, {graphqlOperation} from '@aws-amplify/api';
import { Validator } from 'vee-validate';
import { loadStripe } from '@stripe/stripe-js';
import html2pdf from 'html2pdf.js'

export default {
  data() {
    return {
      printInvoice: 
      {
        id: "invoice-container",
        popTitle: 'Payment Invoice',
        //extraCss: 'https://www.google.com,https://www.google.com',
        //extraHead: '<meta http-equiv="Content-Language"content="zh-cn"/>'
      },  
      //#region top off payment screen
      pricing: [
        {"id": 1,"priceperhour": 10,"hourmin": 0,"hourmax": 24, discountPercentage:0},
        {"id": 2,"priceperhour": 9,"hourmin": 25,"hourmax": 49, discountPercentage:10},
        {"id": 3,"priceperhour": 8,"hourmin": 50,"hourmax": 100, discountPercentage:20},
      ],
      stripe: null,
      card: null,
      
      noOfHours:1,
      isUserProfileSavedInDatabase: false,
      general:{email: "", fullName: "", billingAddress: "", country: "", vatNumber: ""},
      paymentSettings:{autoRecharge: false},
      showReceiptReceipt:false,
      paymentReceipt:null,
      //#endregion

      //#region payment invoice screen
      mailTo: "",
      //#endregion payment invoice screen     
    }
  },
  computed: 
  {
    autorechargetext()
    {
      return (this.paymentSettings.autoRecharge===true?"Yes":"No");
    },
    selectedpriceperhour()
    {
      for (let index = 0; index < this.pricing.length; index++) 
      {
        const element = this.pricing[index];
        if(element.hourmin <=this.noOfHours && element.hourmax >=this.noOfHours)
        {
          return element.priceperhour;
        }        
      }
      return 0;
    },
    validateForm() 
    {         
      if(this.noOfHours > 0  && this.general.fullName!="" && this.general.billingAddress != "" && this.general.country!="")
      {
        return true;
      }
      return false;
    }
  },
  async created() 
  {    
    const currentUserInfo=await this.currentUserInfo();
    this.general.email=currentUserInfo.email;
    const userId=currentUserInfo.id;
    const listUserProfilesFilter={id:{eq:userId}};      
    const listUserProfilesForTopOffPayment = /* GraphQL */ `
      query ListUserProfiles(
        $filter: ModelUserProfileFilterInput
        $limit: Int
        $nextToken: String
      ) {
        listUserProfiles(filter: $filter, limit: $limit, nextToken: $nextToken) {
          items {
            id
            fullName
            billingAddress
            country
            vatNumber
            paymentSettings {
              autoRecharge
            }
          }
          nextToken
        }
      }
    `;
    const result = await API.graphql(graphqlOperation(listUserProfilesForTopOffPayment, {filter: listUserProfilesFilter}));
    console.log(`result: ${JSON.stringify(result)}`);
    const items=result.data.listUserProfiles.items;
    console.log('items:', JSON.stringify(items));
    if(items.length>0)
    {
        this.id=items[0].id;
        this.general.fullName=items[0].fullName;
        this.general.billingAddress=items[0].billingAddress;
        this.general.country=items[0].country;
        this.general.vatNumber=items[0].vatNumber;
        this.paymentSettings.autoRecharge=(items[0].paymentSettings==null || items[0].paymentSettings.autoRecharge==null) ? false:items[0].paymentSettings.autoRecharge;
        this.isUserProfileSavedInDatabase=true;
    }
    else
    {
        this.isUserProfileSavedInDatabase=false;
    }
  },
  async mounted() 
  {    
    this.stripe = await loadStripe(process.env.VUE_APP_STRIPE_PUBLISHABLE_KEY);
    let elements = this.stripe.elements();
    const style = {
      base: {
        color: '#32325d',
        fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
        fontSmoothing: 'antialiased',
        fontSize: '16px',
        '::placeholder': {
          color: '#aab7c4',
        },
      },
      invalid: {
        color: '#fa755a',
        iconColor: '#fa755a',
      },
    };
    //https://stripe.com/docs/js/elements_object/create_element?type=card
    let options={style: style, iconStyle:'solid', hidePostalCode:true};
    this.card = elements.create('card', options);
    this.card.mount(this.$refs.stripe);
  },
  methods: 
  { 
    //# region "Top off payment" screen
    async SavePaymentMethod()
    {
      try 
      {
        this.stripe.createPaymentMethod({type: 'card',card: this.card});
      } 
      catch (error) 
      {
          console.log(error);
          this.$vs.notify({title: 'Error',text: error.message, iconPack: 'feather', icon: 'icon-alert-circle', color: 'danger'});
      };
    },
    async savePaymentSettings() 
    {
      try 
      {
          const currentUserInfo=await this.currentUserInfo();
          const userId=currentUserInfo.id;
          if(userId == null)
          {
              this.$vs.notify({title: 'Error',text: 'There was an error saving your payment settings', iconPack: 'feather', icon: 'icon-alert-circle', color: 'danger'});
              return;   
          }
          console.log(`userId: ${userId}`);
          
          //#region save user profile in dynamodb
          if(this.isUserProfileSavedInDatabase==false)
          {
            const createUserProfileInput={id:userId, paymentSettings:{autoRecharge: this.paymentSettings.autoRecharge,}};
            await API.graphql(graphqlOperation(createUserProfile,{input: createUserProfileInput}));
          }
          else
          {
              const updateUserProfileInput={id:userId, paymentSettings:{autoRecharge: this.paymentSettings.autoRecharge,}};
              await API.graphql(graphqlOperation(updateUserProfile, {input: updateUserProfileInput}));
          }                
          //#endregion save user profile in dynamodb
          this.$vs.notify({title: 'Success', text: 'Payment settings have been saved successfully!', iconPack: 'feather', icon: 'icon-check',color: 'success'}); 
      } 
      catch (error) 
      {
          console.log(error);
          this.$vs.notify({title: 'Error',text: error.message, iconPack: 'feather', icon: 'icon-alert-circle', color: 'danger'});
      };
    },
    async PlaceOrder() 
    {
      try
      {
        this.$vs.loading({text:'Please wait while your payment is being processed'});
        const initParameter = { body: {"NoOFHours": this.noOfHours, "AutoRecharge":this.paymentSettings.autoRecharge, "Email":this.general.email}, headers: {},};
        let paymentIntent=await API.post('CreatePaymentIntent', '/CreatePaymentIntent', initParameter);        
        console.log('paymentIntent:', JSON.stringify(paymentIntent));
        let clientSecret=paymentIntent.clientSecret;

        //#region Process card payment
        // Stripe.js has not yet loaded. Make sure to disable form submission until Stripe.js has loaded.   
        if (this.stripe===false || this.stripe.elements() === false) 
        {
          return;   
        }
        
        //https://stripe.com/docs/js/payment_intents/confirm_card_payment
        let data = {receipt_email: this.general.email, payment_method: {card: this.card, billing_details: {name: this.general.fullName,},}};
        const confirmCardPaymentResult = await this.stripe.confirmCardPayment(clientSecret, data);
        console.log(`confirmCardPaymentResult: ${JSON.stringify(confirmCardPaymentResult)}`);

        if (confirmCardPaymentResult.error) 
        {
          console.log(`confirmCardPaymentResult.error: ${confirmCardPaymentResult.error}`);
          this.$vs.notify({title: 'Error', text: `${confirmCardPaymentResult.error.message}. Please correct the error to continue.`, iconPack: 'feather', icon: 'icon-alert-circle', color: 'danger'});        
          this.showReceiptReceipt=false;
          return;
        }

        if (confirmCardPaymentResult.paymentIntent.status === 'succeeded') 
        {
          let paymentIntentId=confirmCardPaymentResult.paymentIntent.id;

          const options = { body: {"paymentIntentId": paymentIntentId}, headers: {},};
          this.paymentReceipt=await API.post('PaymentIntent', '/Get', options);              
          console.log(`paymentReceipt: ${JSON.stringify(this.paymentReceipt)}`); 
          
          this.$vs.notify({title: 'Payment success', text: 'Your payment was successful!', iconPack: 'feather', icon: 'icon-check',color: 'success'}); 
          // There's a risk of the customer closing the window before callback execution. 
          //Set up a webhook or plugin to listen for the payment_intent.succeeded event that handles any business critical post-payment actions.
        }
        //#endregion

        await this.savePaymentSettings();
        this.showReceiptReceipt=true;
      }
      catch(error)
      {
        console.log(`error: ${JSON.stringify(error)}`); 

        this.$vs.notify({title: 'Error',text: error.message, iconPack: 'feather', icon: 'icon-alert-circle', color: 'danger'});        
        this.showReceiptReceipt=false;
        return;
      }
      finally
      {        
        this.$vs.loading.close();
      };
    },
    //#endregion 
    //# region "Invoice receipt" screen
      downloadInvoice()
      {
        var element = document.getElementById('invoice-container');
        html2pdf().from(element).toPdf().save('Payment Invoice');
      }
      //#endregion 
  },
  components: {
  }
}
</script>