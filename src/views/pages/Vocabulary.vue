<!-- =========================================================================================
    File Name: Vocabulary.vue
    Description: Vocabulary Page    
========================================================================================== -->
<template>
    <vx-card title="Your vocabulary" code-toggler>
        <span>
            Create a custom vocabulary to help Happy Scribe when transcribing technical terms, proper nouns 
            such as a company name or your cat’s nickname.<br/>
            To use it simply add words, and select <code>Use my vocabulary</code> when uploading a file.<br/><br/>
            <code>
                ✔︎ It can be a word or a phrase (baby boom, gnocchi).<br/>
                ✔︎ Enter only one entry per line.<br/>
                ✔︎ You can enter acronyms (HS, CEO).
            </code><br/><br/>
        </span>
        <div class="mt-100">
            <!-- <vs-textarea v-model="vocabularies" height="200px" 
            oninput='this.style.height="";this.style.height=this.scrollHeight+"px"'/>  -->
            <ejs-grid :editSettings='editSettings' :toolbar='toolbar' height='273px'>
                <e-columns>
                    <e-column field='CustomerID' headerText='Customer ID' width=120></e-column>
                </e-columns>
            </ejs-grid>
        </div>                
    </vx-card>    
</template>
<script>
import { createVocabulary } from '@/graphql/mutations';
import {getVocabulary, listVocabularys} from '@/graphql/queries';
import API, {graphqlOperation} from '@aws-amplify/api';

export default
{
    data() {
        return {
            vocabularies: '',
            editSettings: { allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Batch' },
            toolbar: ['Add', 'Edit', 'Delete', 'Update', 'Cancel']
        }
    },
   /*  provide: {
        grid: [Page, Edit, Toolbar]
    }, */
    computed: 
    {
    },
    async created() 
    {
        const userId=this.userIdFromLocalStorage();
        const listVocabularysFilter={userId:{eq:userId}};
        const vocabulariesTemp = await API.graphql(graphqlOperation(listVocabularys, {filter: listVocabularysFilter}));
        this.vocabularies  = vocabulariesTemp.data.listVocabularys.items[0].vocabularies;
    },
    methods: 
    {
        async Save() 
        {
            try 
            {
                const userId=this.userIdFromLocalStorage();
                if(userId == null)
                {
                 this.$vs.notify({title: 'Error',text: 'There was an error saving your vocabulary', iconPack: 'feather', 
                    icon: 'icon-alert-circle', color: 'danger'});
                    return;   
                }
                console.log(`userId: ${userId}`);
                //#region save vocabularies in dynamodb
                const vocabulariesArray=this.vocabularies.split("\n");
                const createVocabularyInput={userId:userid, vocabularies:vocabulariesArray};

                await API.graphql(graphqlOperation(createVocabulary, {input: createVocabularyInput}));
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
