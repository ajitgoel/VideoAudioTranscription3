# modern

## Project setup
```
npm install
```

### Compiles and hot-reloads for development
```
npm run serve
```

### Compiles and minifies for production
```
npm run build
```

### Run your tests
```
npm run test
```

### Lints and fixes files
```
npm run lint
```
### Project related documentation:
###### AUTHENTICATION: Password & user management: 
https://docs.amplify.aws/lib/auth/manageusers/q/platform/js#change-password
###### AWS CLI Command Reference: 
https://docs.aws.amazon.com/cli/latest/reference/cognito-idp/delete-user-pool.html
###### vee-validate version 2 documentation:
http://vee-validate.logaretm.com/v2/guide/rules.html#digits-params
###### Feather Icons: 
https://feathericons.com/?query=code
###### Font Awesome Icons: 
https://fontawesome.com/icons?d=gallery&q=address

### Articles:
https://medium.com/@dabit3/how-to-build-serverless-vue-applications-with-aws-amplify-67d16c79e9d6
https://pixinvent.com/demo/vuexy-vuejs-admin-dashboard-template/documentation/development/installation.html#follow-along-to-get-everything-running
https://pixinvent.com/demo/vuexy-vuejs-admin-dashboard-template/documentation/development/routing.html#using-parent-property-of-route
###### AWS Amplify Package - aws-amplify-vue: 
https://github.com/aws-amplify/amplify-js/tree/master/packages/aws-amplify-vue
###### How to Build Production-ready Vue Authentication: 
https://webcache.googleusercontent.com/search?q=cache:TOxCI3d96jEJ:https://dev.to/dabit3/how-to-build-production-ready-vue-authentication-23mk+&cd=5&hl=en&ct=clnk&gl=us
###### AWS Amplify Storage: 
https://docs.amplify.aws/lib/storage/upload/q/platform/js
###### A Vue.js starter app integrated with AWS Amplify: 
https://github.com/aws-samples/aws-amplify-vue

### API (GRAPHQL) Directives
https://docs.amplify.aws/cli/graphql-transformer/directives#usage
###### Tailwindcss documentation:
https://tailwindcss.com/docs/margin/

###### Vuesax documentation:
https://lusaxweb.github.io/vuesax/components/input.html#default
###### Material Icons documentation:
https://material.io/resources/icons/?style=baseline

####### GraphQL endpoint: 
https://vjakaofrbngqvdeayxuiehs72u.appsync-api.us-east-1.amazonaws.com/graphql
####### Application url: 
http://vidaudtranscription-20200426180010-hostingbucket-dev.s3-website-us-east-1.amazonaws.com

Full Stack Serverless: Introduction to Authentication: https://www.oreilly.com/library/view/full-stack-serverless/9781492059882/ch04.html
https://webcache.googleusercontent.com/search?q=cache:qACxLV5U1GsJ:https://dev.to/aws/the-complete-react-native-guide-to-user-authentication-with-the-amplify-framework-ib2+&cd=12&hl=en&ct=clnk&gl=us
####### Build Production-ready Vue Authentication:
https://webcache.googleusercontent.com/search?q=cache:TOxCI3d96jEJ:https://dev.to/dabit3/how-to-build-production-ready-vue-authentication-23mk+&cd=1&hl=en&ct=clnk&gl=us
###### Amplify aws Community Resources:
https://amplify.aws/community/resources

Accepting a payment:
https://stripe.com/docs/payments/accept-a-payment

https://www.npmjs.com/package/vue-stripe-elements-plus

#### Tasks:

1. 	Full Page layout
	- [ ] should have header
    - [x] ~~should have footer~~
2. Register screen
    - [x] ~~after registering, user has to confirm his email using confirmation code~~
    - [ ] privacy policy needs to be proof read.
    - [ ] terms and conditions needs to be proof read.
    - [x] ~~Clicking on privacy policy, terms and conditions should open those pages in a new tab.~~
    - [x] ~~not required=>Save profile information(Full name, email) in user profile table.~~
    - [x] ~~Focus should be set on the first email field in forgot password-confirmation screen.~~
3. Login screen
	- [ ] Add users to register using Twitter, Github.
    - [ ] Add users to register using Facebook, Google.
    - [ ] Show loading message when the login button is clicked.
4. Logout screen
	- [ ] 
5. Forget password screen
    - [x] ~~send email to user to confirm email.~~
    - [x] ~~Add confirm email screen~~
    - [ ] Error messages do not show correctly below input element.
    - [x] ~~Focus should be set on the first email field in forgot password-confirmation screen.~~
6. Dashboard screen
    - [x] ~~Dashboard screens should be secured.~~
    - [ ] Show user profile information(Full name, email) from user profile table. 
7. FAQ screen
    - [ ] Add FAQ's.
8. General Tasks
	- [ ] Add AWS analytics
	- [x] ~~Add a key on userprofile.userid~~
9. Settings
	a. Change Password screen:
    - [x] ~~check old password is valid.~~
    - [x] ~~changing the password a second time does not work.~~
	b. Profile screen:
    - [x] ~~email, full name(Individual or company), billing address, country, VAT number(if applicable)~~
    - [x] ~~Notification Preferences=> Notify me when my transcripts are done processing, Notify me when there is a problem with my transcripts~~
    - [x] ~~Show email when the profile screen first loads.~~
    - [ ] Allow user to save his profile picture into AWS S3
    c. Payment settings screen:
    - [x] ~~Basic functionality~~
    d. Profile delete screen:
    - [ ] trigger user deletion from aws cognito through a lambda function, when a user is deleted in user profiles table, https://github.com/aws-amplify/amplify-cli/issues/2569
    - [x] ~~Delete user from user profile table.~~
    - [x] ~~basic functionality~~
10. Vocabulary screen
    - [x] ~~Work with AWS appsync and syncfusion grid to save updated, deleted, inserted info to array.~~
    - [x] ~~unable to focus on textarea.~~
    - [ ] Show number of lines in the textarea, below textarea.
    - [x] ~~set focus to last line in the textarea~~
11. Upload transcribing video
	- [ ] Upload file to S3
	- [ ] call .net webservice to transcribe file using amazon transcribe
	- [ ] Find file formats supported by amazon transcribe
	- [ ] Use metadata passed from the vuejs frontend.
	- [ ] use environment variables from application.
12. Home page screen
    - [ ] Show pricing