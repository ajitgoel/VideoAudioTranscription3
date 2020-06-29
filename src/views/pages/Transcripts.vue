<template>  
  <div>    
    <!--Invoice: Start-->
    <div id="invoice-page" v-if="showReceiptReceipt">
      <div class="flex flex-wrap items-center justify-between">
        <vx-input-group class="mb-base mr-3">
          <!-- <vs-input v-model="mailTo" placeholder="Email" /> -->
          <template slot="append">
            <div class="append-text btn-addon">
              <!-- <vs-button type="border" @click="mailTo = ''" class="whitespace-no-wrap">Send Invoice</vs-button> -->
            </div>
          </template>
        </vx-input-group> 
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
      <vs-tabs id="tabs" ref="tabs"> 
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
              <vx-card title="Select file to transcribe">
                <ejs-listview id='fileSources' :dataSource='fileSources' :fields='fileSourcesFields'/>
              </vx-card>  
            </div>
            <div class="vx-col lg:w-2/3 w-full">            
              <vx-card title="">
                <div class="flex items-center my-4">
                  <ejs-dropdownlist id='fileLanguage' :dataSource='fileLanguages' :fields='fileLanguagesFields' allowFiltering='true' placeholder='Select file language' ></ejs-dropdownlist>    
                </div>
                <div class="flex items-center my-4">
                  <vs-switch v-model="useVocabulary" />
                  <span class="ml-4">Use my vocabulary</span>
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
</style>
<script>
import { ListViewPlugin } from '@syncfusion/ej2-vue-lists';
import { Auth } from 'aws-amplify';
import { createUserProfile, updateUserProfile} from '@/graphql/mutations';
import {listUserProfilesForPaymentSettings} from '@/graphql/customQueries';
import API, {graphqlOperation} from '@aws-amplify/api';
import { Validator } from 'vee-validate';
import { loadStripe } from '@stripe/stripe-js';
import html2pdf from 'html2pdf.js';

