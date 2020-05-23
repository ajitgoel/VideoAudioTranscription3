<template>
    <vx-card title="Your vocabulary">
        <span>
            Create a custom vocabulary to help Video Audio Transcription when transcribing technical terms, proper nouns 
            such as a company name or your cat’s nickname.<br/>
            To use it simply add words, and select <code>Use my vocabulary</code> when uploading a file.<br/><br/>
            <code>
                ✔︎ It can be a word or a phrase (baby boom, gnocchi).<br/>
                ✔︎ Enter only one entry per line.<br/>
                ✔︎ You can enter acronyms (HS, CEO).
            </code><br/><br/>
        </span>
        <div>
            <ejs-textbox cssClass="height:500px;" id='vocabularies' :multiline="true" placeholder="Enter your vocabularies" 
                floatLabelType="Auto" :input= "inputHandler" v-model="vocabularies" ref="vocabularies"/>
            <vs-button class="float-right mt-6" @click="Save" :disabled="!validateForm">Save Changes</vs-button>       
        </div> 
    </vx-card> 
</template>
<script>
import { createUserProfile, updateUserProfile} from '@/graphql/mutations';
import {listUserProfilesForVocabularies} from '@/graphql/customQueries';
import API, {graphqlOperation} from '@aws-amplify/api';
export default
{
    data() {
        return {
            vocabularies: '',
            isUserProfileSavedInDatabase: false,
            inputHandler: (args) => 
            {
                args.event.currentTarget.style.height = "auto";
                args.event.currentTarget.style.height = (args.event.currentTarget.scrollHeight)+"px";
            },
        }
    },
    computed: 
    {
        validateForm() 
        {            
            return this.vocabularies != '';
        }
    },
    mounted() 
    {       
        this.$nextTick(function()
        {
            this.$refs.vocabularies.focusIn();
        });        
    },
    async created() 
    {
        const currentUserInfo=await this.currentUserInfo();
        const userId=currentUserInfo.id;
        
        const listUserProfilesFilter={id:{eq:userId}};
        const result = await API.graphql(graphqlOperation(listUserProfilesForVocabularies, {filter: listUserProfilesFilter}));
        console.log(`result: ${JSON.stringify(result)}`);
        const items=result.data.listUserProfiles.items;
        let vocabulariesTemp;
        if(items.length>0)
        {
            vocabulariesTemp=items[0].vocabularies;
            this.isUserProfileSavedInDatabase =true;
        }
        else
        {
            vocabulariesTemp=[''];
            this.isUserProfileSavedInDatabase=false;
        }
        this.vocabularies=vocabulariesTemp.join('\n');
        this.$refs.vocabularies.ej2Instances.addAttributes({ rows: vocabulariesTemp.length+1 }); 
        console.log(`this.vocabularies: ${JSON.stringify(this.vocabularies)}`);
    }, 
    methods: 
    {
        async Save() 
        {
            try 
            {
                const currentUserInfo=await this.currentUserInfo();
                const userId=currentUserInfo.id;
                if(userId == null)
                {
                    this.$vs.notify({title: 'Error',text: 'There was an error saving your vocabulary', iconPack: 'feather', 
                        icon: 'icon-alert-circle', color: 'danger'});
                    return;   
                }
                console.log(`userId: ${userId}`);
                
                //#region save vocabularies in dynamodb
                const vocabulariesArray=this.vocabularies.split('\n');                        
                if(this.isUserProfileSavedInDatabase==false)
                {
                    const createUserProfileInput={id:userId, vocabularies:vocabulariesArray};
                    await API.graphql(graphqlOperation(createUserProfile,{input: createUserProfileInput}));
                }
                else
                {
                    const updateUserProfileInput={id:userId, vocabularies:vocabulariesArray};
                    await API.graphql(graphqlOperation(updateUserProfile, {input: updateUserProfileInput}));
                }                
                //#endregion save vocabularies in dynamodb

                this.$vs.notify({title: 'Success', text: 'Your vocabularies have been saved successfully!', iconPack: 'feather',
                    icon: 'icon-check',color: 'success'}); 
            } 
            catch (error) 
            {
                console.log(error);
                this.$vs.notify({title: 'Error',text: error.message, iconPack: 'feather', icon: 'icon-alert-circle', 
                    color: 'danger'});
            };
        },
    },
}
</script>

<style lang="scss">

@media print {
  .invoice-page {
    * {
      visibility: hidden;
    }

    #content-area {
      margin: 0 !important;
    }

    #invoice-container,
    #invoice-container * {
      visibility: visible;
    }
    #invoice-container {
      position: absolute;
      left: 0;
      top: 0;
      box-shadow: none;
    }
  }
}
</style>