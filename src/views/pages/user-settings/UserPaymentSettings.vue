<template>
  <vx-card no-shadow>
    <div class="mb-base">
      <h6 class="mb-4">Payment settings</h6>
      <div class="flex items-center mb-4">
        <vs-switch v-model="paymentSettings.autoRecharge" />
        <span class="ml-4">Auto Recharge</span>
      </div>
    </div>
    <div class="flex flex-wrap items-center justify-end mt-base">
      <vs-button class="ml-auto mt-2" @click="savePaymentSettings">Save payment settings</vs-button>
    </div>
  </vx-card>
</template>
<script>
import { Auth } from 'aws-amplify';
import { createUserProfile, updateUserProfile} from '@/graphql/mutations';
import {listUserProfilesForPaymentSettings} from '@/graphql/customQueries';
import API, {graphqlOperation} from '@aws-amplify/api';

export default {
  data() {
    return {
      isUserProfileSavedInDatabase: false,
      paymentSettings:{autoRecharge: false}
    }
  },
  computed: { 
    activeUserInfo() {
      return this.$store.state.AppActiveUser
    },   
  },
  async created() 
  {
      const currentUserInfo=await this.currentUserInfo();
      const userId=currentUserInfo.id;
      const listUserProfilesFilter={id:{eq:userId}};
      const result = await API.graphql(graphqlOperation(listUserProfilesForPaymentSettings, {filter: listUserProfilesFilter}));
      console.log(`result: ${JSON.stringify(result)}`);
      const items=result.data.listUserProfiles.items;
      if(items.length>0)
      {
          this.id=items[0].id;
          this.paymentSettings.autoRecharge=(items[0].paymentSettings==null || items[0].paymentSettings.autoRecharge==null) ? 
            false:items[0].paymentSettings.autoRecharge;
          this.isUserProfileSavedInDatabase=true;
      }
      else
      {
          this.isUserProfileSavedInDatabase=false;
      }
  },
  methods: 
  {
    async savePaymentSettings() 
    {
        try 
        {
            const currentUserInfo=await this.currentUserInfo();
            const userId=currentUserInfo.id;
            if(userId == null)
            {
                this.$vs.notify({title: 'Error',text: 'There was an error saving your payment settings', iconPack: 'feather', 
                    icon: 'icon-alert-circle', color: 'danger'});
                return;   
            }
            console.log(`userId: ${userId}`);
            
            //#region save user profile in dynamodb
            if(this.isUserProfileSavedInDatabase==false)
            {
              const createUserProfileInput={id:userId, 
                paymentSettings:{autoRecharge: this.paymentSettings.autoRecharge,}};
              await API.graphql(graphqlOperation(createUserProfile,{input: createUserProfileInput}));
            }
            else
            {
                const updateUserProfileInput={id:userId,                 
                  paymentSettings:{autoRecharge: this.paymentSettings.autoRecharge,}};
                await API.graphql(graphqlOperation(updateUserProfile, {input: updateUserProfileInput}));
            }                
            //#endregion save user profile in dynamodb

            this.$vs.notify({title: 'Success', text: 'Payment settings have been saved successfully!', iconPack: 'feather',
                icon: 'icon-check',color: 'success'}); 
        } 
        catch (error) 
        {
            console.log(error);
            this.$vs.notify({title: 'Error',text: error.message, iconPack: 'feather', icon: 'icon-alert-circle', 
                color: 'danger'});
        };
    },
  }
}
</script>
