<template>
    <vx-card no-shadow>
      <h6 class="mb-4">Delete account</h6>
      <p>Are you sure you want do delete your Video Audio Transcription account? <br/>
        After deleting your account,your personal details, credit card details and all your files will be removed from our service.</p>
      <div class="flex flex-wrap items-center justify-end mt-base">
        <vs-button @click="activePrompt=true" class="ml-auto mt-2">Delete account</vs-button>
      </div>            
      <vs-prompt title="Are you sure?" @cancel="clearValue" @accept="deleteAccount" @close="close" 
        :is-valid="isAlertValid" :active.sync="activePrompt">
        <div class="con-exemple-prompt">
          Type <b>DELETE</b> to confirm.
          <vs-input placeholder="" v-model="deleteText" class="mt-4 mb-2 w-full" />
        </div>
      </vs-prompt>
    </vx-card>
</template>
<script>
import { Auth } from 'aws-amplify';
import { deleteUserProfile} from '@/graphql/mutations';
//import {listUserProfilesForGeneral} from '@/graphql/customQueries';
import API, {graphqlOperation} from '@aws-amplify/api';

export default {
  data() {
    return {
      /* isUserProfileSavedInDatabase: false,
      userProfileChangePassword:{oldPassword: "", newPassword: "", confirmPassword: ""},      
      general:{email: "", fullName: "", billingAddress: "", country: "", vatNumber: ""},
      notification:{transcriptsCompleted: false, transcriptsError: false,}, */
      activePrompt:false,
      deleteText:'',
    }
  },
  computed: { 
    activeUserInfo() {
      return this.$store.state.AppActiveUser
    },
    isAlertValid(){
      return (this.deleteText.length > 0 && this.deleteText.toUpperCase() === 'DELETE');
    }   
  },
  async created() 
  {
      /* const userId=this.userIdFromLocalStorage();
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
      } */
  },
  methods: 
  {
    close(){      
    },
    clearValue() {
      this.deleteText = "";
    },
    async deleteAccount() 
    {
      try 
      {
        const userId=this.userIdFromLocalStorage();
        if(userId == null)
        {
          this.$vs.notify({title: 'Error',text: 'There was an error deleting your profile', iconPack: 'feather', 
            icon: 'icon-alert-circle', color: 'danger'});
          return;   
        }
        console.log(`userId: ${userId}`);
        const deleteUserProfileInput={userId:userId};
        console.log(`deleteUserProfileInput: ${JSON.stringify(deleteUserProfileInput)}`);
        await API.graphql(graphqlOperation(deleteUserProfile,{input: deleteUserProfileInput}));
        //ToDo: trigger user deletion from aws cognito through a lambda function, when a user is deleted in 
        //user profiles table.
        
        const result=await Auth.signOut();
        this.$router.push('/login').catch(() => {});  
        this.$vs.notify({title: 'User profile deleted', 
          text: 'User profile has been deleted and you are successfully logged out!', iconPack: 'feather', icon: 'icon-check',
          color: 'success'});  
      } 
      catch (error) 
      {
        console.log(`error: ${JSON.stringify(error)}`);
        this.$vs.notify({title: 'Error',text: error.message, iconPack: 'feather', icon: 'icon-alert-circle', 
            color: 'danger'});
      };
    },

    /* async saveNotifications() 
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
    async changePassword() 
    {
      try 
      {
        const user=await Auth.currentAuthenticatedUser();                 
        const result=await Auth.changePassword(user, this.userProfileChangePassword.oldPassword, 
          this.userProfileChangePassword.newPassword);
        if(result === 'SUCCESS')
        {
          this.$vs.notify({title: 'Change password', text: 'Your password has been successfully changed!', 
          iconPack: 'feather', icon: 'icon-check',color: 'success'}); 
          this.userProfileChangePassword.oldPassword=""; 
          this.userProfileChangePassword.newPassword= ""; 
          this.userProfileChangePassword.confirmPassword="";
        }
        else
        {
          this.$vs.notify({title: 'Error',text: 'There was an error changing your password', iconPack: 'feather', 
            icon: 'icon-alert-circle', color: 'danger'});
        }
      } 
      catch (error) 
      {
        console.log(`error: ${JSON.stringify(error)}`);
        this.$vs.notify({title: 'Error',text: error.message, iconPack: 'feather', icon: 'icon-alert-circle', 
          color: 'danger'});
      };
    } */
  }
}
</script>