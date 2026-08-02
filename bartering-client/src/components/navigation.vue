<script setup lang="ts">
import { useRouter, useRoute } from "vue-router";
import { ref } from "vue";

const router = useRouter();
const route = useRoute();
const current = ref<string>(route.path.split("/")[2]); // get the path after app

function select(choice: string) {
  current.value = choice;
  router.push("/app/" + choice);
}
</script>

<template>
  <div id="main">
    <button
      @click="select('listings')"
      :class="{ selected: current === 'listings' }"
    >
      All Listings
    </button>
    <button
      @click="select('my-listings')"
      :class="{ selected: current === 'my-listings' }"
    >
      My Listings
    </button>
    <button
      @click="select('inventory')"
      :class="{ selected: current === 'inventory' }"
    >
      Inventory
    </button>
    <button
      @click="select('account')"
      :class="{ selected: current === 'account' }"
    >
      Account
    </button>
  </div>
</template>

<style scoped>
#main {
  height: 70px;
  width: 900px;
  border: solid #00000009 3px;
  background-color: #00000009;
  border-radius: 20px;
  margin: 10px;
  align-items: center;
  justify-content: center;
  display: flex;
}
button {
  border-radius: 8px;
  background: white;
  border: black solid 3px;
  font-weight: bolder;
  margin: 15px;
  height: 40px;
  width: 95%;
  font-size: 20px;
}
button:hover {
  background: #00000020;
  cursor: pointer;
}

button.selected {
  background: #00ff0019;
  /* background: white; */
  /* background-image: linear-gradient(to right, #00ff0020 50%, transparent 50%); */
  /* background-size: 5px 5px; */
  /* background-repeat: repeat; */
}
</style>
