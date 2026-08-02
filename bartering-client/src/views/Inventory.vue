<script setup lang="ts">
import { inventoryData, fetchInventory, addItem } from "@/assets/inventory.ts";
import { refresh, userData } from "@/assets/account.ts";
import { onMounted, ref } from "vue";
import inventoryItem from "@/components/inventoryItem.vue";
import newItem from "@/components/newItem.vue";

onMounted(() => {
  fetchInventory();
});
</script>

<template>
  <div class="inventory-list">
    <inventoryItem v-for="item in inventoryData" :key="item.id" :item="item" />
    <newItem />
  </div>
</template>

<style scoped>
.inventory-list {
  display: flex;
  flex-wrap: wrap;
  padding: 15px;
  /* align-items: center; */
  justify-content: center;
  box-sizing: border-box;
  margin: 0;
  /* width: 100%; */
  /* height: 100%; */
  max-width: 100%;
  max-height: 100%;
  min-width: auto;
  min-height: auto;

  overflow: auto;
}
</style>
