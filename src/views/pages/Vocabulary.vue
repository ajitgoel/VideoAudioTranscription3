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
            <vs-textarea v-model="textarea" height="200px" 
            oninput='this.style.height="";this.style.height=this.scrollHeight+"px"'/>
        </div>        
        <vs-button class="float-right" :disabled="!validateForm" @click="Save">Save Changes</vs-button>
    </vx-card>    
</template>
<script>
export default
{
    data() {
        return {textarea: '',}
    },
    computed: 
    {
        validateForm() 
        {
            return !this.errors.any() && this.textarea != '';
        }
    },
    methods: 
    {
        async Save() 
        {
            try 
            {
                const userInfo= JSON.parse(localStorage.getItem('userInfo'));
                const userid1=userInfo && userInfo.attributes && userInfo.attributes.sub;
                const userid2=userInfo && userInfo.userSub;
                let userid;
                if(userid1 == null && userid2 == null)
                {
                 this.$vs.notify({title: 'Error',text: 'There was an error saving your vocabulary', iconPack: 'feather', 
                    icon: 'icon-alert-circle', color: 'danger'});
                    return;   
                }
                console.log(`userid1: ${userid1} userid2: ${userid2}`);
                //save vocabularies in dynamodb
                this.$router.push('/transcripts').catch(() => {});  
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
