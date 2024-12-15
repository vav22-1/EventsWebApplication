import axios from 'axios';

export const refreshAccessToken = async () => {
    try {
        const refreshToken = localStorage.getItem('refreshToken');
        if (!refreshToken) {
            throw new Error('Refresh token отсутствует');
        }

        const response = await axios.post('https://localhost:7178/api/User/refresh', refreshToken, {
          headers: {
              'Content-Type': 'application/json',
            },
        
      });
        localStorage.setItem('accessToken', response.data.accessToken);
        localStorage.setItem('refreshToken', response.data.refreshToken);
        return response.data.accessToken;
    } catch (error) {
        console.error('Ошибка обновления токенов:', error);
        console.log(error.data)
        alert(123)
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('participantId');
        localStorage.removeItem('role');
        window.location.href = '/login';

        return null;
    }
};
