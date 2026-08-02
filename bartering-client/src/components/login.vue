<script setup lang="ts">
import router from "@/router";
import { ref } from "vue";
import { user_login } from "@/assets/account";

const username = ref<string>("");
const password = ref<string>("");

function login() {
  user_login(username.value, password.value);
}
</script>

<template>
  <div class="container">
    <input type="text" v-model="username" placeholder="Username" />
    <input type="password" v-model="password" placeholder="Password" />
    <div class="buttons">
      <button @click="login" id="login">Login</button>
      <button @click='router.push("/")' id="cancel">Cancel</button>
    </div>
  </div>
</template>

<style scoped>
.container {
  border: #000000bb solid 3px;
  border-radius: 8px;
  width: 300px;
  height: 150px;
  display: flex;
  flex-direction: column;
  padding-top: 10px;
  align-items: center;
}
.buttons {
  display: flex;
  justify-content: space-evenly;
}
button {
  border-radius: 8px;
  background: none;
  border: black solid 3px;
  font-weight: bolder;
  margin: 10px;
  height: 30px;
  width: 100px;
  font-size: 15px;
}
button:hover {
  background: #00000020;
  cursor: pointer;
}
#cancel:hover {
  background: #FF000030;
}

input {
  border: #00000060 dashed 2px;
  border-radius: 20px;
  padding: 2px;
  outline: none;
  font-size: 15px;
  text-align: center;
  width: 90%;
  height: 50px;
  caret-color: black;
  font-weight: bold;
  margin: 5px;
}
input:hover {
  border: #000000ff dashed 2px;
}
</style>
