import os
from cmd import Cmd
from api import api
from pathlib import Path


class MyPrompt(Cmd):
    scriptPath: str = os.path.dirname(os.path.abspath(__file__))
    rootPath: str = Path(scriptPath).parent.absolute()

    def do_exit(self, _input):
        print("Bye")
        return True

    def do_createAdmin(self, _input):
        username = input("username: ")
        password = input("password: ")
        print(api.create_admin(username, password))

    def do_generateKeys(self, _input):
        amount = input("amount: ")
        duration = input("duration: ")
        result = api.generate_keys(amount, duration)
        f = open("keys.csv", "w")
        for s in result:
            f.write(s + os.linesep)
        f.close()

    def do_generateKeysDate(self, _input):
        amount = input("amount: ")
        year = input("year: ")
        month = input("month: ")
        day = input("day: ")
        result = api.generate_keys_date(amount, year, month, day)
        f = open("keys", "w")
        for s in result:
            f.write(s + os.linesep)
        f.close()



MyPrompt().cmdloop()
