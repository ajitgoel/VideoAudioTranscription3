<template>  
  <div class="vx-row">
    <vs-tabs id="tabs" ref="tabs" :value="currentTab"> 
      <vs-tab label="Operation type">
        <vx-card title="Select type of operation">
          <vs-row vs-justify="center">
              <vs-col type="flex" vs-justify="center" vs-align="center" vs-w="6">
                  <vs-card>
                      <div slot="header">
                          <h6>Transcription</h6>
                      </div>
                      <div>
                          <span>Some text about difference between transcription and subtitles operation</span>
                      </div>
                      <div slot="footer">
                          <vs-row vs-justify="flex-end">
                              <vs-button @click="selectOperation('transcription')">Select</vs-button>
                          </vs-row>
                      </div>
                  </vs-card>
              </vs-col>
              <vs-col type="flex" vs-justify="center" vs-align="center" vs-w="6">
                  <vs-card>
                      <div slot="header">
                          <h6>Subtitles</h6>
                      </div>
                      <div>
                          <span>Some text about difference between transcription and subtitles operation</span>
                      </div>
                      <div slot="footer">
                          <vs-row vs-justify="flex-end">
                              <vs-button @click="selectOperation('subtitles')">Select</vs-button>
                          </vs-row>
                      </div>
                  </vs-card>
              </vs-col>
          </vs-row>
        </vx-card>
      </vs-tab>
      <vs-tab label="Upload file">
          <div class="vx-row">
          <div class="vx-col lg:w-1/3 w-full">  
            <vx-card title="Select file source">
              <ejs-listview id='fileSources' :dataSource='$options.fileSources' :fields='fileSourcesFields'/>
            </vx-card>  
          </div>
          <div class="vx-col lg:w-2/3 w-full">            
            <vx-card title="Select file to transcribe and transcription settings">
              <div class="flex items-center my-6">
                <ejs-uploader ref="localDeviceFiles" id='localDeviceFiles' name="localDeviceFiles" :selected="localDeviceFilesSelected" 
                  :allowedExtensions="$options.audioVideoFileExtensions"></ejs-uploader>
              </div>
              <h6 class="mb-4">Select file language</h6>
              <div class="flex items-center my-6">
                <ejs-dropdownlist id='fileLanguage' :dataSource='$options.fileLanguages' :fields='fileLanguagesFields' 
                  v-model="transcriptionSettings.defaultFileLanguageWhenFileIsTranscribed" allowFiltering='true' placeholder='Select file language' />
              </div>
              <div class="flex items-center my-6">
                <vs-switch v-model="transcriptionSettings.useVocabularyWhenFileIsTranscribed" />
                <span class="ml-4">Use my vocabulary</span>
              </div>

              <div class="flex items-center mb-4">
                <vs-switch v-model="notificationSettings.notifyWhenTranscriptsCompleted" />
                <span class="ml-4">Notify me when my transcripts are done processing</span>
              </div>
              <div class="flex items-center mb-4">
                <vs-switch v-model="notificationSettings.notifyWhenTranscriptsError" />
                <span class="ml-4">Notify me when there is a problem with my transcripts</span>
              </div>
              
            </vx-card>               
          </div>
        </div>
      </vs-tab>

      <vs-tab label="Payment details">
        <vx-card title="Payment details">
          <div ref="stripe"/>
          <div class="flex items-center my-4">
            <vs-switch v-model="paymentSettings.autoRecharge" />
            <span class="ml-4">Auto Recharge</span>
          </div>
        </vx-card>
        <vs-button class="float-right mt-6" @click="PlaceOrder" :disabled="!validateForm">Place your order</vs-button> 
      </vs-tab>
    </vs-tabs>

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
#fileSources 
{
  border: 1px solid #dddddd;
  border-radius: 3px;
}
.control-section 
{
  height: 100%;
  min-height: 200px;
}
</style>
<script>
import { ListViewPlugin } from '@syncfusion/ej2-vue-lists';
import { Auth } from 'aws-amplify';
import { createUserProfile, updateUserProfile} from '@/graphql/mutations';
import API, {graphqlOperation} from '@aws-amplify/api';
import { Validator } from 'vee-validate';
import {FILE_SOURCES, FILE_LANGUAGES, AUDIO_VIDEO_FILE_EXTENSIONS} from '@/static.js';

