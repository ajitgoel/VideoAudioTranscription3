<template>
  <div>
    <!--General:Start-->
    <vx-card no-shadow>
      <h6 class="mb-4">General settings</h6>
     <!--  <div class="flex flex-wrap items-center mb-base">
        <vs-avatar :src="activeUserInfo.photoURL" size="70px" class="mr-4 mb-4" />
        <div>
          <vs-button class="mr-4 sm:mb-0 mb-2">Upload photo</vs-button>
          <vs-button type="border" color="danger">Remove</vs-button>
          <p class="text-sm mt-2">Allowed JPG, GIF or PNG. Max size of 800kB</p>
        </div>
      </div> -->
      <vs-input class="w-full mb-base" name="email" ref="email" icon-no-border icon="icon icon-mail" icon-pack="feather" 
        label-placeholder="Email" v-model="general.email" disabled="true" ></vs-input>
      <span class="text-danger text-sm">{{ errors.first('email') }}</span>  
      
      <vs-input class="w-3/4 mb-base" name="fullname" ref="fullname" icon-no-border icon="icon icon-lock" 
        icon-pack="feather" label-placeholder="Full name(Individual or company)" v-model="general.fullName"></vs-input>
      <span class="text-danger text-sm">{{ errors.first('fullname') }}</span>  
      
      <vs-input class="w-full my-base"  name="billingaddress" ref="billingaddress" icon-no-border icon="icon icon-lock" 
        icon-pack="feather" label-placeholder="Full Billing address" v-model="general.billingAddress"></vs-input>
      <span class="text-danger text-sm">{{ errors.first('billingaddress') }}</span>
      <ejs-dropdownlist id='country' :dataSource='$options.countries' :fields='countriesFields' v-model="general.country" allowFiltering='true' placeholder='Select country' 
        width="50%"/>
      <vs-input class="w-1/2 my-base"  label-placeholder="VAT number(if applicable)" name="vatnumber" ref="vatnumber" 
        icon-no-border icon="icon icon-lock" icon-pack="feather" v-model="general.vatNumber"></vs-input>
      <span class="text-danger text-sm">{{ errors.first('vatnumber') }}</span>

      <!-- <vs-alert icon-pack="feather" icon="icon-info" class="h-full my-4" color="warning">
        <span>Your account is not verified. <a href="#" class="hover:underline">Resend Confirmation</a></span>
      </vs-alert> -->
      <div class="flex flex-wrap items-center justify-end">
        <vs-button class="ml-auto mt-2" @click="saveProfile">Save profile</vs-button>
      </div>
    </vx-card>
    <!--General:End-->
    
    <!--Change Password:Start-->
    <vx-card no-shadow>
      <h6 class="mb-4">Change password</h6>
      <vs-input data-vv-validate-on="blur" v-validate="'required|min:6|max:10'" type="password" 
        v-model="userProfileChangePassword.oldPassword" name="oldPassword" ref="oldPassword" 
        icon-no-border icon="icon icon-lock" icon-pack="feather" label-placeholder="Old password" 
        class="w-full mb-base" />
      <span class="text-danger text-sm">{{ errors.first('oldPassword') }}</span>
      <vs-input data-vv-validate-on="blur" v-validate="'required|min:6|max:10'" type="password" 
        v-model="userProfileChangePassword.newPassword" name="newPassword" ref="newPassword" 
        icon-no-border icon="icon icon-lock" icon-pack="feather" label-placeholder="New password" 
        class="w-full mb-base" />
      <span class="text-danger text-sm">{{ errors.first('newPassword') }}</span>
      <vs-input data-vv-validate-on="blur" v-validate="'required|min:6|max:10|confirmed:newPassword'" type="password" 
        v-model="userProfileChangePassword.confirmPassword" name="confirmPassword" 
        icon-no-border icon="icon icon-lock" icon-pack="feather" label-placeholder="Confirm password" 
        class="w-full mb-base" />
      <span class="text-danger text-sm">{{ errors.first('confirmPassword') }}</span>
      <div class="flex flex-wrap items-center justify-end">
        <vs-button class="ml-auto mt-2" @click="changePassword">Change password</vs-button>
      </div>
    </vx-card>
    <!--Change Password:End-->
   
  </div>
</template>
<script>
import { Auth } from 'aws-amplify';
import API, {graphqlOperation} from '@aws-amplify/api';
import {COUNTRIES} from '@/static.js';

export default {
  data() {
    return {      
      countriesFields : {text: 'text', value: 'value' },
      isUserProfileSavedInDatabase: false,
      general:{email: "", fullName: "", billingAddress: "", country: "", vatNumber: ""},
      userProfileChangePassword:{oldPassword: "", newPassword: "", confirmPassword: ""},      
      notification:{transcriptsCompleted: false, transcriptsError: false,}
    }
  },
  mounted() 
  {       
    this.$nextTick(function()
    {
      this.$refs.fullname.$el.querySelector('input').focus();
    });        
  },
  async created() 
  {      
    this.$options.countries = COUNTRIES;
    const currentUserInfoResult=await this.currentUserInfo();
    this.general.email=currentUserInfoResult.email;
    const items=await this.getuserprofile();
    if(items.length>0)
    {
        this.general.fullName=items[0].fullName;
        this.general.billingAddress=items[0].billingAddress;
        this.general.country=items[0].country;
        this.general.vatNumber=items[0].vatNumber;
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
        const currentUserInfoResult=await this.currentUserInfo();
        const userId=currentUserInfoResult.id;
        if(userId == null)
        {
            this.$vs.notify({title: 'Error',text: 'There was an error saving your profile', iconPack: 'feather', 
                icon: 'icon-alert-circle', color: 'danger'});
            return;   
        }
        console.log(`userId: ${userId}`);          
        //#region save user profile in dynamodb
        const userProfileInput={id:userId, fullName: this.general.fullName, 
              billingAddress:this.general.billingAddress, country: this.general.country, vatNumber: this.general.vatNumber,};
            
        if(this.isUserProfileSavedInDatabase==false)
        {
          const createUserProfile = `
            mutation CreateUserProfile(
              $input: CreateUserProfileInput!
              $condition: ModelUserProfileConditionInput
            ) {
              createUserProfile(input: $input, condition: $condition) {
                id
                fullName
                billingAddress
                country
                vatNumber
              }
            }
          `;
          await API.graphql(graphqlOperation(createUserProfile,{input: userProfileInput}));
        }
        else
        {
          const updateUserProfile = `
            mutation UpdateUserProfile(
              $input: UpdateUserProfileInput!
              $condition: ModelUserProfileConditionInput
            ) {
              updateUserProfile(input: $input, condition: $condition) {
                id
                fullName
                billingAddress
                country
                vatNumber
              }
            }
          `;
          await API.graphql(graphqlOperation(updateUserProfile, {input: userProfileInput}));
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
</script>
