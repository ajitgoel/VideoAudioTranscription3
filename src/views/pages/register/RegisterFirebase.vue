<template>
  <div class="clearfix">
    <vs-input v-validate="'required|alpha_dash|min:3'" data-vv-validate-on="blur" label-placeholder="Name" name="displayName"
      placeholder="Name" v-model="displayName" class="w-full" />
    <span class="text-danger text-sm">{{ errors.first('displayName') }}</span>
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
    <vs-checkbox v-model="isTermsConditionAccepted" class="mt-6">I accept the terms & conditions.</vs-checkbox>
    <vs-button  type="border" to="/pages/login" class="mt-6">Login</vs-button>
    <vs-button class="float-right mt-6" @click="registerUser" :disabled="!validateForm">Register</vs-button>
  </div>
</template>

<script>
import { Auth } from 'aws-amplify';
export default 
{
  data() 
  {
    return {displayName: '', email: '', password: '', confirm_password: '', isTermsConditionAccepted: false};
  },
  computed: 
  {
    validateForm() 
    {
        return !this.errors.any() && this.displayName != '' && this.email != '' && this.password != '' && this.confirm_password != '' && this.isTermsConditionAccepted === true;
    }
  },
  methods: 
  {
    checkLogin() 
    {
      if(this.$store.state.auth.isUserLoggedIn()) // If user is already logged in notify          
      {
        // this.$vs.loading.close()// Close animation if passed as payload            
        this.$vs.notify({title: 'Login Attempt', text: 'You are already logged in!', iconPack: 'feather',
          icon: 'icon-alert-circle',color: 'warning'});
        return false;
      }
      return true;
    },
    registerUser() 
    {
      if (!this.validateForm || !this.checkLogin()) // If form is not validated or user is already login return            
      {
        return;
      }
      /* const payload = {
        userDetails: {displayName: this.displayName, email: this.email, password: this.password, 
        confirmPassword: this.confirm_password}, notify: this.$vs.notify
      }; */
      //this.$store.dispatch('auth/registerUser', payload);
      const signUpParams = {username: this.email,password: this.password,attributes: {fullname: this.displayName}};
      Auth.signUp(signUpParams).
        then((data) => 
        {
          this.$vs.notify({title: 'Account Created', text: 'You are successfully registered!', iconPack: 'feather',
            icon: 'icon-check',color: 'success'});
        })
        .catch((error) => 
        {
          this.$vs.notify({title: 'Error',text: error.message, iconPack: 'feather', icon: 'icon-alert-circle', color: 'danger'});
        });
    }
  }
}
</script>