export default {
  data() {
    return {
      operation:'',
      fileSourcesFields: { groupBy: 'category' },
      fileLanguagesFields : { groupBy: 'Category', text: 'Text', value: 'Id' },
      transcriptionSettings: {defaultFileLanguageWhenFileIsTranscribed:false, useVocabularyWhenFileIsTranscribed:false},
      notificationSettings: {notifyWhenTranscriptsCompleted:false, notifyWhenTranscriptsError:false},
      currentTab:0,

      printInvoice: 
      {
        id: "invoice-container",
        popTitle: 'Payment Invoice',
        //extraCss: 'https://www.google.com,https://www.google.com',
        //extraHead: '<meta http-equiv="Content-Language"content="zh-cn"/>'
      },  
      //#region top off payment screen
      stripe: null,
      card: null,
      
      noOfHours:1,
      isUserProfileSavedInDatabase: false,
      general:{email: "", fullName: "", billingAddress: "", country: "", vatNumber: ""},
      paymentInvoices:[],
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
    this.$options.fileSources = FILE_SOURCES;
    this.$options.fileLanguages = FILE_LANGUAGES;
    this.$options.audioVideoFileExtensions = AUDIO_VIDEO_FILE_EXTENSIONS;
    const currentUserInfo=await this.currentUserInfo();
    this.general.email=currentUserInfo.email;
    const userId=currentUserInfo.id;
    const listUserProfilesFilter={id:{eq:userId}};      
    const listUserProfilesForTranscripts = /* GraphQL */ `
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
            paymentInvoices
            paymentSettings {
              autoRecharge
            }  
            transcriptionSettings {
              defaultFileLanguageWhenFileIsTranscribed
              useVocabularyWhenFileIsTranscribed
            }
            notificationSettings {
              notifyWhenTranscriptsCompleted
              notifyWhenTranscriptsError
            }
          }
          nextToken
        }
      }
    `;
    const result = await API.graphql(graphqlOperation(listUserProfilesForTranscripts, {filter: listUserProfilesFilter}));
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
        this.paymentInvoices=items[0].paymentInvoices==null? []:items[0].paymentInvoices;
        this.paymentSettings.autoRecharge=(items[0].paymentSettings==null || items[0].paymentSettings.autoRecharge==null) ? false:items[0].paymentSettings.autoRecharge;
        this.transcriptionSettings.defaultFileLanguageWhenFileIsTranscribed=
          (items[0].transcriptionSettings==null || items[0].transcriptionSettings.defaultFileLanguageWhenFileIsTranscribed==null) ? 
          false:items[0].transcriptionSettings.defaultFileLanguageWhenFileIsTranscribed;
        this.transcriptionSettings.useVocabularyWhenFileIsTranscribed=
          (items[0].transcriptionSettings==null || items[0].transcriptionSettings.useVocabularyWhenFileIsTranscribed==null) ? 
          false:items[0].transcriptionSettings.useVocabularyWhenFileIsTranscribed;

        this.notificationSettings.notifyWhenTranscriptsCompleted=
          (items[0].notificationSettings==null || items[0].notificationSettings.notifyWhenTranscriptsCompleted==null) ? 
          false:items[0].notificationSettings.notifyWhenTranscriptsCompleted;
        this.notificationSettings.notifyWhenTranscriptsError=
          (items[0].notificationSettings==null || items[0].notificationSettings.notifyWhenTranscriptsError==null) ? 
          false:items[0].notificationSettings.notifyWhenTranscriptsError;

        this.isUserProfileSavedInDatabase=true;
    }
    else
    {
        this.isUserProfileSavedInDatabase=false;
    }
  },
  async mounted() 
  {  
  },
  methods: 
  { 
    localDeviceFilesSelected(args)
    {
      console.log(`localDeviceFilesSelected: ${JSON.stringify(args)}`); 
    },
    selectOperation(operation1)
    {
      this.operation=operation1;
      this.currentTab=1;
    },
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
    async saveUserProfile(paymentIntentId) 
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
          let paymentInvoicesTemp=[paymentIntentId];
          const createUserProfileInput={id:userId, paymentInvoices:paymentInvoicesTemp, paymentSettings:{autoRecharge: this.paymentSettings.autoRecharge,}};
          await API.graphql(graphqlOperation(createUserProfile,{input: createUserProfileInput}));
        }
        else
        {
          this.paymentInvoices.push(paymentIntentId);
          const updateUserProfileInput={id:userId, paymentInvoices:this.paymentInvoices, paymentSettings:{autoRecharge: this.paymentSettings.autoRecharge,}};
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
          let paymentIntentIds=[paymentIntentId];
          const options = { body: {"paymentIntentIds": paymentIntentIds}, headers: {},};
          let paymentReceipts=await API.post('PaymentIntent', '/Get', options);              
          console.log(`paymentReceipts: ${JSON.stringify(paymentReceipts)}`); 
          this.paymentReceipt=paymentReceipts[0];              

          await this.saveUserProfile(paymentIntentId);
          this.showReceiptReceipt=true;
          this.$vs.notify({title: 'Payment success', text: 'Your payment was successful!', iconPack: 'feather', icon: 'icon-check',color: 'success'}); 
          return;
          // There's a risk of the customer closing the window before callback execution. 
          //Set up a webhook or plugin to listen for the payment_intent.succeeded event that handles any business critical post-payment actions.
        }
        //#endregion
        await this.saveUserProfile();
        this.showReceiptReceipt=false;
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