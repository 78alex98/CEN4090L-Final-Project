<script setup lang="ts">
import placeholder from "@/assets/placeholder.png";
import { ref, onMounted } from "vue";
import { selectedItem } from "@/assets/selections.ts";
import { deleteItem } from "@/assets/inventory.ts";
import { makeListing } from "@/assets/listings.ts";

const ltitle = ref<string>("");
const ldescription = ref<string>("");
const lmessage = ref<string>("");

function deletePressed() {
  deleteItem(selectedItem.value.id);
  selectedItem.value = null;
}

function publishPressed() {
  if (ltitle.value !== "") {
    makeListing(
      selectedItem.value.id,
      ltitle.value,
      ldescription.value,
      lmessage.value,
    );
    // selectedItem.value = null;
    ltitle.value = "";
    ldescription.value = "";
    lmessage.value = "";
    selectedItem.value.isListed = true;
  }
}
</script>

<template>
  <div id="container" v-if="selectedItem != null">
    <div id="details">
      <h2>Item Details</h2>
      <img :src="selectedItem.image || placeholder" alt="Item Image" />
      <textarea disabled id="name">Name: {{ selectedItem.name }} </textarea>
      <textarea disabled id="description">{{
        selectedItem.description
      }}</textarea>
      <button id="delete" @click="deletePressed">Delete</button>
    </div>
    <div id="options" v-if="!selectedItem.isListed">
      <h2>Make Listing</h2>
      <textarea id="name" v-model="ltitle" placeholder="Listing Title" />
      <textarea
        id="description"
        v-model="ldescription"
        placeholder="Listing Description"
      />
      <textarea
        id="description"
        v-model="lmessage"
        placeholder="Message Sent to Winner"
      />
      <button id="publish" @click="publishPressed">Publish Listing</button>
    </div>
  </div>
  <div v-else>
    <h2>No Item Selected</h2>
  </div>
</template>

<style scoped>
button {
  border-radius: 5px;
  background: none;
  border: #0000002f solid 2px;
  font-weight: bold;
  width: 80px;
  height: 30px;
  cursor: pointer;
}
button#delete {
  margin-bottom: 10px;
  width: 50%;
  font-weight: bold;
  font-size: 15px;
}
button#delete:hover {
  background: #ff00002f;
}
button#add:hover {
  background: #0000ff3f;
}

button#publish {
  margin-bottom: 10px;
  width: 60%;
  font-weight: bold;
  font-size: 15px;
  overflow: auto;
}

button#publish:hover {
  background: #0000ff2f;
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

#details img {
  width: 100%;
  border-bottom: 3px solid black;
}

#options {
  border: 3px solid #000000ee;
  width: 80%;
  height: fit-content;
  margin-top: 20px;
  overflow: wrap;
  display: flex;
  flex-direction: column;
  align-content: center;
  align-items: center;
  border-radius: 5px;
  background-color: white;
  margin-bottom: 20px;
}

#details {
  border: 3px solid #000000ee;
  width: 80%;
  height: fit-content;
  margin-top: 20px;
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
