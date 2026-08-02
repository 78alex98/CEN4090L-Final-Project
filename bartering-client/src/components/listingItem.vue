<script setup lang="ts">
// import imageNotFound from "@/assets/imageNotFound.png";
import placeholder from "@/assets/placeholder.png";
import { selectedListing, selectedItem } from "@/assets/selections.ts";
import { fetchBids, bids } from "@/assets/listings.ts";
const props = defineProps({
  listing: {
    type: Object,
    required: true,
  },
});

function select() {
  if(selectedListing.value !== props.listing){
    selectedItem.value = null;
    bids.value = null;
    selectedListing.value = props.listing;
    fetchBids(props.listing.id);
  }
}
</script>

<template>
  <div
    id="listing"
    :class="{ selected: selectedListing === listing }"
    @click="select"
  >
    <div class="item">
      <div id="info">
        <img :src="listing.item.image || placeholder" alt="Item Image" />
        <p id="name">{{ listing.item.name }}</p>
      </div>
    </div>
    <p id="lid">{{ listing.title }}</p>
  </div>
</template>

<style scoped>
#listing {
  border: 3px solid black;
  border-radius: 10px;
}

#lid {
  overflow: hidden;
  height: 20px;
  font-weight: bold;
  width: 100%;
  text-align: center;
}

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

#listing:hover {
  cursor: pointer;
  border: 3px solid #000000ee;
  background: #0000002f;
}

#listing.selected {
  background-color: #ffa5003f;
  border: 3px solid #c76e004f;
}

.item {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  margin: 10px;
  /* border: 2px solid black; */
  /* padding: 10px; */
  text-align: center;
  width: fit-content;
  height: fit-content;
  background: none;
  user-select: none;
}
.item img {
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
