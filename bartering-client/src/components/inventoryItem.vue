<script setup lang="ts">
// import imageNotFound from "@/assets/imageNotFound.png";
import placeholder from "@/assets/placeholder.png";
import { selectedItem } from "@/assets/selections.ts";
const props = defineProps({
  item: {
    type: Object,
    required: true,
  },
});

function removeItem() {
  console.log("Removing " + props.item.name);
}
</script>

<template>
  <div class="inventory-item" @click="selectedItem = item">
    <div id="info" :class="{ selected: selectedItem === item }">
      <img :src="item.image || placeholder" alt="Item Image" />
      <p id="name">{{ item.name }}</p>
    </div>
  </div>
</template>

<style scoped>
#name {
  overflow: hidden;
  height: 20px;
  font-weight: bold;
  width: 100%;
}

#info {
  border: 3px solid #000000aa;
  border-radius: 10px;
  width: 120px;
  height: 130px;
  /* padding: 5px; */
  overflow: hidden;
}

#info:hover {
  cursor: pointer;
  border: 3px solid #000000ee;
  background: #0000002f;
}

#info.selected {
  background-color: #FFA5003f;
  border: 3px solid #c76e004f;
}

.inventory-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  margin: 10px;
  /* border: 2px solid black; */
  /* padding: 10px; */
  text-align: center;
  width: auto;
  height: auto;
  background: white;
  user-select: none;
}
.inventory-item img {
  width: 100%;
  height: 100px;
  object-fit: cover;
  border-bottom: 3px solid black;
}

button {
  margin-top: 3px;
  border-radius: 5px;
  background: none;
  border: #0000002f solid 2px;
  font-weight: bold;
  /* padding: 5px 10px; */
  width: 80px;
  height: 25px;
  cursor: pointer;
}

button:hover {
  background: #ff00002f;
}
</style>
