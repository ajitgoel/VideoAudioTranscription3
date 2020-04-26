<template>
  <div class="layout--full-page">
    <router-view></router-view>
      <the-footer />
  </div>
</template>
<script>
/* import BackToTop           from 'vue-backtotop'
import HNavMenu            from "@/layouts/components/horizontal-nav-menu/HorizontalNavMenu.vue"
import navMenuItems        from "@/layouts/components/vertical-nav-menu/navMenuItems.js"
import TheCustomizer       from "@/layouts/components/customizer/TheCustomizer.vue"
import TheNavbarHorizontal from '@/layouts/components/navbar/TheNavbarHorizontal.vue'
import TheNavbarVertical   from '@/layouts/components/navbar/TheNavbarVertical.vue' */
import TheFooter           from '@/layouts/components/TheFooter.vue';
/* import themeConfig         from '@/../themeConfig.js'
import VNavMenu            from '@/layouts/components/vertical-nav-menu/VerticalNavMenu.vue'

const VxTour = () => import('@/components/VxTour.vue') */

export default {
  components: {
    /* BackToTop,
    HNavMenu,
    TheCustomizer, */
    TheFooter,
    /* TheNavbarHorizontal,
    TheNavbarVertical,
    VNavMenu,
    VxTour */
  },
}
</script>