<template>
  <vs-tabs :position="isSmallerScreen ? 'top' : 'left'" class="tabs-shadow-none" id="profile-tabs" :key="isSmallerScreen">

    <!-- GENERAL -->
    <vs-tab icon-pack="feather" icon="icon-user" :label="!isSmallerScreen ? 'General' : ''">
      <div class="tab-general md:ml-4 md:mt-0 mt-4 ml-0">
        <user-settings-general />
      </div>
    </vs-tab>
    <vs-tab icon-pack="feather" icon="icon-lock" :label="!isSmallerScreen ? 'Payment and Transcription' : ''">
      <div class="tab-change-pwd md:ml-4 md:mt-0 mt-4 ml-0">
        <UserPaymentSettings/>
      </div>
    </vs-tab>
    <vs-tab icon-pack="feather" icon="icon-info" :label="!isSmallerScreen ? 'Delete account' : ''">
      <div class="tab-info md:ml-4 md:mt-0 mt-4 ml-0">
        <DeleteAccount/>
      </div>
    </vs-tab>
    <!-- <vs-tab icon-pack="feather" icon="icon-github" :label="!isSmallerScreen ? 'Social Links' : ''">
      <div class="tab-social-links md:ml-4 md:mt-0 mt-4 ml-0">
        <user-settings-social-links />
      </div>
    </vs-tab>
    <vs-tab icon-pack="feather" icon="icon-link-2" :label="!isSmallerScreen ? 'Connections' : ''">
      <div class="tab-text md:ml-4 md:mt-0 mt-4 ml-0">
        <user-settings-connections />
      </div>
    </vs-tab> -->
    <!-- <vs-tab icon-pack="feather" icon="icon-bell" :label="!isSmallerScreen ? 'Notifications' : ''">
      <div class="tab-text md:ml-4 md:mt-0 mt-4 ml-0">
        <user-settings-notifications />
      </div>
    </vs-tab> -->
  </vs-tabs>
</template>

<script>
import UserSettingsGeneral from "./UserSettingsGeneral.vue";
import UserPaymentSettings from "./UserPaymentSettings.vue";
import DeleteAccount from "./DeleteAccount.vue";

export default {
  components: {
    UserSettingsGeneral,
    UserPaymentSettings,
    DeleteAccount,
  },
  data() {
    return {

    }
  },
  computed: {
    isSmallerScreen() {
      return this.$store.state.windowWidth < 768
    }
  }
}
</script>

<style lang="scss">
#profile-tabs {
  .vs-tabs--content {
    padding: 0;
  }
}
</style>
