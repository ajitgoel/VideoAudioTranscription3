<template lang="html">
  <div>
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
        <vs-button class="mb-base mr-3" type="border" icon-pack="feather" icon="icon icon-download" @click="downloadInvoices">Download</vs-button>
        <vs-button class="mb-base mr-3" icon-pack="feather" icon="icon icon-file" v-print="printInvoice">Print</vs-button>
      </div>
    </div>

    <vs-table search stripe max-items="999" id="paymentReceipts" pagination :data="paymentReceipts">
      <template slot="thead">
        <vs-th>Invoice date</vs-th>
        <vs-th>Payment method</vs-th>
        <vs-th>Total amount</vs-th>
        <vs-th>Receipt url</vs-th>   
      </template>
      <template slot-scope="{data}">
        <vs-tr :key="id" v-for="(tr, id) in data">
          <vs-td :data="data[id].invoiceDate">
            {{ data[id].invoiceDate }}
          </vs-td>
          <vs-td :data="data[id].paymentMethod">
            {{ data[id].paymentMethod}}
          </vs-td>
          <vs-td :data="data[id].invoiceData.total">
            {{ data[id].invoiceData.total }} USD
          </vs-td>
          
          <vs-td :data="data[id].receiptUrl">
            <a :href="data[id].receiptUrl" target="_blank" rel="nofollow">Click here</a>
          </vs-td>

          <template slot="expand">
            <vs-list>
              <vs-list-item title="Details"></vs-list-item>
              <vs-list-item title="No of hours purchased" :subtitle="data[id].invoiceData.tasks[0].noOFHours"></vs-list-item>
              <vs-list-item title="Price per hour" :subtitle="data[id].invoiceData.tasks[0].pricePerHour + ' USD'"></vs-list-item>
              <vs-list-item title="Discount percentage" :subtitle="data[id].invoiceData.discountPercentage + ' %'"></vs-list-item>
            </vs-list>
          </template>          

        </vs-tr>
      </template>
    </vs-table>
  </div>
</template>

<script>
import { Auth } from 'aws-amplify';
import { createUserProfile, updateUserProfile} from '@/graphql/mutations';
import {listUserProfilesForPaymentSettings} from '@/graphql/customQueries';
import API, {graphqlOperation} from '@aws-amplify/api';
import html2pdf from 'html2pdf.js';

export default {
  data(){
    return {
      printInvoice: 
      {
        id: "paymentReceipts",
        popTitle: 'Payment Invoices',
        //extraCss: 'https://www.google.com,https://www.google.com',
        //extraHead: '<meta http-equiv="Content-Language"content="zh-cn"/>'
      },  
      paymentReceipts:[],
    }
  },
  async created() 
  {    
    try
    {
      this.$vs.loading({text:'Please wait while the system loads your data'});        
      const currentUserInfo=await this.currentUserInfo();
      const userId=currentUserInfo.id;
      const listUserProfilesFilter={id:{eq:userId}};      
      const listUserProfiles = /* GraphQL */ `
        query ListUserProfiles(
          $filter: ModelUserProfileFilterInput
          $limit: Int
          $nextToken: String
        ) {
          listUserProfiles(filter: $filter, limit: $limit, nextToken: $nextToken) {
            items {
              id
              paymentInvoices
            }
            nextToken
          }
        }
      `;
      const result = await API.graphql(graphqlOperation(listUserProfiles, {filter: listUserProfilesFilter}));
      console.log(`result: ${JSON.stringify(result)}`);
      let paymentInvoices= result.data.listUserProfiles.items[0].paymentInvoices;
      const options = { body: {"paymentIntentIds": paymentInvoices}, headers: {},};
      console.log(`options: ${JSON.stringify(options)}`); 
      this.paymentReceipts=await API.post('PaymentIntent', '/Get', options);              
      console.log(`paymentReceipt: ${JSON.stringify(this.paymentReceipts)}`);
    }    
    finally
    {        
      this.$vs.loading.close();
    }; 
  },
  methods:
  {    
    downloadInvoices()
    {
      var element = document.getElementById('paymentReceipts');
      html2pdf().from(element).toPdf().save('Payment Invoices');
    }
  },
  components: {
  }
}
</script>