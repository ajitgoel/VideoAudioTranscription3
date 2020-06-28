<template lang="html">
  <div>
    <vs-table search stripe max-items="999" pagination :data="paymentReceipts">
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

export default {
  data(){
    return {
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

  },
  components: {
  }
}
</script>