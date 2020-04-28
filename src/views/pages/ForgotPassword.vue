<template>
  <div class="h-screen flex w-full bg-img">
    <div class="vx-col w-4/5 sm:w-4/5 md:w-3/5 lg:w-3/4 xl:w-3/5 mx-auto self-center">
      <vx-card>
        <div slot="no-body" class="full-page-bg-color">
          <div class="vx-row">
            <div class="vx-col hidden sm:hidden md:hidden lg:block lg:w-1/2 mx-auto self-center">
              <img src="@/assets/images/pages/forgot-password.png" alt="login" class="mx-auto">
            </div>
            <div class="vx-col sm:w-full md:w-full lg:w-1/2 mx-auto self-center d-theme-dark-bg">
                <div class="p-8 register-tabs-container" v-if="isSignUpConfirmed">
                    <div class="vx-card__title mb-8">
                        <h4 class="mb-4">Reset password</h4>
                        <p>Enter the confirmation code to reset your password.</p>
                    </div>
                    <!--Confirm Signup: Start-->
                    <vs-input v-validate="'required|digits:6'" data-vv-validate-on="blur" label-placeholder="Confirmation code" 
                        name="confirmationCode" v-model="confirmationCode" scope="ConfirmSignup" 
                        placeholder="Confirmation code" class="w-full mb-8" icon="icon icon-code" 
			            icon-pack="feather" icon-no-border/>
                    <span class="text-danger text-sm">{{ errors.first('confirmationCode') }}</span>

                    <vs-input ref="password" type="password" data-vv-validate-on="blur" v-validate="'required|min:6|max:10'"
                        name="password" label-placeholder="New password" placeholder="New password" v-model="password" 
                        scope="ConfirmSignup" class="w-full mb-8"  icon="icon icon-lock" icon-pack="feather" icon-no-border/>
                    <span class="text-danger text-sm">{{ errors.first('password') }}</span>                    
		    
                    <vs-button type="border" to="/login" class="px-4 w-full md:w-auto">Back To Login</vs-button>
                    <vs-button class="float-right px-4 w-full md:w-auto mt-3 mb-8 md:mt-0 md:mb-0" 
                    @click="confirmSignUp" :disabled="!validateConfirmationCode">Reset Password</vs-button>
                    <!--Confirm Signup: End-->                 
                </div> 
                <div class="p-8" v-else>
                    <div class="vx-card__title mb-8">
                    <h4 class="mb-4">Recover your password</h4>
                    <p>Please enter your email address and we'll send you confirmation code to reset your password.</p>
                    </div>
                    <vs-input v-validate="'required|email'" data-vv-validate-on="blur" name="email" type="email" 
                        label-placeholder="Email" placeholder="Email" v-model="email" scope="RecoverPassword" class="w-full mb-8" 
		                icon-no-border icon="icon icon-user" icon-pack="feather" />
                    <span class="text-danger text-sm">{{ errors.first('email') }}</span> 

                    <vs-button type="border" to="/login" class="px-4 w-full md:w-auto">Back To Login</vs-button>
                    <vs-button class="float-right px-4 w-full md:w-auto mt-3 mb-8 md:mt-0 md:mb-0" 
                    @click="forgotPassword" :disabled="!validateForm">Recover Password</vs-button>
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
    data() 
    {
        return {email: '', password: '', isSignUpConfirmed:false, confirmationCode:''};
    },
    computed: 
    {
        validateConfirmationCode() 
        {
            return !this.errors.any('ConfirmSignup') && this.confirmationCode != '' && this.password != '';
        },
        validateForm() 
        {
            return !this.errors.any('RecoverPassword') && this.email != '';
        }
    },
    methods: 
    {
        async confirmSignUp() 
        {
            if (!this.validateConfirmationCode) // If form is not validated          
            {
                return;
            }
            try
            {
                await Auth.forgotPasswordSubmit(this.email, this.confirmationCode, this.password);
                this.$router.push('/').catch(() => {});
                this.$vs.notify({title: 'Reset password', text: 'Reset password was successfully!', iconPack: 'feather',
                icon: 'icon-check',color: 'success'}); 
            }
            catch(error)
            {
                console.log(error);
                this.$vs.notify({title: 'Error',text: error.message, iconPack: 'feather', icon: 'icon-alert-circle', color: 'danger'});
            };
        },
        async forgotPassword() 
        {
            if (!this.validateForm) // If form is not validated or user is already login return            
            {
                return;
            }
            try
            {
                var result=await Auth.forgotPassword(this.email);       
                this.isSignUpConfirmed=true; 
                this.$vs.notify({title: 'Forgot password', text: 'Please check your email to confirm your account!', 
                    iconPack: 'feather', icon: 'icon-check',color: 'success'}); 
            }
            catch(error)
            {
                console.log(error);
                this.isSignUpConfirmed=false; 
                this.$vs.notify({title: 'Error',text: error.message, iconPack: 'feather', icon: 'icon-alert-circle', color: 'danger'});
            };
        },
    }
  }
</script>
<style lang="scss">
.register-tabs-container {
  min-height: 400px;
  .con-tab {
    padding-bottom: 23px;
  }
}
</style>