export default {
  data() {
    return {
      operation:'',  
      fileSources : [
        { text: 'Your device', id: '1', category: 'Local sources'  },
        { text: 'Public links" subtitle="Eg: Youtube, Instagram etc', id: '2', category: 'Online sources'  },
        { text: 'Google drive', id: '3' , category: 'Online sources' },
        { text: 'Dropbox', id: '4', category: 'Online sources'  },
        { text: 'Your Vimeo channel', id: '5', category: 'Online sources'  },
        { text: 'Your Youtube channel', id: '6', category: 'Online sources'  },
        { text: 'Wistia', id: '7', category: 'Online sources' },
      ],
      fileSourcesFields: { groupBy: 'category' },
      //#region File Languages
      fileLanguages: 
      [
        {Category: 'Top languages', Id: 'en-GB', Text: 'English (United Kingdom)'},
        {Category: 'Top languages', Id: 'en-US', Text: 'English (United States)'},
        {Category: 'Top languages', Id: 'en', Text: 'English (Multiple Accents)'},
        {Category: 'Top languages', Id: 'es-ES', Text: 'Spanish (Spain)'},
        {Category: 'Top languages', Id: 'ca-ES', Text: 'Catalan (Spain)'},
        {Category: 'Top languages', Id: 'fr-FR', Text: 'French (France)'},
          
        {Category: 'All languages', Id: 'af-ZA', Text: 'Afrikaans (South Africa)'},
        {Category: 'All languages', Id: 'am-ET', Text: 'Amharic (Ethiopia)'},
        {Category: 'All languages', Id: 'hy-AM', Text: 'Armenian (Armenia)'},
        {Category: 'All languages', Id: 'az-AZ', Text: 'Azerbaijani (Azerbaijan)'},
        {Category: 'All languages', Id: 'id-ID', Text: 'Indonesian (Indonesia)'},
        {Category: 'All languages', Id: 'ms-MY', Text: 'Malay (Malaysia)'},
        {Category: 'All languages', Id: 'bn-BD', Text: 'Bengali (Bangladesh)'},
        {Category: 'All languages', Id: 'bn-IN', Text: 'Bengali (India)'},
        {Category: 'All languages', Id: 'ca-ES', Text: 'Catalan (Spain)'},
        {Category: 'All languages', Id: 'cs-CZ', Text: 'Czech (Czech Republic)'},
        {Category: 'All languages', Id: 'da-DK', Text: 'Danish (Denmark)'},
        {Category: 'All languages', Id: 'de-DE', Text: 'German (Germany)'},
        {Category: 'All languages', Id: 'en-AU', Text: 'English (Australia)'},
        {Category: 'All languages', Id: 'en-CA', Text: 'English (Canada)'},
        {Category: 'All languages', Id: 'en-GH', Text: 'English (Ghana)'},
        {Category: 'All languages', Id: 'en-GB', Text: 'English (United Kingdom)'},
        {Category: 'All languages', Id: 'en-IN', Text: 'English (India)'},
        {Category: 'All languages', Id: 'en-IE', Text: 'English (Ireland)'},
        {Category: 'All languages', Id: 'en-KE', Text: 'English (Kenya)'},
        {Category: 'All languages', Id: 'en-NZ', Text: 'English (New Zealand)'},
        {Category: 'All languages', Id: 'en-NG', Text: 'English (Nigeria)'},
        {Category: 'All languages', Id: 'en-PH', Text: 'English (Philippines)'},
        {Category: 'All languages', Id: 'en-ZA', Text: 'English (South Africa)'},
        {Category: 'All languages', Id: 'en-TZ', Text: 'English (Tanzania)'},
        {Category: 'All languages', Id: 'en-US', Text: 'English (United States)'},
        {Category: 'All languages', Id: 'es-AR', Text: 'Spanish (Argentina)'},
        {Category: 'All languages', Id: 'es-BO', Text: 'Spanish (Bolivia)'},
        {Category: 'All languages', Id: 'es-CL', Text: 'Spanish (Chile)'},
        {Category: 'All languages', Id: 'es-CO', Text: 'Spanish (Colombia)'},
        {Category: 'All languages', Id: 'es-CR', Text: 'Spanish (Costa Rica)'},
        {Category: 'All languages', Id: 'es-EC', Text: 'Spanish (Ecuador)'},
        {Category: 'All languages', Id: 'es-SV', Text: 'Spanish (El Salvador)'},
        {Category: 'All languages', Id: 'es-ES', Text: 'Spanish (Spain)'},
        {Category: 'All languages', Id: 'es-US', Text: 'Spanish (United States)'},
        {Category: 'All languages', Id: 'es-GT', Text: 'Spanish (Guatemala)'},
        {Category: 'All languages', Id: 'es-HN', Text: 'Spanish (Honduras)'},
        {Category: 'All languages', Id: 'es-MX', Text: 'Spanish (Mexico)'},
        {Category: 'All languages', Id: 'es-NI', Text: 'Spanish (Nicaragua)'},
        {Category: 'All languages', Id: 'es-PA', Text: 'Spanish (Panama)'},
        {Category: 'All languages', Id: 'es-PY', Text: 'Spanish (Paraguay)'},
        {Category: 'All languages', Id: 'es-PE', Text: 'Spanish (Peru)'},
        {Category: 'All languages', Id: 'es-PR', Text: 'Spanish (Puerto Rico)'},
        {Category: 'All languages', Id: 'es-DO', Text: 'Spanish (Dominican Republic)'},
        {Category: 'All languages', Id: 'es-UY', Text: 'Spanish (Uruguay)'},
        {Category: 'All languages', Id: 'es-VE', Text: 'Spanish (Venezuela)'},
        {Category: 'All languages', Id: 'eu-ES', Text: 'Basque (Spain)'},
        {Category: 'All languages', Id: 'fil-PH', Text: 'Filipino (Philippines)'},
        {Category: 'All languages', Id: 'fr-CA', Text: 'French (Canada)'},
        {Category: 'All languages', Id: 'fr-FR', Text: 'French (France)'},
        {Category: 'All languages', Id: 'gl-ES', Text: 'Galician (Spain)'},
        {Category: 'All languages', Id: 'ka-GE', Text: 'Georgian (Georgia)'},
        {Category: 'All languages', Id: 'gu-IN', Text: 'Gujarati (India)'},
        {Category: 'All languages', Id: 'hr-HR', Text: 'Croatian (Croatia)'},
        {Category: 'All languages', Id: 'zu-ZA', Text: 'Zulu (South Africa)'},
        {Category: 'All languages', Id: 'is-IS', Text: 'Icelandic (Iceland)'},
        {Category: 'All languages', Id: 'it-IT', Text: 'Italian (Italy)'},
        {Category: 'All languages', Id: 'jv-ID', Text: 'Javanese (Indonesia)'},
        {Category: 'All languages', Id: 'kn-IN', Text: 'Kannada (India)'},
        {Category: 'All languages', Id: 'km-KH', Text: 'Khmer (Cambodia)'},
        {Category: 'All languages', Id: 'lo-LA', Text: 'Lao (Laos)'},
        {Category: 'All languages', Id: 'lv-LV', Text: 'Latvian (Latvia)'},
        {Category: 'All languages', Id: 'lt-LT', Text: 'Lithuanian (Lithuania)'},
        {Category: 'All languages', Id: 'hu-HU', Text: 'Hungarian (Hungary)'},
        {Category: 'All languages', Id: 'ml-IN', Text: 'Malayalam (India)'},
        {Category: 'All languages', Id: 'mr-IN', Text: 'Marathi (India)'},
        {Category: 'All languages', Id: 'nl-NL', Text: 'Dutch (Netherlands)'},
        {Category: 'All languages', Id: 'ne-NP', Text: 'Nepali (Nepal)'},
        {Category: 'All languages', Id: 'nb-NO', Text: 'Norwegian Bokmål (Norway)'},
        {Category: 'All languages', Id: 'pl-PL', Text: 'Polish (Poland)'},
        {Category: 'All languages', Id: 'pt-BR', Text: 'Portuguese (Brazil)'},
        {Category: 'All languages', Id: 'pt-PT', Text: 'Portuguese (Portugal)'},
        {Category: 'All languages', Id: 'ro-RO', Text: 'Romanian (Romania)'},
        {Category: 'All languages', Id: 'si-LK', Text: 'Sinhala (Srilanka)'},
        {Category: 'All languages', Id: 'sk-SK', Text: 'Slovak (Slovakia)'},
        {Category: 'All languages', Id: 'sl-SI', Text: 'Slovenian (Slovenia)'},
        {Category: 'All languages', Id: 'su-ID', Text: 'Sundanese (Indonesia)'},
        {Category: 'All languages', Id: 'sw-TZ', Text: 'Swahili (Tanzania)'},
        {Category: 'All languages', Id: 'sw-KE', Text: 'Swahili (Kenya)'},
        {Category: 'All languages', Id: 'fi-FI', Text: 'Finnish (Finland)'},
        {Category: 'All languages', Id: 'sv-SE', Text: 'Swedish (Sweden)'},
        {Category: 'All languages', Id: 'ta-IN', Text: 'Tamil (India)'},
        {Category: 'All languages', Id: 'ta-SG', Text: 'Tamil (Singapore)'},
        {Category: 'All languages', Id: 'ta-LK', Text: 'Tamil (Sri Lanka)'},
        {Category: 'All languages', Id: 'ta-MY', Text: 'Tamil (Malaysia)'},
        {Category: 'All languages', Id: 'te-IN', Text: 'Telugu (India)'},
        {Category: 'All languages', Id: 'vi-VN', Text: 'Vietnamese (Vietnam)'},
        {Category: 'All languages', Id: 'tr-TR', Text: 'Turkish (Turkey)'},
        {Category: 'All languages', Id: 'ur-PK', Text: 'Urdu (Pakistan)'},
        {Category: 'All languages', Id: 'ur-IN', Text: 'Urdu (India)'},
        {Category: 'All languages', Id: 'el-GR', Text: 'Greek (Greece)'},
        {Category: 'All languages', Id: 'bg-BG', Text: 'Bulgarian (Bulgaria)'},
        {Category: 'All languages', Id: 'ru-RU', Text: 'Russian (Russia)'},
        {Category: 'All languages', Id: 'sr-RS', Text: 'Serbian (Serbia)'},
        {Category: 'All languages', Id: 'uk-UA', Text: 'Ukrainian (Ukraine)'},
        {Category: 'All languages', Id: 'he-IL', Text: 'Hebrew (Israel)'},
        {Category: 'All languages', Id: 'ar-IL', Text: 'Arabic (Israel)'},
        {Category: 'All languages', Id: 'ar-JO', Text: 'Arabic (Jordan)'},
        {Category: 'All languages', Id: 'ar-AE', Text: 'Arabic (United Arab Emirates)'},
        {Category: 'All languages', Id: 'ar-BH', Text: 'Arabic (Bahrain)'},
        {Category: 'All languages', Id: 'ar-DZ', Text: 'Arabic (Algeria)'},
        {Category: 'All languages', Id: 'ar-SA', Text: 'Arabic (Saudi Arabia)'},
        {Category: 'All languages', Id: 'ar-IQ', Text: 'Arabic (Iraq)'},
        {Category: 'All languages', Id: 'ar-KW', Text: 'Arabic (Kuwait)'},
        {Category: 'All languages', Id: 'ar-MA', Text: 'Arabic (Morocco)'},
        {Category: 'All languages', Id: 'ar-TN', Text: 'Arabic (Tunisia)'},
        {Category: 'All languages', Id: 'ar-OM', Text: 'Arabic (Oman)'},
        {Category: 'All languages', Id: 'ar-PS', Text: 'Arabic (State of Palestine)'},
        {Category: 'All languages', Id: 'ar-QA', Text: 'Arabic (Qatar)'},
        {Category: 'All languages', Id: 'ar-LB', Text: 'Arabic (Lebanon)'},
        {Category: 'All languages', Id: 'ar-EG', Text: 'Arabic (Egypt)'},
        {Category: 'All languages', Id: 'fa-IR', Text: 'Persian (Iran)'},
        {Category: 'All languages', Id: 'hi-IN', Text: 'Hindi (India)'},
        {Category: 'All languages', Id: 'th-TH', Text: 'Thai (Thailand)'},
        {Category: 'All languages', Id: 'ko-KR', Text: 'Korean (South Korea)'},
        {Category: 'All languages', Id: 'cmn-Hant-TW', Text: 'Chinese, Mandarin (Traditional, Taiwan)'},
        {Category: 'All languages', Id: 'yue-Hant-HK', Text: 'Chinese, Cantonese (Traditional, Hong Kong)'},
        {Category: 'All languages', Id: 'ja-JP', Text: 'Japanese (Japan)'},
        {Category: 'All languages', Id: 'cmn-Hans-HK', Text: 'Chinese, Mandarin (Simplified, Hong Kong)'},
        {Category: 'All languages', Id: 'cmn-Hans-CN', Text: 'Chinese, Mandarin (Simplified, China)'},
      ],
      //#endregion
      fileLanguagesFields : { groupBy: 'Category', text: 'Text', value: 'Id' },
    
      useVocabulary:false,
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
            paymentInvoices
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
        this.paymentInvoices=items[0].paymentInvoices==null? []:items[0].paymentInvoices;
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
    selectOperation(operation1)
    {
      this.operation=operation1;
      this.$refs.tabs.value=1;
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