<script setup lang="ts">
import placeholder from "@/assets/placeholder.png";
import { inventoryData, fetchInventory } from "@/assets/inventory.ts";
import { ref, onMounted } from "vue";
import { selectedListing } from "@/assets/selections.ts";
import {
  makeListing,
  deleteListing,
  userListings,
  fetchBids,
  userBid,
  bids,
} from "@/assets/listings.ts";
import bid from "@/components/bid.vue";
import { userData } from "@/assets/account.ts";

function remove(){
  deleteListing(selectedListing.value.id);
}

onMounted(() => {
  fetchInventory();
});
</script>

<template>
  <div id="container" v-if="selectedListing != null">
    <div id="listing">
      <h2>Listing Details</h2>
      <textarea disabled id="name">{{ selectedListing.title }} </textarea>
      <textarea disabled id="description">{{
        selectedListing.description
      }}</textarea>
      <button v-if="userData.UserName === selectedListing.item.owner" @click="remove">
        Remove
      </button>
    </div>
    <div id="item">
      <h2>Item Details</h2>
      <img :src="selectedListing.item.image || placeholder" alt="Item Image" />
      <textarea disabled id="name">
Name: {{ selectedListing.item.name }} </textarea
      >
      <textarea disabled id="description">{{
        selectedListing.item.description
      }}</textarea>
    </div>
    <div class="bids" v-if="userBid !== null && userBid.length !== 0">
      <h2>My Bid</h2>
      <bid
        v-for="bid in userBid"
        :key="bid.item.id"
        :item="bid.item"
        :owner="bid.user"
        :isPlaced="1"
        :bidId="bid.id"
      />
    </div>

    <div class="bids">
      <h2>All Bids</h2>
      <bid
        v-for="bid in bids"
        :key="bid.item.id"
        :item="bid.item"
        :owner="bid.user"
        :isPlaced="1"
        :bidId="bid.id"
      />
    </div>
    <div
      class="bids"
      v-if="
        selectedListing.item.owner !== userData.UserName &&
        userBid !== null &&
        userBid.length === 0
      "
    >
      <h2>Place Bid</h2>
      <bid
        v-for="item in inventoryData"
        :key="item.id"
        :item="item"
        :owner="userData.UserName"
        :isPlaced="0"
      />
    </div>
  </div>
  <div v-else>
    <h2>No Listing Selected</h2>
  </div>
</template>

<style scoped>
button {
  border-radius: 5px;
  background: none;
  border: #0000002f solid 2px;
  font-weight: bold;
  width: 50%;
  height: 30px;
  cursor: pointer;
  margin-bottom: 10px;
}

#listing button:hover {
  background-color: #ff00002f;
}

#name {
  height: 20px;
  margin-top: 10px;
  margin-bottom: 10px;
  border-radius: 0;
}

#description {
  margin-bottom: 10px;
}
textarea {
  width: 90%;
  height: 70px;
  border: #00000020 dashed 2px;
  padding: 2px;
  border-radius: 15px;
  outline: none;
  font-size: 15px;
  text-align: center;
  caret-color: black;
  font-weight: bold;
  resize: none;
}
h2 {
  width: 100%;
  margin-top: 10px;
  padding-bottom: 5px;
  text-align: center;
  border-bottom: 3px solid black;
}

#item img {
  width: 100%;
  border-bottom: 3px solid black;
}

#listing {
  border: 3px solid #000000ee;
  width: 80%;
  height: fit-content;
  overflow: wrap;
  display: flex;
  flex-direction: column;
  align-content: center;
  align-items: center;
  border-radius: 5px;
  background-color: white;
  margin-top: 20px;
  margin-bottom: 20px;
}
.bids {
  border: 3px solid #000000ee;
  width: 80%;
  max-height: 350px;
  display: flex;
  flex-wrap: wrap;
  flex-direction: row;
  overflow-y: scroll;
  justify-content: center;
  border-radius: 5px;
  background-color: white;
  margin-bottom: 20px;
}

.bids h2 {
  border: none;
}

#item {
  border: 3px solid #000000ee;
  width: 80%;
  height: fit-content;
  margin-bottom: 20px;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  align-content: center;
  align-items: center;
  border-radius: 5px;
  background-color: white;
}

#container {
  max-width: 100%;
  max-height: 100%;
  min-width: auto;
  min-height: auto;

  display: flex;
  flex-wrap: wrap;
  flex-direction: row;
  overflow-y: scroll;
  justify-content: center;
}
</style>
