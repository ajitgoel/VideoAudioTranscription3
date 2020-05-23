<template>
  <!-- <form-wizard ref="wizard" color="rgba(var(--vs-primary), 1)" errorColor="rgba(var(--vs-danger), 1)" :title="null" :subtitle="null" 
     finishButtonText="Submit" @on-change='onChange'>
    <tab-content title="Choose a plan" class="mb-5" step-size="xs" icon="feather icon-home" :before-change="validateChoosePlan">  
      <form data-vv-scope="choosePlan">
        <div class="vx-row">
          <div class="vx-col sm:w-1/2 w-full mb-2">
            <ejs-slider id="noOfHoursSlider" name="noOfHoursSlider" ref="noOfHoursSlider" value=1 min=0 max=100 :ticks='ticks' 
              v-model="noOfHours"/>
          </div>
          <div class="vx-col sm:w-1/2 w-full mb-2">
            <ejs-numerictextbox id="noOfHours" name="noOfHours" ref="noOfHours" v-model="noOfHours" format='n' value="1" min="1" max="100" 
              strictMode="true" placeholder="Number of hours" floatLabelType="Always" width="25%"/> 
          </div>
        </div>
      </form>
    </tab-content>

    <tab-content title="Billing/Payment Information" class="mb-5" step-size="xs" icon="feather icon-credit-card" 
      :before-change="validateBillingInformation">
      <form data-vv-scope="billingInformation"> -->
        <div class="vx-row">
          <div class="vx-col lg:w-2/3 w-full">
            <!-- <ejs-slider id="noOfHoursSlider" name="noOfHoursSlider" ref="noOfHoursSlider" value=1 min=0 max=100 :ticks='ticks' 
              v-model="noOfHours"/> -->
              <vx-card title="Select no of hours to prepay">
                  <vs-slider :min="50" @change="noOfHoursSelectedChanged" v-model="value1"/>
                  <div :style="{'width':widthx+'px','height':heightx+'px'}" class="cuadrox">
                    {{value1}}
                  </div>                              
                  <vs-input-number id="noOfHours" name="noOfHours" ref="noOfHours" v-model="noOfHours" min="1" max="100"/>
                  <!--  <ejs-numerictextbox id="noOfHours" name="noOfHours" ref="noOfHours" v-model="noOfHours" format='n' value="1" min="1" 
                  max="100" strictMode="true" placeholder="Number of hours" floatLabelType="Always" width="25%"/>  -->                           
                  <vs-table :data="pricing">
                    <template slot="thead">                    
                      <vs-th>Pricing per hour</vs-th>
                      <vs-th>No of hours</vs-th>
                    </template>
                    <template slot-scope="{data}">
                      <vs-tr :key="indextr" v-for="(tr, indextr) in data">
                        <vs-td :data="data[indextr].priceperhour">
                          {{ data[indextr].priceperhour }}
                        </vs-td>
                        <vs-td :data="data[indextr].hourrange">
                          {{ data[indextr].hourrange }}
                        </vs-td>
                      </vs-tr>
                    </template>
                  </vs-table>               
              </vx-card>
              
            <vs-divider />

            <vx-card title="Billing details">
              <vs-input class="w-3/4 mb-base" name="fullname" ref="fullname" icon-no-border icon="icon icon-lock" 
                  icon-pack="feather" label-placeholder="Full name(Individual or company)" v-model="general.fullName"></vs-input>
              <span class="text-danger text-sm">{{ errors.first('fullname') }}</span>  
              
              <vs-input class="w-full my-base"  name="billingaddress" ref="billingaddress" icon-no-border icon="icon icon-lock" 
                  icon-pack="feather" label-placeholder="Full Billing address" v-model="general.billingAddress"></vs-input>
              <span class="text-danger text-sm">{{ errors.first('billingaddress') }}</span>
              
              <vs-input class="w-1/4 my-base"  label-placeholder="Country" name="country" ref="country" icon-no-border 
                  icon="icon icon-lock" icon-pack="feather" v-model="general.country"></vs-input>
              <span class="text-danger text-sm">{{ errors.first('country') }}</span>
              
              <!-- <vs-input class="w-1/2 my-base"  label-placeholder="VAT number(if applicable)" name="vatnumber" ref="vatnumber" 
                  icon-no-border icon="icon icon-lock" icon-pack="feather" v-model="general.vatNumber"></vs-input>
              <span class="text-danger text-sm">{{ errors.first('vatnumber') }}</span> -->
            </vx-card>

            <vs-divider />
            <vx-card title="Payment details">
              <div ref="card"></div>
            </vx-card>

            <vs-button class="float-right mt-6" @click="PlaceOrder" :disabled="!validateForm">Place your order</vs-button> 
          </div>

          <div class="vx-col lg:w-1/3 w-full">
            <vx-card title="Order summary">
              <div class="flex justify-between mb-2">
                  <span>No of hours</span>
                  <span class="font-semibold">1</span>
              </div>
              <div class="flex justify-between mb-2">
                  <span>Price per hour</span>
                  <span class="text-success">$10</span>
              </div>
              <div class="flex justify-between mb-2">
                  <span>Auto recharge</span>
                  <span class="text-success">Yes</span>
              </div>
              <vs-divider />
              <div class="flex justify-between">
                  <span>Total</span>
                  <span class="font-semibold">$10</span>
              </div>
            </vx-card>
          </div>    
        </div>
      <!-- </form>
    </tab-content>

    <tab-content title="Payment Confirmation" class="mb-5" step-size="xs" icon="feather icon-image" 
      :before-change="validatePaymentConfirmation">
      <form data-vv-scope="paymentConfirmation">
        <div class="vx-row">
          <div class="vx-col md:w-1/2 w-full">
            <vs-input label="Event Name" v-model="eventName" class="w-full mt-5" name="event_name" v-validate="'required|alpha_spaces'" />
            <span class="text-danger"></span>
          </div>
          <div class="vx-col md:w-1/2 w-full">
            <vs-select v-model="city" class="w-full select-large mt-5" label="Event Location">
              <vs-select-item :key="index" :value="item.value" :text="item.text" v-for="(item,index) in cityOptions" class="w-full" />
            </vs-select>
          </div>
          <div class="vx-col md:w-1/2 w-full mt-5">
            <vs-select v-model="status" class="w-full select-large" label="Event Status">
              <vs-select-item :key="index" :value="item.value" :text="item.text" v-for="(item,index) in statusOptions" class="w-full" />
            </vs-select>
          </div>
          <div class="vx-col md:w-1/2 w-full md:mt-8">
            <div class="demo-alignment">
              <span>Requirements:</span>
              <div class="flex">
                <vs-checkbox>Staffing</vs-checkbox>
                <vs-checkbox>Catering</vs-checkbox>
              </div>
            </div>
          </div>
        </div>
      </form>
    </tab-content>
  </form-wizard> -->
