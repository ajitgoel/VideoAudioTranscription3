<template>
  <div class="h-screen flex w-full bg-img vx-row no-gutter items-center justify-center">
    <div class="vx-col sm:w-1/2 md:w-1/2 lg:w-3/4 xl:w-3/5 sm:m-0 m-4">
      <vx-card>
        <div slot="no-body" class="full-page-bg-color">
          <div class="vx-row no-gutter">
            <div class="vx-col hidden sm:hidden md:hidden lg:block lg:w-1/2 mx-auto self-center">
                <img src="@/assets/images/pages/register.jpg" alt="register" class="mx-auto">
            </div>
            <div class="vx-col sm:w-full md:w-full lg:w-1/2 mx-auto self-center d-theme-dark-bg">
              <div class="px-8 pt-8 register-tabs-container" v-if="showConfirmSignupScreen">
                <div class="vx-card__title mb-4">
                    <h4 class="mb-4">Confirm Signup</h4>
                    <p>Enter the confirmation code to confirm signup.</p>
                </div>
                <!--Confirm Signup: Start-->
                <vs-input v-validate="'required|email'" data-vv-validate-on="blur" name="email" ref="email2" type="email" 
                  label-placeholder="Email" placeholder="Email" v-model="email" scope="ConfirmSignup" class="w-full mt-6" 
                  icon-no-border icon="icon icon-user" icon-pack="feather" />
                <span class="text-danger text-sm">{{ errors.first('email') }}</span>
                
                <vs-input v-validate="'required|length:6|numeric'" data-vv-validate-on="blur" name="confirmationCode" 
                  placeholder="Confirmation code" class="w-full mt-6" label-placeholder="Confirmation code" v-model="confirmationCode" 
                  scope="ConfirmSignup" icon-no-border icon="icon icon-code" icon-pack="feather" />
                <span class="text-danger text-sm">{{ errors.first('confirmationCode') }}</span>

                <vs-button type="border" @click='showConfirmSignupScreen=false' class="mt-6">Register</vs-button>
                <vs-button class="float-right mt-6" @click="confirmSignUp" :disabled="!validateConfirmationCode">
                  Confirm email
                </vs-button>
                <!--Confirm Signup: End-->
              </div>
              <div class="px-8 pt-8 register-tabs-container" v-else>
                <div class="vx-card__title mb-4">
                    <h4 class="mb-4">Create Account</h4>
                    <p>Fill the below form to create a new account.</p>
                </div>
                <!--Register: Start-->
                <div class="clearfix">
                  <!-- <vs-input v-validate="'required|alpha_dash|min:3'" data-vv-validate-on="blur" label-placeholder="Name" 
                  name="displayName" placeholder="Name" v-model="displayName" scope="CreateAccount" class="w-full" />
                  <span class="text-danger text-sm">{{ errors.first('displayName') }}</span> -->
                  <vs-input v-validate="'required|email'" data-vv-validate-on="blur" name="email" type="email" ref="email" 
                    label-placeholder="Email" placeholder="Email" v-model="email" scope="CreateAccount" class="w-full mt-6" 
                    icon-no-border icon="icon icon-user" icon-pack="feather" />
                  <span class="text-danger text-sm">{{ errors.first('email') }}</span>
                  <vs-input ref="password" type="password" data-vv-validate-on="blur" v-validate="'required|min:6|max:10'"
                    name="password" label-placeholder="Password" placeholder="Password" v-model="password" scope="CreateAccount" 
                    class="w-full mt-6" icon="icon icon-lock" icon-pack="feather" icon-no-border/>
                  <span class="text-danger text-sm">{{ errors.first('password') }}</span>
                  <vs-input type="password" v-validate="'min:6|max:10|confirmed:password'" data-vv-validate-on="blur"
                    data-vv-as="password" name="confirm_password" label-placeholder="Confirm Password" placeholder="Confirm Password"
                    v-model="confirm_password" scope="CreateAccount" class="w-full mt-6"  icon="icon icon-lock" icon-pack="feather" 
                    icon-no-border/>
                  <span class="text-danger text-sm">{{ errors.first('confirm_password') }}</span>

                  <div class="flex flex-wrap justify-between my-5">
                    <a href='#' @click='showConfirmSignupScreen=true'>Enter Verification Code</a>
                  </div>                    
                  <vs-button type="border" to="/login" class="mt-6">Login</vs-button>
                  <vs-button class="float-right mt-6" @click="registerUser" :disabled="!validateForm">Register</vs-button>

                   <div class="flex flex-wrap justify-left my-5">
                    By creating an account, you agree to VideoAudioTranscription's <div class="mr-2"><a href='/terms-conditions' target='_blank' rel="nofollow">Conditions of Use</a></div> and <div class="ml-2"><a href='/privacy-policy' target='_blank' rel="nofollow">Privacy Policy</a></div>.
                  </div> 

                </div>
                <!--Register: End-->
              </div>
            </div>
          </div>
        </div>
      </vx-card>
    </div>
  </div>
