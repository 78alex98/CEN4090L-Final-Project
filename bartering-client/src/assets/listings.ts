import { ref } from "vue";
import { api } from "@/assets/axiosConfig.ts";
import { userData, update_user_data } from "@/assets/account.ts";
import { selectedItem, selectedListing } from "@/assets/selections.ts";
import { fetchInventory } from "./inventory";

export const listings = ref<any>(null);
export const userListings = ref<any>(null);
export const bids = ref<any>(null);
export const userBid = ref<any>(null);

export async function makeListing(
  id: number,
  title: string,
  description: string,
  message: string,
) {
  update_user_data();
  if (userData.value === null) {
    userListings.value = null;
    return;
  }
  const post_data = {
    itemId: id,
    title: title,
    description: description,
    message: message,
  };
  try {
    const response = await api.post("listings", post_data);

    if (response.status === 201) {
      console.log("Listing Made");
      // fetchListings();
    }
  } catch (error) {
    console.log("Listing Post Error");
  }
}
export async function postBid(listingId: number, itemId: number) {
  update_user_data();
  if (userData.value === null) {
    userListings.value = null;
    return;
  }
  const post_data = {
    itemId: itemId,
  };

  try {
    const response = await api.post(
      "listings/" + listingId + "/bids",
      post_data,
    );

    if (response.status === 201) {
      console.log("Bid Posted");
      fetchBids(listingId);
    }
  } catch (error) {
    console.log("Bid Post Error");
  }
}

export async function fetchListings() {
  update_user_data();
  if (userData.value === null) {
    userListings.value = null;
    listings.value = null;
    return;
  }
  try {
    const response = await api.get("listings");

    if (response.status === 200) {
      listings.value = response.data;
      userListings.value = listings.value.filter(
        (listing: any) => listing.item.owner === userData.value.UserName,
      );
      console.log("Listings Fetched");
      // console.log(listings.value);
    }
  } catch (error) {
    console.log("Listings Fetch Error");
  }
}

export async function deleteListing(listingId: number) {
  update_user_data();
  if (userData.value === null) {
    bids.value = null;
    return;
  }
  try {
    const response = await api.delete("listings/" + listingId);

    if (response.status === 204) {
      console.log("Listing Deleted");
      selectedListing.value = null;
      fetchListings();
    }
  } catch (error) {
    console.log("Listing Deletion Error");
  }
}

export async function fetchBids(id: number) {
  update_user_data();
  if (userData.value === null) {
    bids.value = null;
    return;
  }
  try {
    const response = await api.get("listings/" + id);

    if (response.status === 200) {
      bids.value = response.data.bids;
      userBid.value = bids.value.filter(
        (bid: any) => bid.user === userData.value.UserName,
      );
      console.log("Bids Fetched");
    }
  } catch (error) {
    console.log("Bids Fetch Error");
  }
}
export async function deleteBid(listingId: number, bidId: number) {
  update_user_data();
  if (userData.value === null) {
    bids.value = null;
    return;
  }
  try {
    const response = await api.delete(
      "listings/" + listingId + "/bids/" + bidId,
    );

    if (response.status === 204) {
      console.log("Bid Deleted");
      fetchBids(listingId);
    }
  } catch (error) {
    console.log("Bid Deletion Error");
  }
}

export async function selectWinner(listingId: number, bidId: number) {
  update_user_data();
  if (userData.value === null) {
    bids.value = null;
    return;
  }
  try {
    const response = await api.post(
      "listings/" + listingId + "/bids/winningbid/" + bidId,
    );

    if (response.status === 204) {
      console.log("Winning Bid Selected");
      selectedListing.value = null;
      selectedItem.value = null;
      fetchListings();
    }
  } catch (error) {
    console.log("Winning Bid Selection Error");
  }
}