</template>
<style>
  .payment-form 
  {
    max-width: 400px;
    margin: 20px auto;
    border: 1px solid #ececec;
  }
  .payment-form h5 
  {
    margin: 0;
    padding: 10px;
    font-size: 1.2rem;
  }
  .card-element 
  {
    margin-top: 5px;
  }
  #card-number-element,
  #card-expiry-element,
  #card-cvc-element 
  {
    background: white;
    padding: 5px;
    border: 1px solid #ececec;
    height: 30px;
  }
  .place-order-button-block 
  {
    margin: 10px 0;
  }
#noOfHoursSlider .e-handle 
{
  height: 25px;
  width: 24px;
  background-size: 24px;
}
#noOfHoursSlider .e-handle 
{
  background-image: url('https://ej2.syncfusion.com/demos/src/slider/images/thumb.png');
  background-repeat: no-repeat;
  background-color: transparent;
  border: 0;
}
#noOfHoursSlider .e-tab-handle::after 
{
  background-color: transparent;
}
</style>
<script>

import { FormWizard, TabContent } from 'vue-form-wizard';
import 'vue-form-wizard/dist/vue-form-wizard.min.css';
// For custom error message
import { Validator } from 'vee-validate';

let stripe = Stripe(process.env.VUE_APP_STRIPE_PUBLISHABLE_KEY);
let elements = stripe.elements();
let card = undefined;
let style = {
  base: {border: '1px solid #D8D8D8', borderRadius: '4px',color: "#000",},
  invalid: {
    // All of the error styles go inside of here.
  }
};

const dict = {
  custom: {
    first_name: {
      required: 'First name is required',
      alpha: "First name may only contain alphabetic characters"
    },
    last_name: {
      required: 'Last name is required',
      alpha: "Last name may only contain alphabetic characters"
    },
    email: {
      required: 'Email is required',
      email: "Please enter valid email"
    },
    job_title: {
      required: 'Job title name is required',
      alpha: "Job title may only contain alphabetic characters"
    },
    proposal_title: {
      required: 'Proposal title name is required',
      alpha: "Proposal title may only contain alphabetic characters"
    },
    event_name: {
      required: 'Event name is required',
      alpha: "Event name may only contain alphabetic characters"
    },
  }
};

// register custom messages
Validator.localize('en', dict);