</template>
<script>
import { Auth } from 'aws-amplify';
export default 
{  
  name: "Register",
  data() 
  {
    return {email: '', password: '', confirm_password: '', showConfirmSignupScreen:false, confirmationCode:''};
  },
  computed: 
  {
    validateConfirmationCode() 
    {
      return !this.errors.any('ConfirmSignup') && this.confirmationCode != '';
    },
    validateForm() 
    {
      return !this.errors.any('CreateAccount') && this.email != '' && this.password != '' && this.confirm_password != '';
    }
  },
  mounted() 
  {       
    this.$nextTick(function()
    {
      this.$refs.email.$el.querySelector('input').focus();
    });        
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
      if (!this.validateConfirmationCode || !this.checkLogin()) // If form is not validated or user is already login return            
      {
        return;
      }
      try
      {
        this.$vs.loading();
        const result=await Auth.confirmSignUp(this.email, this.confirmationCode);
        if(result === 'SUCCESS')
        {
          console.log(`confirmSignUp result: ${JSON.stringify(result)}`);
          this.$vs.notify({title: 'Account signup', text: 'Account confirmed successfully. Please login to continue!', 
          iconPack: 'feather', icon: 'icon-check',color: 'success'});                    
          this.$router.push('/login').catch(() => {});  
        }
        else
        {
          this.$vs.notify({title: 'Error',text: 'There was an error confirming your account', iconPack: 'feather', 
            icon: 'icon-alert-circle', color: 'danger'});
        }
      }
      catch(error)
      {
        console.log(`confirmSignUp error: ${JSON.stringify(error)}`);
        this.$vs.notify({title: 'Error',text: error.message, iconPack: 'feather', icon: 'icon-alert-circle', color: 'danger'});
      }
      finally
      {        
        this.$vs.loading.close();
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
        this.$vs.loading();
        const params = {username: this.email, password: this.password, attributes: {email: this.email}};
        const iSignUpResult=await Auth.signUp(params);
        console.log(`signUp iSignUpResult: ${JSON.stringify(this.iSignUpResult)}`);
        this.showConfirmSignupScreen=true;
        this.$nextTick(function()
        {
          this.$refs.email2.$el.querySelector("input").focus();
        });        
        this.$vs.notify({title: 'Register user', text: 'Please check your email to confirm your account!', iconPack: 'feather',
          icon: 'icon-check',color: 'success'}); 
      }
      catch(error)
      {
        this.showConfirmSignupScreen=false;
        console.log(`signUp error: ${JSON.stringify(error)}`);
        this.$vs.notify({title: 'Error',text: error.message, iconPack: 'feather', icon: 'icon-alert-circle', color: 'danger'});
      }
      finally
      {        
        this.$vs.loading.close();
      };
    },
  }
}
</script>
<style lang="scss">
.register-tabs-container {
  min-height: 517px;
  .con-tab {
    padding-bottom: 23px;
  }
}
</style>