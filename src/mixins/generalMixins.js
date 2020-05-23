import Vue from 'vue';
import { Auth } from 'aws-amplify';

Vue.mixin({
  methods: 
  {
    capitalizeFirstLetter(str) 
    {
        return str.charAt(0).toUpperCase() + str.slice(1);
    },
    async currentUserInfo()
    {
      const result=await Auth.currentUserInfo();
      console.log(`currentUserInfo result: ${JSON.stringify(result)}`);
      return {id:result.username, email:result.attributes.email};
    },
    arrayToSingleArrayObject(array, keyname) 
    {
      var objectResult = {};
      for (var i = 0; i < array.length; ++i)
      {
        if (array[i] !== undefined) 
        {
          objectResult[keyname] = array[i];
        }
      }  
      let arrayResult=[];
      arrayResult[0]=objectResult;
      return arrayResult;
    }
  }
})