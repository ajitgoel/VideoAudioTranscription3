<template>
  <vx-card no-shadow>

    <!-- <vs-input class="w-full mb-base" label-placeholder="Old Password" type="password" v-model="oldPassword" />
    <vs-input class="w-full mb-base" label-placeholder="New Password" type="password" v-model="newPassword" />
    <vs-input class="w-full mb-base" label-placeholder="Confirm Password" type="password" v-model="confirmPassword" /> -->

     <vs-input data-vv-validate-on="blur" v-validate="'required|min:6|max:10'" type="password" 
      v-model="oldPassword" name="oldPassword" ref="oldPassword" 
      icon-no-border icon="icon icon-lock" icon-pack="feather" label-placeholder="Old password" class="w-full mb-base" />
    <span class="text-danger text-sm">{{ errors.first('oldPassword') }}</span>

    <vs-input data-vv-validate-on="blur" v-validate="'required|min:6|max:10'" type="password" 
      v-model="newPassword" name="newPassword" ref="newPassword" 
      icon-no-border icon="icon icon-lock" icon-pack="feather" label-placeholder="New password" class="w-full mb-base" />
    <span class="text-danger text-sm">{{ errors.first('newPassword') }}</span>

    <vs-input data-vv-validate-on="blur" v-validate="'required|min:6|max:10|confirmed:newPassword'" type="password" 
      v-model="confirmPassword" name="confirmPassword" 
      icon-no-border icon="icon icon-lock" icon-pack="feather" label-placeholder="Confirm password" 
      class="w-full mb-base" />
    <span class="text-danger text-sm">{{ errors.first('confirmPassword') }}</span>

    <div class="flex flex-wrap items-center justify-end">
      <vs-button class="ml-auto mt-2" @click="changePassword">Change password</vs-button>
    </div>
  </vx-card>
</template>

<script>
import { Auth } from 'aws-amplify';
export default {
  data() {
    return {oldPassword: "", newPassword: "", confirmPassword: "",}
  },
  computed: {    
  },
  methods: 
  {
    async changePassword() 
    {
      try 
      {
        const user=await Auth.currentAuthenticatedUser();
        console.log(`user: ${JSON.stringify(user)}`);
        //ToDo: check if old password is valid or not. 
        const result=await Auth.changePassword(user, this.oldPassword, this.newPassword);
        console.log(`result: ${JSON.stringify(result)}`);   
        this.$vs.notify({title: 'Change password', text: 'Your password has been successfully changed!', 
          iconPack: 'feather', icon: 'icon-check',color: 'success'}); 
        this.$router.push('/user-settings').catch(() => {});  
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
