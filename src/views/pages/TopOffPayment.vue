<template>
  <form-wizard color="rgba(var(--vs-primary), 1)" errorColor="rgba(var(--vs-danger), 1)" :title="null" :subtitle="null" 
    finishButtonText="Submit">
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
      <form data-vv-scope="billingInformation">
      <div class="vx-row">
        <div class="vx-col lg:w-2/3 w-full">
          <vs-input class="w-3/4 mb-base" name="fullname" ref="fullname" icon-no-border icon="icon icon-lock" 
              icon-pack="feather" label-placeholder="Full name(Individual or company)" v-model="general.fullName"></vs-input>
          <span class="text-danger text-sm">{{ errors.first('fullname') }}</span>  
          
          <vs-input class="w-full my-base"  name="billingaddress" ref="billingaddress" icon-no-border icon="icon icon-lock" 
              icon-pack="feather" label-placeholder="Full Billing address" v-model="general.billingAddress"></vs-input>
          <span class="text-danger text-sm">{{ errors.first('billingaddress') }}</span>
          
          <vs-input class="w-1/4 my-base"  label-placeholder="Country" name="country" ref="country" icon-no-border 
              icon="icon icon-lock" icon-pack="feather" v-model="general.country"></vs-input>
          <span class="text-danger text-sm">{{ errors.first('country') }}</span>
          
          <vs-input class="w-1/2 my-base"  label-placeholder="VAT number(if applicable)" name="vatnumber" ref="vatnumber" 
              icon-no-border icon="icon icon-lock" icon-pack="feather" v-model="general.vatNumber"></vs-input>
          <span class="text-danger text-sm">{{ errors.first('vatnumber') }}</span>
        </div>

        <div class="vx-col lg:w-1/3 w-full">
          <vx-card title="Price Details">
            <div class="flex justify-between mb-2">
                <span>Price 3 Items</span>
                <span class="font-semibold">$699.30</span>
            </div>
            <div class="flex justify-between mb-2">
                <span>Delivery Charges</span>
                <span class="text-success">Free</span>
            </div>
            <vs-divider />
            <div class="flex justify-between">
                <span>Amount Payable</span>
                <span class="font-semibold">$699.30</span>
            </div>
          </vx-card>
        </div>    
      </div>
      </form>
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
  </form-wizard>
</template>
<style>
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
import '@syncfusion/ej2-base/styles/material.css';
import '@syncfusion/ej2-vue-buttons/styles/material.css';
import '@syncfusion/ej2-vue-popups/styles/material.css';
import '@syncfusion/ej2-vue-inputs/styles/material.css';

import { FormWizard, TabContent } from 'vue-form-wizard';
import 'vue-form-wizard/dist/vue-form-wizard.min.css';
// For custom error message
import { Validator } from 'vee-validate';
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
  methods: {
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