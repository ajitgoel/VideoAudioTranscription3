import Vue from 'vue';
import App from './App.vue';
import '@/mixins/generalMixins.js';

import { GridPlugin, Page, Toolbar, Edit } from "@syncfusion/ej2-vue-grids";
Vue.use(GridPlugin);

//#region Globally register components
import TermsConditions from '@/views/pages/TermsConditions.vue';
import PrivacyPolicy from '@/views/pages/PrivacyPolicy.vue';
Vue.component('TermsConditions', TermsConditions);
Vue.component('PrivacyPolicy', PrivacyPolicy);
//#endregion

// Vuesax Component Framework
import Vuesax from 'vuesax';
import 'material-icons/iconfont/material-icons.css'; //Material Icons
import 'vuesax/dist/vuesax.css'; // Vuesax
Vue.use(Vuesax);

// axios
// @ts-ignore
import axios from "./axios.js";
Vue.prototype.$http = axios;

// API Calls
import "./http/requests";

// mock
import "./fake-db/index.js";

// Theme Configurations
import '../themeConfig.js';

// Firebase
import '@/firebase/firebaseConfig';

// Auth0 Plugin
//import AuthPlugin from "./plugins/auth";
//Vue.use(AuthPlugin);

// ACL
// @ts-ignore
import acl from './acl/acl';

// Globally Registered Components

import './globalComponents.js';

// Styles: SCSS
import './assets/scss/main.scss';

// Tailwind
import '@/assets/css/main.css';

// Vue Router
// @ts-ignore
import router from './router';

// Vuex Store
// @ts-ignore
import store from './store/store';

// i18n
// @ts-ignore
import i18n from './i18n/i18n';

// Vuexy Admin Filters
import './filters/filters';

// Clipboard
import VueClipboard from 'vue-clipboard2';
Vue.use(VueClipboard);

// Tour
import VueTour from 'vue-tour';
Vue.use(VueTour);
require('vue-tour/dist/vue-tour.css');

// VeeValidate
import VeeValidate from 'vee-validate';
Vue.use(VeeValidate);

import Amplify from 'aws-amplify';
//import Amplify, { Analytics} from 'aws-amplify';
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

Vue.config.productionTip = false;
new Vue({router, store, i18n, acl, render: h => h(App)}).$mount('#app');