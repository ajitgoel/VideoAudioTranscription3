<template>
   <vx-card title="Top off payment" code-toggler>
      <div class="mt-100">
        <stripe-elements ref="paymentCard" :pk="publishableKey" :amount="amount" locale="en" @token="tokenCreated"
          @loading="loading = $event" style=""></stripe-elements>
        <button @click="submit">Pay ${{amount / 100}}</button>      
      </div> 
    </vx-card> 
</template> 
<script>
import { StripeElements } from 'vue-stripe-checkout';
export default 
{
  components: 
  {
    StripeElements
  },
  data() {
    return {loading: false, amount: 1000,token: null,charge: null, 
      publishableKey: process.env.VUE_APP_STRIPE_PUBLISHABLE_KEY, }
  },
  methods: 
  {
    submit () 
    {
      this.$refs.paymentCard.submit();
    },
    tokenCreated (token) 
    {      
      console.log('tokenCreated:', JSON.stringify(token));
      this.token = token;
      // for additional charge objects go to https://stripe.com/docs/api/charges/object
      this.charge = {source: token.id,
        amount: this.amount, // the amount you want to charge the customer in cents. $100 is 1000 (it is strongly recommended you use a product id and quantity and get calculate this on the backend to avoid people manipulating the cost)
        description: this.description // optional description that will show up on stripe when looking at payments
      }
      this.sendTokenToServer(this.charge);
    },
    sendTokenToServer (charge) {
      // Send to charge to your backend server to be processed
      // Documentation here: https://stripe.com/docs/api/charges/create
      console.log('sendTokenToServer:', JSON.stringify(charge));      
    }
  }
}
</script> 
<style>
/**The CSS shown here will not be introduced in the Quickstart guide, but shows
 * how you can use CSS to style your Element's container.
 */
.StripeElement {
  box-sizing: border-box;

  height: 40px;

  padding: 10px 12px;

  border: 1px solid transparent;
  border-radius: 4px;
  background-color: white;

  box-shadow: 0 1px 3px 0 #e6ebf1;
  -webkit-transition: box-shadow 150ms ease;
  transition: box-shadow 150ms ease;
}
.StripeElement--focus {
  box-shadow: 0 1px 3px 0 #cfd7df;
}
.StripeElement--invalid {
  border-color: #fa755a;
}
.StripeElement--webkit-autofill {
  background-color: #fefde5 !important;
}
</style>