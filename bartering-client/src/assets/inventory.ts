import { ref } from "vue";
import { api } from "@/assets/axiosConfig.ts";
import { userData, update_user_data } from "@/assets/account.ts";
import { selectedItem } from "@/assets/selections.ts";

export const inventoryData = ref<any>(null);

export async function fetchInventory() {
  update_user_data();
  if (userData.value === null) {
    inventoryData.value = null;
    return;
  }

  try {
    const response = await api.get("inventory");

    if (response.status === 200) {
      inventoryData.value = response.data;
      // console.log(inventoryData.value);
      console.log("inventory data fetch success");
    }
  } catch (error) {
    console.log("Inventory Fetch Error");
  }
}

export async function deleteItem(id: number) {
  update_user_data();
  if (userData.value === null) {
    inventoryData.value = null;
    selectedItem.value = null;
    return;
  }

  try {
    const response = await api.delete("items/"+id);

    if (response.status === 200) {
      console.log("Item Deleted");
      fetchInventory();
    }
  } catch (error) {
    console.log("Item Delete Error");
  }
}

export async function addItem(name: string, description: string, image: File) {
  // skip if no user cookie
  update_user_data();
  if (userData.value === null) {
    return null;
  }

  let base64data: string | null = null;

  if (image) {
    base64data = await convertToBase64(image).catch(() => {
      console.error("File could not be read.");
      return null; // This is redundant, but something has to be returned in this case.
    });
  }

  const post_data = {
    name: name,
    description: description,
    image: base64data,
  };

  try {
    const response = await api.post("items", post_data);

    if (response.status === 201) {
      console.log("Item Added");
      fetchInventory();
    }
  } catch (error) {
    console.log("Add Item Error");
  }
}

async function convertToBase64(file: File): Promise<string> {
  const reader = new FileReader();

  return new Promise((resolve, reject) => {
    reader.onloadend = () => {
      resolve(reader.result as string);
    };

    reader.onerror = () => {
      reject("Error reading file.");
    };

    reader.readAsDataURL(file);
  });
}