export default {
  data() {
    return {
      pricing: [
        {
          "id": 1,
          "priceperhour": "10$/hour",
          "hourrange": "0 to 24",
        },
        {
          "id": 2,
          "priceperhour": "9$/hour",
          "hourrange": "25 to 49 hours",
        },
        {
          "id": 3,
          "priceperhour": "8$/hour",
          "hourrange": "50 to 100 hours",
        },
      ],
        stripe: null,
        cardNumberElement: null,
        cardExpiryElement: null,
        cardCVCElement: null,
        stripeValidationError: "",
        amount:25,
        
        value1:55,widthx:55,heightx:55,
        ticks: { placement: 'After',smallStep: 10, largeStep: 20, showSmallTicks: true },
        noOfHours:1,
        general:{email: "", fullName: "", billingAddress: "", country: "", vatNumber: ""},      
        firstName: "",
        lastName: "",
        email: "",
        city: "new-york",
        proposalTitle: "",
        jobTitle: "",
        textarea: "",
        eventName: "",
        eventLocation: "san-francisco",
        status: "plannning",
        cityOptions: [
            { text: "New York", value: "new-york" },
            { text: "Chicago", value: "chicago" },
            { text: "San Francisco", value: "san-francisco" },
            { text: "Boston", value: "boston" },
        ],
        statusOptions: [
            { text: "Plannning", value: "plannning" },
            { text: "In Progress", value: "in progress" },
            { text: "Finished", value: "finished" },
        ],
        LocationOptions: [
            { text: "New York", value: "new-york" },
            { text: "Chicago", value: "chicago" },
            { text: "San Francisco", value: "san-francisco" },
            { text: "Boston", value: "boston" },
        ],
    }
  },
  computed: 
  {
      validateForm() 
      {            
          return false;
      }
  },
  mounted() 
  {
    card = elements.create('card', style);
    card.mount(this.$refs.card);

    /* this.stripe = Stripe(process.env.VUE_APP_STRIPE_PUBLISHABLE_KEY);
    //#region createAndMountFormElements;
    var elements = this.stripe.elements();
    this.cardNumberElement = elements.create("cardNumber");
    this.cardNumberElement.mount("#card-number-element");
    this.cardExpiryElement = elements.create("cardExpiry");
    this.cardExpiryElement.mount("#card-expiry-element");
    this.cardCvcElement = elements.create("cardCvc");
    this.cardCvcElement.mount("#card-cvc-element");
    this.cardNumberElement.on("change", this.setValidationError);
    this.cardExpiryElement.on("change", this.setValidationError);
    this.cardCvcElement.on("change", this.setValidationError);
    //#endregion */
  },
  methods: 
  { 
    onChange(prevIndex, nextIndex)
    {
      console.log(`${prevIndex} : ${nextIndex}`);
      if(prevIndex===0 && nextIndex===1 || prevIndex===1 && nextIndex===2)
      {        
        this.$refs.wizard.NextButtonText="Confirm payment";
      }
    },
    async PlaceOrder() 
    {
      try
      {
        var result=await stripe.createToken(card);
        if (result.error) 
        {
          self.hasCardErrors = true;
          self.$forceUpdate(); // Forcing the DOM to update so the Stripe Element can update.
          return;
        }
      }
      catch(error)
      {
        self.hasCardErrors = true;
        self.$forceUpdate(); // Forcing the DOM to update so the Stripe Element can update.
        return;
      };
    },
    /* setValidationError(event) 
    {
      this.stripeValidationError = event.error ? event.error.message : "";
    },
    async placeOrder() 
    {
      try
      {
        let result= await this.stripe.createToken(this.cardNumberElement);
        var stripeObject = {amount: this.amount,source: result.token};
        console.log('stripeObject:', JSON.stringify(stripeObject));
        //#region saveDataToFireStore(stripeObject)
        const db = firebase.firestore();
        const chargesRef = db.collection("charges");
        const pushId = chargesRef.doc().id;
        db.collection("charges").doc(pushId).set(stripeObject);
        chargesRef.doc(pushId).onSnapshot(snapShot => 
        {
          const charge = snapShot.data();
          if (charge.error) 
          {
              alert(charge.error);
              chargesRef.doc(pushId).delete();
              return;
          }
          if (charge.status && charge.status == "succeeded") 
          {
              alert(charge.status);
          }
        }); 
        //#endregion
      }
      catch(error)
      {
        this.stripeValidationError = result.error.message;
      }
    }, */
    cambio(value){
      this.widthx = value
      this.heightx = value
    },
    validateChoosePlan() {
      return new Promise((resolve, reject) => {
        this.$validator.validateAll('choosePlan').then(result => {
          if (result) {
            resolve(true)
          } else {
            reject("correct all values");
          }
        })
      })
    },
    validateBillingInformation() {
      return new Promise((resolve, reject) => {
        this.$validator.validateAll("billingInformation").then(result => {
          if (result) {
            resolve(true)
          } else {
            reject("correct all values");
          }
        })
      })
    },
    validatePaymentConfirmation() {
      return new Promise((resolve, reject) => {
        this.$validator.validateAll("paymentConfirmation").then(result => {
          if (result) {
            alert("Form submitted!");
            resolve(true)
          } else {
            reject("correct all values");
          }
        })
      })
    }
  },
  components: {
    FormWizard,
    TabContent
  }
}
</script>