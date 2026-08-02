import { createRouter, createWebHistory } from "vue-router";
import HomeView from "@/views/Home.vue";
import Main from "../views/Main.vue";
import Inventory from "../views/Inventory.vue";
import InventoryControl from "@/views/InventoryControl.vue";
import test from "@/components/test.vue";
import Account from "../views/Account.vue";
import UserListings from "@/views/UserListings.vue";
import Listings from "@/views/Listings.vue";
import ListingsControl from "@/views/ListingsControl.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      redirect: "/app/account",
    },
    {
      path: "/app",
      component: Main,
      children: [
        {
          path: "",
          redirect: "/app/account",
        },
        {
          path: "listings",
          components: {
            default: Listings,
            control: ListingsControl,
          },
        },
        {
          path: "my-listings",
          components: {
            default: UserListings,
            control: ListingsControl,
          },
        },
        {
          path: "inventory",
          components: {
            default: Inventory,
            control: InventoryControl,
          },
        },
        {
          path: "account",
          component: Account,
        },
      ],
    },
    {
      path: "/:pathMatch(.*)*",
      redirect: "/app/account",
    },
  ],
});

export default router;
