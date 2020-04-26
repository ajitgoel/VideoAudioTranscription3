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
                            <div class="p-8">
                                <div class="vx-card__title mb-8">
                                    <h4 class="mb-4">Recover your password</h4>
                                    <p>Please enter your email address and we'll send you instructions on how to reset your password.</p>
                                </div>
                                <vs-input v-validate="'required|email'" data-vv-validate-on="blur" name="email" type="email" 
                                 label-placeholder="Email" placeholder="Email" v-model="email" class="w-full mb-8" />
                                <vs-button type="border" to="/pages/login" class="px-4 w-full md:w-auto">Back To Login</vs-button>
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
        return {email: ''};
    },
    computed: 
    {
        validateForm() 
        {
            return !this.errors.any() && this.email != '';
        }
    },
    methods: 
    {
        async forgotPassword() 
        {
            if (!this.validateForm) // If form is not validated or user is already login return            
            {
                return;
            }
            try
            {
                var result=await Auth.forgotPassword(this.email);       
                this.$router.push('/').catch(() => {});  
                this.$vs.notify({title: 'Forgot password', text: 'Please check your email for instructions on how to reset your password!', 
                    iconPack: 'feather', icon: 'icon-check',color: 'success'}); 
            }
            catch(error)
            {
                console.log(error);
                this.$vs.notify({title: 'Error',text: error.message, iconPack: 'feather', icon: 'icon-alert-circle', color: 'danger'});
            };
        },
    }
}
</script>
