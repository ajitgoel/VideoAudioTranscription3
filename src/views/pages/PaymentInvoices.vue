<template lang="html">
  <div>
    <vs-table max-items="10" pagination :data="users">
      <template slot="thead">
        <vs-th>Email</vs-th>
        <vs-th>Name</vs-th>
        <vs-th>Website</vs-th>
        <vs-th>Nro</vs-th>
      </template>
      <template slot-scope="{data}">
        <vs-tr :key="indextr" v-for="(tr, indextr) in data">
          <vs-td :data="data[indextr].email">
            {{ data[indextr].email }}
          </vs-td>
          <vs-td :data="data[indextr].username">
            {{ data[indextr].username }}
          </vs-td>
          <vs-td :data="data[indextr].id">
            {{ data[indextr].website }}
          </vs-td>
          <vs-td :data="data[indextr].id">
            {{ data[indextr].id }}
          </vs-td>
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
//import { Validator } from 'vee-validate';
//import { loadStripe } from '@stripe/stripe-js';
//import html2pdf from 'html2pdf.js';

export default {
  data: () => ({
    users: [
      {
        "id": 1,
        "name": "Leanne Graham",
        "username": "Bret",
        "email": "Sincere@april.biz",
        "website": "hildegard.org",
      },
      {
        "id": 2,
        "name": "Ervin Howell",
        "username": "Antonette",
        "email": "Shanna@melissa.tv",
        "website": "anastasia.net",
      },
      {
        "id": 3,
        "name": "Clementine Bauch",
        "username": "Samantha",
        "email": "Nathan@yesenia.net",
        "website": "ramiro.info",
      },
      {
        "id": 4,
        "name": "Patricia Lebsack",
        "username": "Karianne",
        "email": "Julianne.OConner@kory.org",
        "website": "kale.biz",
      },
      {
        "id": 5,
        "name": "Chelsey Dietrich",
        "username": "Kamren",
        "email": "Lucio_Hettinger@annie.ca",
        "website": "demarco.info",
      },
      {
        "id": 6,
        "name": "Mrs. Dennis Schulist",
        "username": "Leopoldo_Corkery",
        "email": "Karley_Dach@jasper.info",
        "website": "ola.org",
      },
      {
        "id": 7,
        "name": "Kurtis Weissnat",
        "username": "Elwyn.Skiles",
        "email": "Telly.Hoeger@billy.biz",
        "website": "elvis.io",
      },
      {
        "id": 8,
        "name": "Nicholas Runolfsdottir V",
        "username": "Maxime_Nienow",
        "email": "Sherwood@rosamond.me",
        "website": "jacynthe.com",
      },
      {
        "id": 9,
        "name": "Glenna Reichert",
        "username": "Delphine",
        "email": "Chaim_McDermott@dana.io",
        "website": "conrad.com",
      },
      {
        "id": 10,
        "name": "Clementina DuBuque",
        "username": "Moriah.Stanton",
        "email": "Rey.Padberg@karina.biz",
        "website": "ambrose.net",
      }
    ]
  }),
  async created() 
  {    
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
    
    const options = { body: {"paymentIntentIds": result.data.listUserProfiles.items[0].paymentInvoices}, headers: {},};
    console.log(`options: ${JSON.stringify(options)}`); 
    let paymentReceipt=await API.post('PaymentIntent', '/Get', options);              
    console.log(`paymentReceipt: ${JSON.stringify(paymentReceipt)}`); 
    
    /* const items=result.data.listUserProfiles.items;
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
    } */
  },
}
</script>