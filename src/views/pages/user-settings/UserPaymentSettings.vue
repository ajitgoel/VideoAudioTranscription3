<template>
  <!--Payments:Start-->
  <vx-card no-shadow>
    <div class="mb-base">
      <h6 class="mb-4">Payments</h6>
      <div class="flex items-center mb-4">
        <vs-switch v-model="payments.autoRecharge" />
        <span class="ml-4">Auto Recharge</span>
      </div>
    </div>
    <div class="flex flex-wrap items-center justify-end mt-base">
      <vs-button class="ml-auto mt-2" @click="savePaymentSettings">Save payment settings</vs-button>
    </div>
  </vx-card>
  <!--Payments:End-->
</template>
<script>
import { Auth } from 'aws-amplify';
import { createUserProfile, updateUserProfile} from '@/graphql/mutations';
import {listUserProfilesForGeneral} from '@/graphql/customQueries';
import API, {graphqlOperation} from '@aws-amplify/api';

export default {
  data() {
    return {
      isUserProfileSavedInDatabase: false,
      payments:{autoRecharge: false}
    }
  },
  computed: { 
    activeUserInfo() {
      return this.$store.state.AppActiveUser
    },   
  },
  async created() 
  {
      const userId=this.userIdFromLocalStorage();
      const listUserProfilesFilter={userId:{eq:userId}};
      const result = await API.graphql(graphqlOperation(listUserProfilesForGeneral, {filter: listUserProfilesFilter}));
      console.log(`result: ${JSON.stringify(result)}`);
      const items=result.data.listUserProfiles.items;
      if(items.length>0)
      {
          this.id=items[0].id;
          this.general.fullName=items[0].fullName;
          this.general.billingAddress=items[0].billingAddress;
          this.general.country=items[0].country;
          this.general.vatNumber=items[0].vatNumber;
          this.notification.transcriptsCompleted=items[0].notificationTranscriptsCompleted==null?
            false:items[0].notificationTranscriptsCompleted;
          this.notification.transcriptsError=items[0].notificationTranscriptsError==null?
            false:items[0].notificationTranscriptsError;
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
            const userId=this.userIdFromLocalStorage();
            if(userId == null)
            {
                this.$vs.notify({title: 'Error',text: 'There was an error saving your profile', iconPack: 'feather', 
                    icon: 'icon-alert-circle', color: 'danger'});
                return;   
            }
            console.log(`userId: ${userId}`);
            
            //#region save user profile in dynamodb
            if(this.isUserProfileSavedInDatabase==false)
            {
              const createUserProfileInput={userId:userId, 
                notificationTranscriptsCompleted: this.notification.transcriptsCompleted, 
                notificationTranscriptsError: this.notification.transcriptsError, };
              await API.graphql(graphqlOperation(createUserProfile,{input: createUserProfileInput}));
            }
            else
            {
                const updateUserProfileInput={id:this.id, userId:userId,                 
                  notificationTranscriptsCompleted: this.notification.transcriptsCompleted, 
                  notificationTranscriptsError: this.notification.transcriptsError, };
                await API.graphql(graphqlOperation(updateUserProfile, {input: updateUserProfileInput}));
            }                
            //#endregion save user profile in dynamodb

            this.$vs.notify({title: 'Success', text: 'User profile have been saved successfully!', iconPack: 'feather',
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
