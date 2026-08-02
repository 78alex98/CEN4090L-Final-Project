<script setup lang="ts">
import { ref } from "vue";
import placeholder from "@/assets/placeholder.png";
import { selectedItem } from "@/assets/selections.ts";
import { userData } from "@/assets/account.ts";
import { selectedListing } from "@/assets/selections.ts";
import { postBid, deleteBid, selectWinner } from "@/assets/listings.ts";

const props = defineProps({
  item: {
    type: Object,
    required: true,
  },
  owner: {
    type: String,
    required: true,
  },
  isPlaced: {
    type: Number,
    required: true,
  },
  bidId: {
    type: Number,
    required: false,
  },
});

function click() {
  // emit("post", props.item.id);
  postBid(selectedListing.value.id, props.item.id);
}
function remove() {
  // emit("delete", props.bidId);
  deleteBid(selectedListing.value.id, props.bidId);
}
function win() {
  selectWinner(selectedListing.value.id, props.bidId);
}
</script>

<template>
  <div
    id="container"
    @click="selectedItem = item"
    :class="{ selected: selectedItem === item }"
  >
    <div v-if="selectedItem != item" id="basic">
      <p id="name">{{ item.name }}</p>
    </div>
    <div v-else id="detail">
      <p id="name">{{ item.name }}</p>
      <img :src="item.image || placeholder" alt="Item Image" />
      <textarea disabled id="description">{{ item.description }}</textarea>
      <button
        v-if="owner === userData.UserName && isPlaced === 0"
        @click="click"
      >
        Place Bid
      </button>
      <button
        v-else-if="owner === userData.UserName && isPlaced === 1"
        @click="remove"
      >
        Remove Bid
      </button>
      <button
        v-else-if="
          selectedListing.item.owner === userData.UserName && isPlaced === 1
        "
        @click="win"
      >
        Select Winner
      </button>
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

#description {
  width: 90%;
  height: 50px;
  border: #00000020 dashed 2px;
  border-radius: 15px;
  outline: none;
  font-size: 15px;
  text-align: center;
  caret-color: black;
  font-weight: bold;
  resize: none;
}

#detail {
  overflow: hidden;
}

#container {
  flex-direction: column;
  align-items: center;
  justify-content: center;
  border-top: 2px solid black;
  text-align: center;
  width: 100%;
  height: 30px;
  background: white;
  user-select: none;
  overflow: hidden;
}

#container:hover {
  background-color: #0000001f;
  cursor: pointer;
}

#container.selected {
  height: fit-content;
  background-color: #ffa5003f;
  padding-top: 5px;
  padding-bottom: 5px;
  cursor: default;
}
#container img {
  width: 100%;
  height: 150px;
  object-fit: cover;
  border-bottom: 3px solid black;
  border-top: 3px solid black;
}

button {
  margin-top: 10px;
  border-radius: 5px;
  background: none;
  border: #0000002f solid 2px;
  font-weight: bold;
  width: 60%;
  height: 30px;
  font-weight: bold;
  cursor: pointer;
  background-color: #ffffffaf;
}

button:hover {
  background: #ffa5005f;
}
</style>
