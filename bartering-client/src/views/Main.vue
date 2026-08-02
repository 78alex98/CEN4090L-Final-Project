<script setup lang="ts">
import { userData, refresh } from "@/assets/account";
import navigation from "@/components/navigation.vue";
import { useRouter, useRoute } from "vue-router";
import { onUpdated, onMounted, ref } from "vue";
import { selectedItem, selectedListing } from "@/assets/selections.ts";

let hasControl = ref<bool>(false);

function update() {
  selectedItem.value = null;
  selectedListing.value = null;
  const route = useRoute();
  const current = ref<string>(route.path.split("/")[2]); // get the path after app
  if (current.value === "account") {
    hasControl.value = false;
  } else {
    hasControl.value = true;
  }
}

onMounted(() => {
  update();
});

onUpdated(() => {
  update();
});
</script>

<template>
  <body id="bod">
    <div id="boxes">
      <div id="main-box">
        <router-view />
      </div>
      <div id="control-box" v-if="hasControl">
        <router-view name="control" />
      </div>
    </div>
    <navigation />
  </body>
</template>

<style scoped>
h1 {
  text-align: center;
}

#main-box {
  width: 60vw;
  height: 70vh;
  border: #00000080 dashed 4px;
  border-radius: 10px;
  margin: 5px;
}

#control-box {
  width: 20vw;
  max-width: 300px;
  height: 65vh;
  /* border: #00000040 solid 2px; */
  background-color: #0000000a;
  border-radius: 10px;
  margin: 5px;
}

#boxes {
  display: flex;
  flex-direction: row;
  align-items: center;
}

#bod {
  height: 100vh;
  width: 100vw;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}
</style>
