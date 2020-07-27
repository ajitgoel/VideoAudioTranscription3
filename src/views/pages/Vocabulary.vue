<template>
    <vx-card title="Your vocabulary">
        <span>
            You can give Video Audio Transcription more information about how to process speech in your input file by
            creating a custom vocabulary. <br/>
            A custom vocabulary is a list of specific words that you want Video Audio Transcription 
            to recognize in your audio input. <br/>
            These are generally domain-specific words and phrases, words that Video Audio Transcription isn't recognizing, 
            or proper nouns.
            <br/>
            To use it simply add words, and select <code>Use my vocabulary</code> when uploading a file.<br/><br/>
            <code>
                ✔︎ Enter only one entry per line.<br/>
                ✔︎ Enter acronyms or other words whose letters should be pronounced individually as single letters separated by dots, such as A.B.C. or F.B.I.. <br/>
                ✔︎ To enter the plural form of an acronym, such as "FBIs", separate the "s" from the acronym with a hyphen: F.B.I.-s.<br/>
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
import API, {graphqlOperation} from '@aws-amplify/api';
//import { createUserProfile, updateUserProfile} from '@/graphql/mutations';

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
        const currentUserInfoResult=await this.currentUserInfo();
        const userId=currentUserInfoResult.id;
        
        const listUserProfilesFilter={id:{eq:userId}};
        const listUserProfilesForVocabularies = `
        query ListUserProfiles(
            $filter: ModelUserProfileFilterInput
            $limit: Int
            $nextToken: String
        ) {
            listUserProfiles(filter: $filter, limit: $limit, nextToken: $nextToken) {
            items {
                id
                transcriptionSettings {
                    vocabularies
                }                   
            }
            nextToken
            }
        }
        `;
        const result = await API.graphql(graphqlOperation(listUserProfilesForVocabularies, {filter: listUserProfilesFilter}));
        console.log(`result: ${JSON.stringify(result)}`);
        const items=result.data.listUserProfiles.items;
        let vocabulariesTemp;
        if(items.length>0)
        {
            if(items[0].transcriptionSettings!=null && items[0].transcriptionSettings.vocabularies != null)
            {
                vocabulariesTemp=items[0].transcriptionSettings.vocabularies;
            }
            else
            {
                vocabulariesTemp=[''];
            }            
            this.isUserProfileSavedInDatabase =true;
        }
        else
        {
            vocabulariesTemp=[''];
            this.isUserProfileSavedInDatabase=false;
        }
        this.vocabularies=vocabulariesTemp.join('\n');
        console.log(`this.vocabularies: ${JSON.stringify(this.vocabularies)}`);
        this.$nextTick(function()
        {
            this.$refs.vocabularies.ej2Instances.addAttributes({ rows: vocabulariesTemp.length+1 }); 
        });  
    }, 
    methods: 
    {
        async Save() 
        {
            try 
            {
                const currentUserInfoResult=await this.currentUserInfo();
                const userId=currentUserInfoResult.id;
                if(userId == null)
                {
                    this.$vs.notify({title: 'Error',text: 'There was an error saving your vocabulary', iconPack: 'feather', 
                        icon: 'icon-alert-circle', color: 'danger'});
                    return;   
                }
                console.log(`userId: ${userId}`);
                
                //#region save vocabularies in dynamodb
                let vocabulariesArray=this.vocabularies.split('\n');  
                let vocabulariesArray_RemoveEmptyElements = vocabulariesArray.filter(function(item){return item!==''});
                let vocabulariesArray_RemoveDuplicates = vocabulariesArray_RemoveEmptyElements.filter(function(item, index){return vocabulariesArray_RemoveEmptyElements.indexOf(item) === index});
                const userProfileInput={id:userId, transcriptionSettings:{vocabularies:vocabulariesArray_RemoveDuplicates}};
                
                if(this.isUserProfileSavedInDatabase==false)
                {
                    const createUserProfile = `
                        mutation CreateUserProfile(
                            $input: CreateUserProfileInput!
                            $condition: ModelUserProfileConditionInput
                        ) {
                            createUserProfile(input: $input, condition: $condition) {
                            id
                            transcriptionSettings {
                                    vocabularies
                                }   
                            }
                        }`;
                    await API.graphql(graphqlOperation(createUserProfile,{input: userProfileInput}));
                }
                else
                {
                    const updateUserProfile = `
                        mutation UpdateUserProfile(
                            $input: UpdateUserProfileInput!
                            $condition: ModelUserProfileConditionInput
                        ) {
                            updateUserProfile(input: $input, condition: $condition) {
                            id
                            transcriptionSettings {
                                    vocabularies
                                }   
                            }
                        }`;
                    await API.graphql(graphqlOperation(updateUserProfile, {input: userProfileInput}));
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