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

amplify init
amplify add auth
amplify add storage
amplify add hosting
amplify push
amplify publish
##### Delete user from aws app pool:
aws cognito-idp admin-delete-user --user-pool-id us-east-1_Fvp76tzTJ --username ajitgoel@gmail.com --profile amplify-workshop-user

| Category | Resource name               | Operation | Provider plugin   |
| -------- | --------------------------- | --------- | ----------------- |
| Auth     | vidaudtranscriptionf4ea9b8a | Create    | awscloudformation |

#### Tasks:

1. 	Full Page layout
	- [ ] should have header
    - [x] ~~should have footer~~
2. Register screen
    => - [ ] after registering, user has to confirm his email using confirmation code
    - [ ] privacy policy will need to be checked
    - [ ] terms and conditions will need to be checked
    - [ ] Clicking on privacy policy, terms and conditions should open those pages in a new tab. 
3. Login screen
	- [ ] Add users to register using Twitter, Github.
    - [ ] Add users to register using Facebook, Google.
4. Logout screen
	- [ ] 
5. Forget password screen
    - [ ] send email to user to confirm email. 
    - [ ] Add confirm email screen
6. Dashboard screen
    - [ ] the application should show the logged in user and his name.

aws cognito-idp admin-delete-user --user-pool-id us-east-1_Fvp76tzTJ --username ajitgoel@gmail.com
