//const API_URL = 'http://localhost:5099';
const API_URL = 'https://koninkrijk-api.test.example';

const getApiKey = () => sessionStorage.getItem('apiKey');

const request = async (url, options = {}) => {
    const apiKey = getApiKey();
    const headers = {
        'Content-Type': 'application/json',
        ...options.headers,
        ...(apiKey && { 'Authorization': apiKey }),
    };

    const response = await fetch(`${API_URL}${url}`, {
        ...options,
        headers,
    });

    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || 'Something went wrong');
    }

    return response.json();
};

class ApiService {
    // retrieve apikey if set
    getCurrentApiKey(){
        let apiKey = sessionStorage.getItem("apiKey");
        if(apiKey){
            return apiKey;
        } else {
            return null;
        }
    }

    // player endpoints
    async registerPlayer(nickname) {
        return await request('/players/register', {
            method: 'POST',
            body: JSON.stringify({ nickname }),
        });
    }

    async login(){
        return await request('/players/login', { method: 'POST' });
    }

    async getPlayerInfo() {
        return await request('/players/info');
    }

    async getScoreboard(){
        return await request('/players/scoreboard');
    }

    // province endpoints
    async getProvinces() {
        return await request('/provinces');
    }

    async getProvince(provinceName) {
        return await request(`/provinces/${provinceName}`);
    }

    async getProvinceTries(provinceName) {
        return await request(`/provinces/${provinceName}/tries`);
    }

    async refreshProvinceTries(provinceName) {
        return await request(`/provinces/${provinceName}/refresh`);
    }

    // log endpoints
    async getLogs() {
        return await request('/logs');
    }

    async getLogsPaginated(page) {
        return await request(`/logs?page=${page}`);
      }

    // action endpoints
    async captureProvince(id, guess) {
        return await request(`/actions/capture/${id}/${guess}`, {
            method: 'POST',
        });
    }
}

export default new ApiService();
