import requests


class GenerateKeysPayload:
    amount: int
    type: int
    duration: int
    year: int
    month: int
    day: int




class Api:
    url = "http://localhost:5000/local"

    def create_admin(self, username: str, password: str):
        response = requests.post(f"{self.url}/user/admin", json={'username': username, 'password': password})
        if response.status_code != 200:
            print(response.content)
        return response.status_code

    def generate_keys(self, amount: int, duration: int):
        payload = GenerateKeysPayload()
        payload.type = 0
        payload.amount = amount
        payload.duration = duration

        response = requests.post(f"{self.url}/product/generate", payload)
        if response.status_code != 200:
            print(response.content)
        return response.json()

    def generate_keys_date(self, amount: int, year: int, month: int, day: int):
        payload = GenerateKeysPayload()
        payload.type = 1
        payload.amount = amount
        payload.year = year
        payload.month = month
        payload.day = day

        response = requests.post(f"{self.url}/product/generate", payload)
        if response.status_code != 200:
            print(response.content)
        return response.json()


api = Api()
