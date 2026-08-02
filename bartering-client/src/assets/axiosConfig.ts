import axios from "axios";

const base: string = "http://localhost:5096/api";
const refreshEndpoint: string = "/auth/refresh";

export const api = axios.create({
    baseURL: base,
    headers: {},
    withCredentials: true,
});

api.interceptors.response.use(
    response => response,
    async error => {
        const request = error.config;
        if (error.status === 401 && !request.retry) {
            request.retry = true;   // To prevent retry loops
            try {
                const response = await axios.post(`${base}${refreshEndpoint}`,
                    {},
                    {
                        headers: {},
                        withCredentials: true
                    },
                );

                return api(request);
            } catch (e) {
                console.error('Refresh Error');
                return Promise.reject(e);
            }
        }

        return Promise.reject(error);
    }
)
