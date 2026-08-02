<script setup lang="ts">
import { ref } from "vue";
import { addItem } from "@/assets/inventory.ts";

const showForm = ref<boolean>(false);
const name = ref<string>("");
const description = ref<string>("");
const fileName = ref<string>("Upload File");
let file: File | null = null;

function resetValues() {
  name.value = "";
  description.value = "";
  file = null;
  fileName.value = "Upload File";
}

function addClicked() {
  if (name.value !== "") {
    showForm.value = false;
    addItem(name.value, description.value, file);
    resetValues();
  }
}

function selectFile(event: Event) {
  const target = event.target;

  if (target.files) {
    file = target.files[0];
    fileName.value = target.files[0].name;
  }
}
</script>

<template>
  <div v-if="!showForm" id="blank" @click="showForm = true">
    <b>+</b>
    <p>Add New</p>
  </div>
  <div v-else id="form">
    <div id="input">
      <input id="name" type="text" v-model="name" placeholder="Item Name" />
      <textarea
        id="description"
        v-model="description"
        placeholder="Description"
      />
      <div id="file">
        <input
          id="file-input"
          type="file"
          accept="image/*"
          @change="selectFile"
        />
        <label id="file-label" for="file-input"> {{ fileName }} </label>
      </div>
    </div>
    <div id="control">
      <button id="add" @click="addClicked">Add</button>
      <button id="cancel" @click="showForm = false; resetValues()">Cancel</button>
    </div>
  </div>
</template>

<style scoped>
#control {
  display: flex;
  gap: 10px;
}

button {
  margin-top: 13px;
  border-radius: 5px;
  background: none;
  border: #0000002f solid 2px;
  font-weight: bold;
  width: 80px;
  height: 25px;
  cursor: pointer;
}
button#cancel:hover {
  background: #ff00002f;
}
button#add:hover {
  background: #0000ff3f;
}

#input {
  display: flex;
  gap: 10px;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}

#description {
  height: 100px;
  border: #00000020 dashed 2px;
  border-radius: 20px;
  padding: 2px;
  outline: none;
  font-size: 15px;
  text-align: center;
  width: 90%;
  caret-color: black;
  font-weight: bold;
  resize: none;
}

#name {
  border: #00000020 dashed 2px;
  border-radius: 20px;
  padding: 2px;
  outline: none;
  font-size: 15px;
  text-align: center;
  width: 90%;
  height: 25px;
  caret-color: black;
  font-weight: bold;
}

#file {
  border: #00000020 dashed 2px;
  border-radius: 20px;
  padding: 2px;
  outline: none;
  font-size: 15px;
  text-align: center;
  width: 90%;
  height: 25px;
  caret-color: black;
  font-weight: bold;
}

#file-input {
  display: none;
  width: 100%;
  height: 100%;
}

#file-label {
  border: none;
  border-radius: inherit;
  height: 100%;
  width: 100%;
  display: block;
  overflow: hidden;
  line-height: 25px;
  color: #757575;
}

#file-label:hover {
  background: #e7e7e7e8;
  color: black;
  cursor: pointer;
}

#form {
  border: 3px solid #0000005f;
  color: #000000af;
  width: 210px;
  height: 255px;
  user-select: none;
  border-radius: 10px;
  margin: 10px;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  font-size: 30px;
}

#blank {
  border: 3px dashed #0000005f;
  color: #000000af;
  width: 120px;
  height: 130px;
  user-select: none;
  border-radius: 10px;
  margin: 10px;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  font-size: 30px;
}

#blank:hover {
  background-color: #0000001f;
  border-color: black;
  color: black;
  cursor: pointer;
}
</style>
