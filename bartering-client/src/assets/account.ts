import { ref } from "vue";
import { api } from "@/assets/axiosConfig.ts";

const basePath = "/auth";

export const userData = ref<any>(null);


const getCookie = (name: string) => {
  const match = document.cookie.match(new RegExp("(^| )" + name + "=([^;]+)"));
  return match ? match[2] : null;
};

export async function refresh() {
  // skip if no userData cookie
  update_user_data();
  if(userData.value === null) {return null;}

  try {
    const response = await api.post(`${basePath}/refresh`, {});
    if (response.status === 200) {
      update_user_data();
      console.log("refresh success")
    }
  } catch (error) {
    console.log("Refresh Error");
  }
}

export async function user_register(username: string, password: string) {
  const post_data = {
    username: username,
    password: password,
  };
  try {
    const response = await api.post(`${basePath}/register`, post_data);
    if (response.status === 201) {
      console.log("Account Created");
    }
    update_user_data();
  } catch (error) {
    console.error("Registration Error");
  }
}

export async function user_login(username: string, password: string) {
  const post_data = {
    username: username,
    password: password,
  };
  try {
    const response = await api.post(`${basePath}/login`, post_data);
    if (response.status === 200) {
      console.log("Login Successful");
      update_user_data();
    }
  } catch (error) {
    console.error("Login Error");
  }
}

export async function user_logout() {
  try {
    const response = await api.post(`${basePath}/logout`, {});
    if (response.status === 200) {
      update_user_data();
    }
  } catch (error) {
    console.log("Logout Error");
  }
}

export function update_user_data() {
  const cookie = getCookie("userData")
  userData.value = (cookie ? JSON.parse(decodeURIComponent(cookie)) : null)
}

update_user_data();
