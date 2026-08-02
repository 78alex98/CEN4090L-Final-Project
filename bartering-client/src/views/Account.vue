<script setup lang="ts">
import { onMounted, ref } from "vue";
import login from "../components/login.vue";
import register from "@/components/register.vue";
import manage from "@/components/manage.vue";
import { userData, update_user_data } from "@/assets/account";

const register_toggle = ref<boolean>(false);

function toggle_register(){
  register_toggle.value = !register_toggle.value;
}

onMounted(() => {
  update_user_data(); // more efficient than refresh()
})
</script>

<template>
  <body v-if="userData">
    <h1>Manage Account</h1>
    <manage />
  </body>
  <body v-else-if="!register_toggle">
    <h1>Login to Existing Account</h1>
    <login />
    <button @click="toggle_register" id="toggle">Create Account</button>
  </body>
  <body v-else>
    <h1>Register New Account</h1>
    <register />
    <button @click="toggle_register" id="toggle">Have Account?</button>
  </body>
</template>

<style scoped>
body {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 100%;
}
button {
  border-radius: 8px;
  background: none;
  border: black solid 3px;
  font-weight: bolder;
  margin: 10px;
  height: 30px;
  width: auto;
  padding: 10px;
  display: inline-flex;
  align-items: center;
  text-align: center;
  font-size: 15px;
}
button:hover {
  background: #0000FF20;
  cursor: pointer;
}
h1{
  margin: 2%;
}
</style>
