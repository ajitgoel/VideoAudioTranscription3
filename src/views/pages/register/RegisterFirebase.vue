<template>
  <div>
    <div class="clearfix" v-if="isSignUpConfirmed">
      <vs-input v-validate="'required|length:6|numeric'" data-vv-validate-on="blur" label-placeholder="Confirmation code" 
      placeholder="Confirmation code" v-model="confirmationCode" class="w-full mt-6" />
      <span class="text-danger text-sm">{{ errors.first('confirmationCode') }}</span>
      <vs-button class="float-right mt-6" @click="confirmSignUp" :disabled="!validateConfirmationCode">Confirm email</vs-button>
    </div>
    <div class="clearfix" v-else>
      <!-- <vs-input v-validate="'required|alpha_dash|min:3'" data-vv-validate-on="blur" label-placeholder="Name" name="displayName"
        placeholder="Name" v-model="displayName" class="w-full" />
      <span class="text-danger text-sm">{{ errors.first('displayName') }}</span> -->
      <vs-input v-validate="'required|email'" data-vv-validate-on="blur" name="email" type="email" label-placeholder="Email"
        placeholder="Email" v-model="email" class="w-full mt-6" />
      <span class="text-danger text-sm">{{ errors.first('email') }}</span>
      <vs-input ref="password" type="password" data-vv-validate-on="blur" v-validate="'required|min:6|max:10'"
        name="password" label-placeholder="Password" placeholder="Password" v-model="password" class="w-full mt-6" />
      <span class="text-danger text-sm">{{ errors.first('password') }}</span>
      <vs-input type="password" v-validate="'min:6|max:10|confirmed:password'" data-vv-validate-on="blur"
        data-vv-as="password" name="confirm_password" label-placeholder="Confirm Password" placeholder="Confirm Password"
        v-model="confirm_password" class="w-full mt-6" />
      <span class="text-danger text-sm">{{ errors.first('confirm_password') }}</span>
      <vs-checkbox v-model="isTermsConditionAccepted" class="mt-6">
        <a href="/pages/terms-conditions" target="_blank">I accept the terms & conditions.</a>
      </vs-checkbox>    
      <vs-checkbox v-model="isPrivacyPolicyAccepted" class="mt-6">
        <a href="/pages/privacy-policy" target="_blank">I accept the privacy policy.</a>
      </vs-checkbox> 
      <vs-button  type="border" to="/pages/login" class="mt-6">Login</vs-button>
      <vs-button class="float-right mt-6" @click="registerUser" :disabled="!validateForm">Register</vs-button>
    </div>
  </div>
</template>
<script>
import { Auth } from 'aws-amplify';
export default 
{
  data() 
  {
    return {email: '', password: '', confirm_password: '', isTermsConditionAccepted: false, isPrivacyPolicyAccepted:false, 
    isSignUpConfirmed:false, confirmationCode:''};
  },
  computed: 
  {
    validateConfirmationCode() 
    {
        return !this.errors.any() && this.confirmationCode != '';
    },
    validateForm() 
    {
        return !this.errors.any() && this.email != '' && this.password != '' && this.confirm_password != '' && 
          this.isTermsConditionAccepted === true && this.isPrivacyPolicyAccepted === true;
    }
  },
  methods: 
  {
    checkLogin() 
    {
      if(this.$store.state.auth.isUserLoggedIn()) // If user is already logged in notify          
      {
        // this.$vs.loading.close()// Close animation if passed as payload            
        this.$vs.notify({title: 'Login attempt', text: 'You are already logged in!', iconPack: 'feather',
          icon: 'icon-alert-circle',color: 'warning'});
        return false;
      }
      return true;
    },
    async confirmSignUp() 
    {
      if (!this.validateForm || !this.checkLogin()) // If form is not validated or user is already login return            
      {
        return;
      }
      try
      {
        const result=await Auth.confirmSignUp(this.email, this.confirmationCode);
        this.$router.push('/').catch(() => {});
        this.$vs.notify({title: 'Account created', text: 'Account created successfully!', iconPack: 'feather',
          icon: 'icon-check',color: 'success'}); 
      }
      catch(error)
      {
        console.log(error);
        this.$vs.notify({title: 'Error',text: error.message, iconPack: 'feather', icon: 'icon-alert-circle', color: 'danger'});
      };
    },
    async registerUser() 
    {
      if (!this.validateForm || !this.checkLogin()) // If form is not validated or user is already login return            
      {
        return;
      }
      try
      {
        const params = {username: this.email, password: this.password, attributes: {email: this.email}};
        const iSignUpResult=await Auth.signUp(params);
        //this.$router.push('/').catch(() => {});  
        this.isSignUpConfirmed=true;
        this.$vs.notify({title: 'Confirm account', text: 'Please check your email to confirm your account!', iconPack: 'feather',
          icon: 'icon-check',color: 'success'}); 
      }
      catch(error)
      {
        this.isSignUpConfirmed=false;
        console.log(error);
        this.$vs.notify({title: 'Error',text: error.message, iconPack: 'feather', icon: 'icon-alert-circle', color: 'danger'});
      };
    }
  }
}
</script>
