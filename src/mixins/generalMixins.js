import Vue from 'vue';

Vue.mixin({
  methods: 
  {
    capitalizeFirstLetter(str) 
    {
        return str.charAt(0).toUpperCase() + str.slice(1);
    },
    userIdFromLocalStorage()
    {
      const userInfo= JSON.parse(localStorage.getItem('userInfo'));
      const userid1=userInfo && userInfo.attributes && userInfo.attributes.sub;
      if(userid1 != null)
      {
        return userid1;
      }
      return null;
    },
    saveUser(cognitoUser)
    {
      localStorage.removeItem("userInfo");
      localStorage.setItem("userInfo", JSON.stringify(cognitoUser));
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