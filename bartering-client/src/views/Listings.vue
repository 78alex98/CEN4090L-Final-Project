<script setup lang="ts">
import { inventoryData, fetchInventory, addItem } from "@/assets/inventory.ts";
import { listings, fetchListings } from "@/assets/listings.ts";
import { refresh, userData } from "@/assets/account.ts";
import { onMounted, ref } from "vue";
import listingItem from "@/components/listingItem.vue";

// refresh();
onMounted(() => {
  fetchInventory();
  fetchListings();
});
</script>

<template>
  <div class="inventory-list">
    <listingItem v-for="listing in listings" :key="listing.id" :listing="listing" />
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
  gap: 15px;

  overflow: auto;
}
</style>

