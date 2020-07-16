import Vue from 'vue';
import App from './App.vue';
import '@/mixins/generalMixins.js';
import Print from 'vue-print-nb'; 
Vue.use(Print);

import {UploaderPlugin, TextBoxPlugin} from '@syncfusion/ej2-vue-inputs';
import { ListViewPlugin } from '@syncfusion/ej2-vue-lists';
import { DropDownListPlugin } from '@syncfusion/ej2-vue-dropdowns';

Vue.use(DropDownListPlugin);
Vue.use(ListViewPlugin);
Vue.use(TextBoxPlugin);
Vue.use(UploaderPlugin); 

//#region Globally register components
import TermsConditions from '@/views/pages/TermsConditions.vue';
import PrivacyPolicy from '@/views/pages/PrivacyPolicy.vue';
Vue.component('TermsConditions', TermsConditions);
Vue.component('PrivacyPolicy', PrivacyPolicy);
//#endregion

import Vuesax from 'vuesax';
import 'material-icons/iconfont/material-icons.css'; //Material Icons
import 'vuesax/dist/vuesax.css'; // Vuesax
Vue.use(Vuesax);

// @ts-ignore
import axios from "./axios.js";
Vue.prototype.$http = axios;
import "./http/requests";
import '../themeConfig.js';
// @ts-ignore
import acl from './acl/acl';

import './globalComponents.js';
import './assets/scss/main.scss';
import '@/assets/css/main.css';
// @ts-ignore
import router from './router';

// @ts-ignore
import store from './store/store';
// @ts-ignore
import i18n from './i18n/i18n';
import './filters/filters';

import VueClipboard from 'vue-clipboard2';
Vue.use(VueClipboard);

// Tour
/* import VueTour from 'vue-tour';
Vue.use(VueTour);
require('vue-tour/dist/vue-tour.css'); */

import VeeValidate from 'vee-validate';
Vue.use(VeeValidate, {fieldsBagName: 'formFields'});

import Amplify,{Storage} from 'aws-amplify';
import '@aws-amplify/ui-vue';
// @ts-ignore
import aws_exports from './aws-exports';
Amplify.configure(aws_exports);
Vue.prototype.$Amplify = Amplify;

/* Analytics.autoTrack('session', {enable: true,provider: 'AWSPinpoint'});
Analytics.autoTrack('pageView', 
    {enable: true,eventName: 'pageView',type: 'SPA',provider: 'AWSPinpoint',
    getUrl: () => {return window.location.origin + window.location.pathname;}
}); */

// Google Maps
import * as VueGoogleMaps from 'vue2-google-maps';
Vue.use(VueGoogleMaps, {
    load: {
        // Add your API key here
        key: 'YOUR_API_KEY',
        libraries: 'places', // This is required if you use the Auto complete plug-in
    },
});

// Vuejs - Vue wrapper for hammerjs
import { VueHammer } from 'vue2-hammer';
Vue.use(VueHammer);

// PrismJS
import 'prismjs';
import 'prismjs/themes/prism-tomorrow.css';

// Feather font icon
require('./assets/css/iconfont.css');

// Vue select css
// Note: In latest version you have to add it separately
// import 'vue-select/dist/vue-select.css';

Vue.directive('focus', {
    // When the bound element is inserted into the DOM...
    inserted: function (el) {el.focus();}
  })

Vue.config.productionTip = false;
new Vue({router, store, i18n, acl, render: h => h(App)}).$mount('#app');