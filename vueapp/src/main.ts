import { createApp } from 'vue'
import App from './App.vue'
import "vuetify/styles";
import { createVuetify } from "vuetify";
import * as components from "vuetify/components";
import * as directives from "vuetify/directives";
import '@mdi/font/css/materialdesignicons.css'
import { createVueI18nAdapter } from 'vuetify/locale/adapters/vue-i18n'
import VueMeta from 'vue-meta'


const vuetify = createVuetify({
    components,
    directives,
});
createApp(App).use(vuetify).mount('#app')
