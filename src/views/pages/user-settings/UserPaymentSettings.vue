<template>
  <div>
    <vx-card no-shadow>
      <div class="mb-base">
        <h6 class="mb-4">Payment settings</h6>
        <div class="flex items-center mb-4">
          <vs-switch v-model="paymentSettings.autoRecharge" />
          <span class="ml-4">Auto Recharge</span>
        </div>
      </div>  
    </vx-card>    
      
    <vx-card no-shadow>
      <div class="mb-base">
        <h6 class="mb-4">Transcription settings</h6>
        <div class="flex items-center my-4">
          <ejs-dropdownlist id='fileLanguage' :dataSource='$options.fileLanguages' :fields='fileLanguagesFields' 
            v-model="transcriptionSettings.defaultFileLanguageWhenFileIsTranscribed" allowFiltering='true' placeholder='Select default file language when file is transcribed'/>
        </div>
        <div class="flex items-center mb-4">
          <vs-switch v-model="transcriptionSettings.useVocabularyWhenFileIsTranscribed" />
          <span class="ml-4">Use vocabulary when file is transcribed</span>
        </div>
        <div class="flex items-center mb-4">
            <vs-switch v-model="transcriptionSettings.useAutomaticContentRedaction" />
            <span class="ml-4">Use automatic content redaction when file is transcribed              
              <a href='#' @click='showAutomaticContentRedactionHelp=true'>Help</a> 
              <vs-popup title="Help" :active.sync="showAutomaticContentRedactionHelp">
                <AutomaticContentRedactionHelp/>
              </vs-popup> 
            </span>
        </div>
        
      </div>    
    </vx-card>

     <vx-card title="Unwanted words for vocabulary filtering">
        <span>
            You can mask or remove words that you don't want to appear in your transcription results with
            vocabulary filtering. <br/>
            For example, you can use vocabulary filtering to prevent the display of offensive
            or profane terms. <br/>
            This enables you to generate family-friendly captions of a TV show or transcripts of
            conferences that are appropriate for your audiences. <br/>
            Use vocabulary filtering for any word that you consider profane, obscene, offensive, or otherwise 
            unsuitable for the readers of your transcripts.
            <br/>
            To use it simply add words, and select <code>Unwanted words for vocabulary filtering</code> when uploading a file.<br/>
        </span>
        <div>
            <ejs-textbox cssClass="height:500px;" id='vocabularies' :multiline="true" 
            placeholder="Unwanted words for vocaulary filtering" floatLabelType="Auto" :input= "inputHandler" 
              v-model="vocabularies" ref="vocabularies"/>     
        </div> 
    </vx-card> 

    <!--Notifications:Start-->
    <vx-card no-shadow>
      <div class="mb-base">
        <h6 class="mb-4">Notifications</h6>
        <div class="flex items-center mb-4">
          <vs-switch v-model="notificationSettings.notifyWhenTranscriptsCompleted" />
          <span class="ml-4">Notify me when my transcripts are done processing</span>
        </div>
        <div class="flex items-center mb-4">
          <vs-switch v-model="notificationSettings.notifyWhenTranscriptsError" />
          <span class="ml-4">Notify me when there is a problem with my transcripts</span>
        </div>
      </div>
    </vx-card>
    <!--Notifications:End-->

    <div class="flex flex-wrap items-center justify-end mt-base">
      <vs-button class="ml-auto mt-2" @click="save">Save settings</vs-button>
    </div>  
  </div>
</template>
<script>
import { Auth } from 'aws-amplify';
import { createUserProfile, updateUserProfile} from '@/graphql/mutations';
import API, {graphqlOperation} from '@aws-amplify/api';
import {FILE_LANGUAGES} from '@/static.js';
import AutomaticContentRedactionHelp from "@/views/pages/AutomaticContentRedactionHelp.vue";

