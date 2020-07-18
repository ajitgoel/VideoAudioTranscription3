import Vue from 'vue';
import { Auth } from 'aws-amplify';
import {listUserProfilesForGeneral} from '@/graphql/customQueries';
import API, {graphqlOperation} from '@aws-amplify/api';

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
      return {id:result.id, email:result.attributes.email};
    },
    async getuserprofile()
    {
      const resultCurrentUserInfo=await this.currentUserInfo();
      const userId=resultCurrentUserInfo.id;      
      const listUserProfilesFilter={id:{eq:userId}};
      const result = await API.graphql(graphqlOperation(listUserProfilesForGeneral, {filter: listUserProfilesFilter}));
      console.log(`result: ${JSON.stringify(result)}`);
      const items=result.data.listUserProfiles.items;
      return items;
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