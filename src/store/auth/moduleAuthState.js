/*=========================================================================================
  File Name: moduleAuthState.js
  Description: Auth Module State  
==========================================================================================*/
import firebase from 'firebase/app';
import 'firebase/auth';
export default {
    /* isUserLoggedIn: () => {
        let isAuthenticated = false
        // get firebase current user
        const firebaseCurrentUser = firebase.auth().currentUser
        if (auth.isAuthenticated() || firebaseCurrentUser) isAuthenticated = true
        else isAuthenticated = false
        return (localStorage.getItem('userInfo') && isAuthenticated)
    }, */
    isUserLoggedIn: () => (localStorage.getItem('userInfo') && firebase.auth().currentUser),
}
