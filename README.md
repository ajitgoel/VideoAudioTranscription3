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
https://medium.com/@dabit3/how-to-build-serverless-vue-applications-with-aws-amplify-67d16c79e9d6
https://pixinvent.com/demo/vuexy-vuejs-admin-dashboard-template/documentation/development/installation.html#follow-along-to-get-everything-running
https://pixinvent.com/demo/vuexy-vuejs-admin-dashboard-template/documentation/development/routing.html#using-parent-property-of-route
### AWS Amplify Package - aws-amplify-vue: 
https://github.com/aws-amplify/amplify-js/tree/master/packages/aws-amplify-vue
### How to Build Production-ready Vue Authentication: 
https://webcache.googleusercontent.com/search?q=cache:TOxCI3d96jEJ:https://dev.to/dabit3/how-to-build-production-ready-vue-authentication-23mk+&cd=5&hl=en&ct=clnk&gl=us

### AUTHENTICATION: Password & user management: 
https://docs.amplify.aws/lib/auth/manageusers/q/platform/js#change-password
### AWS CLI Command Reference: 
https://docs.aws.amazon.com/cli/latest/reference/cognito-idp/delete-user-pool.html

#### Tailwindcss documentation:
https://tailwindcss.com/docs/margin/
#### vee-validate version 2 documentation:
http://vee-validate.logaretm.com/v2/guide/rules.html#digits-params
#### Vuesax documentation:
https://lusaxweb.github.io/vuesax/components/input.html#default
#### Material Icons documentation:
https://material.io/resources/icons/?style=baseline

###### Commands:
npm install -g @aws-amplify/cli
amplify configure
npm install -g @vue/cli
npm install aws-amplify @aws-amplify/ui-vue

amplify init
amplify add auth
amplify add storage
amplify add hosting
amplify push
amplify publish
delete folders very fast: RMDIR /Q/S node_modules

####### Delete user from aws app pool:
aws cognito-idp admin-delete-user --user-pool-id us-east-1_Fvp76tzTJ --username ajitgoel@gmail.com --profile amplify-workshop-user
####### GraphQL endpoint: 
https://vjakaofrbngqvdeayxuiehs72u.appsync-api.us-east-1.amazonaws.com/graphql
####### Application url: 
http://vidaudtranscription-20200426180010-hostingbucket-dev.s3-website-us-east-1.amazonaws.com

Full Stack Serverless: Introduction to Authentication: https://www.oreilly.com/library/view/full-stack-serverless/9781492059882/ch04.html
https://webcache.googleusercontent.com/search?q=cache:qACxLV5U1GsJ:https://dev.to/aws/the-complete-react-native-guide-to-user-authentication-with-the-amplify-framework-ib2+&cd=12&hl=en&ct=clnk&gl=us
####### Build Production-ready Vue Authentication:
https://webcache.googleusercontent.com/search?q=cache:TOxCI3d96jEJ:https://dev.to/dabit3/how-to-build-production-ready-vue-authentication-23mk+&cd=1&hl=en&ct=clnk&gl=us

| Category  | Resource name               | Operation | Provider plugin   |
| --------- | --------------------------- | --------- | ----------------- |
| Analytics | vidaudtranscription         | Create    | awscloudformation |
| Auth      | vidaudtranscriptionf4ea9b8a | Update    | awscloudformation |
| Api       | vidaudtranscription         | No Change | awscloudformation |
| Function  | S3Trigger355399e5           | No Change | awscloudformation |
| Storage   | vidaudtranscription         | No Change | awscloudformation |
| Hosting   | S3AndCloudFront             | No Change | awscloudformation |

#### Tasks:

1. 	Full Page layout
	- [ ] should have header
    - [x] ~~should have footer~~
2. Register screen
    - [x] ~~after registering, user has to confirm his email using confirmation code~~
    - [ ] privacy policy will need to be checked
    - [ ] terms and conditions will need to be checked
    - [x] ~~Clicking on privacy policy, terms and conditions should open those pages in a new tab.~~ 
3. Login screen
	- [ ] Add users to register using Twitter, Github.
    - [ ] Add users to register using Facebook, Google.
4. Logout screen
	- [ ] 
5. Forget password screen
    - [x] ~~send email to user to confirm email. ~~
    - [x] ~~Add confirm email screen~~
    - [ ] Error messages do not show correctly below input element.
6. Dashboard screen
    - [ ] the application should show the logged in user and his name.
    - [x] ~~Dashboard screens should be secured.~~
7. FAQ screen
    - [ ] Add FAQ's.
8. General Tasks
	- [x] Add AWS analytics
