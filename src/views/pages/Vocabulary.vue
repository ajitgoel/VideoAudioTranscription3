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
            <!-- <ejs-grid :dataSource="vocabularies" :editSettings='editSettings' :toolbar='toolbar' height='273px'>
                <e-columns>
                    <e-column field='vocabulary' headerText='Vocabulary' width=120></e-column>
                </e-columns>
            </ejs-grid> -->
            <ejs-textbox id='default' :multiline="true" placeholder="Enter your vocabularies" floatLabelType="Auto" 
                :input= "inputHandler" v-model="vocabularies" ref="vocabularies" /> 
            <vs-button class="float-right mt-6" @click="Save" :disabled="!validateForm">Save</vs-button>       
        </div>                
    </vx-card>    
</template>
<script>
import { createVocabulary, updateVocabulary} from '@/graphql/mutations';
import {getVocabulary, listVocabularys} from '@/graphql/queries';
import API, {graphqlOperation} from '@aws-amplify/api';
//import { GridPlugin, Page, Toolbar, Edit } from "@syncfusion/ej2-vue-grids";
//import { TextBoxPlugin } from '@syncfusion/ej2-vue-inputs';
import '@syncfusion/ej2-base/styles/material.css';
import '@syncfusion/ej2-vue-inputs/styles/material.css';

export default
{
    data() {
        return {
            vocabularies: '',
            vocabulariesLengthInDatabase: 0,
            inputHandler: (args) => 
            {
                args.event.currentTarget.style.height = "auto";
                args.event.currentTarget.style.height = (args.event.currentTarget.scrollHeight)+"px";
            },
            //editSettings: { allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Batch' },
            //toolbar: ['Add', 'Edit', 'Delete', 'Update', 'Cancel']
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
        this.$refs.vocabularies.$el.focus();
    },
    async created() 
    {
        const userId=this.userIdFromLocalStorage();
        const listVocabularysFilter={userId:{eq:userId}};
        const result = await API.graphql(graphqlOperation(listVocabularys, {filter: listVocabularysFilter}));
        const items=result.data.listVocabularys.items;
        let vocabulariesTemp;
        if(items.length>0)
        {
            vocabulariesTemp=items[0].vocabularies;
        }
        else
        {
            vocabulariesTemp =[''];
        }
        this.vocabulariesLengthInDatabase =vocabulariesTemp.length;
        this.vocabularies=vocabulariesTemp.join('<br/>');
        //this.vocabularies=this.arrayToSingleArrayObject(vocabulariesTemp, 'vocabulary');
        console.log(`this.vocabularies: ${JSON.stringify(this.vocabularies)}`);
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
                const vocabulariesArray=this.vocabularies.split('\n');
                //const vocabulariesArray=Object.values(this.vocabularies);
                        
                if(this.vocabulariesLengthInDatabase==0)
                {
                    const createVocabularyInput={userId:userId, vocabularies:vocabulariesArray};
                    await API.graphql(graphqlOperation(createVocabulary, {input: createVocabularyInput}));
                }
                else
                {
                    const updateVocabularyInput={userId:userId, vocabularies:vocabulariesArray};
                    await API.graphql(graphqlOperation(updateVocabulary, {input: updateVocabularyInput}));
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
    /* provide: 
    {
        grid: [Page, Edit, Toolbar]
    } */
}
</script>

<style>
    #container {
        visibility: hidden;
        padding-left: 5%;
        width: 100%;
    }
    #loader {
        color: #008cff;
        font-family: 'Helvetica Neue','calibiri';
        font-size: 14px;
        height: 40px;
        left: 45%;
        position: absolute;
        top: 45%;
        width: 30%;
    }
    .multiline{
        margin: 10px auto;
        width: 30%;
    }

</style>
