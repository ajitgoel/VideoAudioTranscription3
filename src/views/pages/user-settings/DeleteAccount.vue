<template>
    <vx-card no-shadow>
      <div class="demo-alignment">
        <vs-button @click="activePrompt2 = true" color="primary" type="border">Delete account</vs-button>
        <!-- <div class="op-block">
          Name: {{ valMultipe.value1 }} | Last Name: {{ valMultipe.value2 }}
        </div> -->
      </div>
      <vs-prompt title="Are you sure?" @cancel="clearValMultiple" @accept="acceptAlert" @close="close" :is-valid="validName" 
        :active.sync="activePrompt2">
        <div class="con-exemple-prompt">
          Type <b>DELETE</b> to confirm.
        <vs-input placeholder="" v-model="valMultipe.value1" class="mt-4 mb-2 w-full" />
        <!-- <vs-alert :vs-active="!validName" color="danger" vs-icon="new_releases" >
          Fields can not be empty please enter the data
        </vs-alert> -->
        </div>
      </vs-prompt>
    </vx-card>
</template>
/* <script>
import { Auth } from 'aws-amplify';
import { createUserProfile, updateUserProfile} from '@/graphql/mutations';
import {listUserProfilesForGeneral} from '@/graphql/customQueries';
import API, {graphqlOperation} from '@aws-amplify/api';

export default {
  data() {
    return {
      isUserProfileSavedInDatabase: false,
      userProfileChangePassword:{oldPassword: "", newPassword: "", confirmPassword: ""},      
      general:{email: "", fullName: "", billingAddress: "", country: "", vatNumber: ""},
      notification:{transcriptsCompleted: false, transcriptsError: false,},
      deleteText: '',
      value2: '',
      popupActive2: false,
      popupActive3: false
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
    async saveProfile() 
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
                const createUserProfileInput={userId:userId, fullName: this.general.fullName, 
                  billingAddress:this.general.billingAddress, country: this.general.country, vatNumber: this.general.vatNumber,};
                await API.graphql(graphqlOperation(createUserProfile,{input: createUserProfileInput}));
            }
            else
            {
                const updateUserProfileInput={id:this.id, userId:userId, fullName: this.general.fullName,
                  billingAddress:this.general.billingAddress, country: this.general.country, vatNumber: this.general.vatNumber,};
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

    async saveNotifications() 
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
    }
  }
}
</script> */
<script>
import { Auth } from 'aws-amplify';
import { createUserProfile, updateUserProfile} from '@/graphql/mutations';
import {listUserProfilesForGeneral} from '@/graphql/customQueries';
import API, {graphqlOperation} from '@aws-amplify/api';

export default {
  data(){
    return {
      activePrompt:false,
      activePrompt2:false,
      val:'',
      valMultipe:{
        value1:'',
        value2:''
      },
    }
  },
  computed:{
    validName(){
      return (this.valMultipe.value1.length > 0 && this.valMultipe.value2.length > 0)
    }
  },
  methods:{
    acceptAlert(){
      this.$vs.notify({
        color:'success',
        title:'Accept Selected',
        text:'Lorem ipsum dolor sit amet, consectetur'
      })
    },
    close(){
      this.$vs.notify({
        color:'danger',
        title:'Closed',
        text:'You close a dialog!'
      })
    },
    clearValMultiple() {
      this.valMultipe.value1 = "";
      this.valMultipe.value2 = "";
    }
  }
}
</script>