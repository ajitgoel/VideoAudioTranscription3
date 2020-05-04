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
      const userid2=userInfo && userInfo.userSub;
      if(userid1 == null && userid2 == null)
      {
          return null;   
      }
      if(userid1 != null)
      {
        return userid1;
      }
      else if(userid2 != null)
      {
        return userid2;
      }
      return null;
    },
    saveUser(cognitoUser)
    {
      localStorage.removeItem("userInfo");
      localStorage.setItem("userInfo", JSON.stringify(cognitoUser));
    },

  }
})