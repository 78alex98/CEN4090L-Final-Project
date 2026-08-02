<script setup lang="ts">
import router from "@/router";
import { userData, user_logout } from "@/assets/account";

function delete_press() {
  console.log("Account Delete Pressed");
}
</script>

<template>
  <div class="container">
    <h2>Name</h2>
    <h1>{{userData.UserName}}</h1>
    <div class="buttons">
      <button @click="delete_press" id="delete">Delete</button>
      <button @click='user_logout' id="logout">Logout</button>
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