export default {
  components: {
    AutomaticContentRedactionHelp
  },
  data() {
    return {
      isUserProfileSavedInDatabase: false,
      paymentSettings:{autoRecharge: false},
      transcriptionSettings: {defaultFileLanguageWhenFileIsTranscribed:false, useVocabularyWhenFileIsTranscribed:false,
        useAutomaticContentRedaction:false},
      notificationSettings: {notifyWhenTranscriptsCompleted:false, notifyWhenTranscriptsError:false},
      fileLanguagesFields : { groupBy: 'Category', text: 'Text', value: 'Id' },
      showAutomaticContentRedactionHelp:false,
    }
  },
  computed: { 
    activeUserInfo() {
      return this.$store.state.AppActiveUser
    },   
  },
  async created() 
  {      
    this.$options.fileLanguages = FILE_LANGUAGES;
    const currentUserInfoResult=await this.currentUserInfo();
    const userId=currentUserInfoResult.id;
    const listUserProfilesFilter={id:{eq:userId}};
    const listUserProfilesForPaymentSettings = /* GraphQL */ `
      query ListUserProfiles(
        $filter: ModelUserProfileFilterInput
        $limit: Int
        $nextToken: String
      ) {
        listUserProfiles(filter: $filter, limit: $limit, nextToken: $nextToken) {
          items {
            id
            paymentSettings {
              autoRecharge
            }   
            transcriptionSettings {
              defaultFileLanguageWhenFileIsTranscribed
              useVocabularyWhenFileIsTranscribed
              useAutomaticContentRedaction
            }
            notificationSettings {
              notifyWhenTranscriptsCompleted
              notifyWhenTranscriptsError
            }
          }
          nextToken
        }
      }
    `;

    const result = await API.graphql(graphqlOperation(listUserProfilesForPaymentSettings, {filter: listUserProfilesFilter}));
    console.log(`result: ${JSON.stringify(result)}`);
    const items=result.data.listUserProfiles.items;
    if(items.length>0)
    {
      this.id=items[0].id;
      this.paymentSettings.autoRecharge=(items[0].paymentSettings==null || items[0].paymentSettings.autoRecharge==null) ? false:items[0].paymentSettings.autoRecharge;
      this.transcriptionSettings.defaultFileLanguageWhenFileIsTranscribed=
        (items[0].transcriptionSettings==null || items[0].transcriptionSettings.defaultFileLanguageWhenFileIsTranscribed==null) ? 
        false:items[0].transcriptionSettings.defaultFileLanguageWhenFileIsTranscribed;
      this.transcriptionSettings.useVocabularyWhenFileIsTranscribed=
        (items[0].transcriptionSettings==null || items[0].transcriptionSettings.useVocabularyWhenFileIsTranscribed==null) ? 
        false:items[0].transcriptionSettings.useVocabularyWhenFileIsTranscribed;
      this.transcriptionSettings.useAutomaticContentRedaction=
        (items[0].transcriptionSettings==null || items[0].transcriptionSettings.useAutomaticContentRedaction==null) ? 
        false:items[0].transcriptionSettings.useAutomaticContentRedaction;  
        
      this.notificationSettings.notifyWhenTranscriptsCompleted=
        (items[0].notificationSettings==null || items[0].notificationSettings.notifyWhenTranscriptsCompleted==null) ? 
        false:items[0].notificationSettings.notifyWhenTranscriptsCompleted;
      this.notificationSettings.notifyWhenTranscriptsError=
        (items[0].notificationSettings==null || items[0].notificationSettings.notifyWhenTranscriptsError==null) ? 
        false:items[0].notificationSettings.notifyWhenTranscriptsError;

      this.isUserProfileSavedInDatabase=true;
    }
    else
    {
      this.isUserProfileSavedInDatabase=false;
    }
  },
  methods: 
  {
    async save() 
    {
      try 
      {
        const currentUserInfoResult=await this.currentUserInfo();
        const userId=currentUserInfoResult.id;
        if(userId == null)
        {
          this.$vs.notify({title: 'Error',text: 'There was an error saving your payment settings', iconPack: 'feather', 
              icon: 'icon-alert-circle', color: 'danger'});
          return;   
        }
        console.log(`userId: ${userId}`);
        
        //#region save user profile in dynamodb
        const createOrUpdateUserProfileInput={
              id:userId, 
              paymentSettings:{autoRecharge: this.paymentSettings.autoRecharge,},
              transcriptionSettings:{
                defaultFileLanguageWhenFileIsTranscribed: this.transcriptionSettings.defaultFileLanguageWhenFileIsTranscribed,
                useVocabularyWhenFileIsTranscribed: this.transcriptionSettings.useVocabularyWhenFileIsTranscribed,
                useAutomaticContentRedaction: this.transcriptionSettings.useAutomaticContentRedaction,
                },
              notificationSettings:{
                notifyWhenTranscriptsCompleted: this.notificationSettings.notifyWhenTranscriptsCompleted,
                notifyWhenTranscriptsError: this.notificationSettings.notifyWhenTranscriptsError,
                },  
            };
        if(this.isUserProfileSavedInDatabase==false)
        {
          await API.graphql(graphqlOperation(createUserProfile,{input: createOrUpdateUserProfileInput}));
        }
        else
        {
          await API.graphql(graphqlOperation(updateUserProfile, {input: createOrUpdateUserProfileInput}));
        }                
        //#endregion save user profile in dynamodb
        this.$vs.notify({title: 'Success', text: 'Payment, transcription and notification settings have been saved successfully!', iconPack: 'feather', icon: 'icon-check',color: 'success'}); 
      } 
      catch (error) 
      {
        console.log(error);
        this.$vs.notify({title: 'Error',text: error.message, iconPack: 'feather', icon: 'icon-alert-circle', color: 'danger'});
      };
    },
  }
}
</script>
