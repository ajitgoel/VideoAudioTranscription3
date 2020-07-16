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
        const currentUserInfoResult=await this.currentUserInfo();
        const userId=currentUserInfoResult.id;
        
        const listUserProfilesFilter={id:{eq:userId}};
        const listUserProfilesForVocabularies = /* GraphQL */ `
        query ListUserProfiles(
            $filter: ModelUserProfileFilterInput
            $limit: Int
            $nextToken: String
        ) {
            listUserProfiles(filter: $filter, limit: $limit, nextToken: $nextToken) {
            items {
                id
                vocabularies
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
            vocabulariesTemp=items[0].vocabularies;
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
                const userProfileInput={id:userId, vocabularies:vocabulariesArray_RemoveDuplicates};
                    
                if(this.isUserProfileSavedInDatabase==false)
                {
                    await API.graphql(graphqlOperation(createUserProfile,{input: userProfileInput}));
                }
                else
                {
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