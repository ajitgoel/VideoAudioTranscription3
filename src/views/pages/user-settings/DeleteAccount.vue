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
import API, {graphqlOperation} from '@aws-amplify/api';

export default {
  data() {
    return {
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
        const currentUserInfoResult=await this.currentUserInfo();
        const userId=currentUserInfoResult.id;
        if(userId == null)
        {
          this.$vs.notify({title: 'Error',text: 'There was an error deleting your profile', iconPack: 'feather', 
            icon: 'icon-alert-circle', color: 'danger'});
          return;   
        }
        console.log(`userId: ${userId}`);
        const deleteUserProfileInput={id:userId};
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
  }
}
</script>