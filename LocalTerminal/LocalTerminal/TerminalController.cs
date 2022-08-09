using System;
using System.Collections.Generic;
using System.IO;
using ModelLibrary.Models.API.Requests;

namespace LocalTerminal
{
    public class TerminalController
    {

        private readonly Client _client = new Client();

        public void InputCommand()
        {

            while (true)
            {
                var input = Console.ReadLine()?.ToLower();

                if (input == null || input == "exit") return;
                
                if (input == "generatekeys") GenerateKeys();

            }
            
        }

        private int InputNumber(int? max = null)
        {
            var input = Console.ReadLine();
            var i = 0;
            while (!int.TryParse(input, out i) || !ValidInt(i, max))
            {
                Console.WriteLine("Invalid number!");
                input = Console.ReadLine();
            }

            return i;
        }

        private bool ValidInt(int i, int? max)
        {
            if (!max.HasValue) return true;
            if (i <= 0) return false;
            if (i > max) return false;
            return true;
        }
        
        private void GenerateKeys()
        {
            Console.WriteLine("Mode:");
            Console.WriteLine("1.) for duration");
            Console.WriteLine("2.) for due date");

            int i = InputNumber(2);
            
            if (i == 1) GenerateKeysForDuration();
            if (i == 2) GenerateKeysForDueDate();

        }

        private void GenerateKeysForDuration()
        {
            Console.WriteLine("Amount:");
            int amount = InputNumber();
            
            Console.WriteLine("Duration:");
            int duration = InputNumber();

            GenerateKeysPayload payload = new GenerateKeysPayload();
            payload.Amount = amount;
            payload.Type = 0;
            payload.Duration = duration;

            var keys = _client.GenerateKeys(payload);
            
            SaveKeys(keys);

            Console.WriteLine("Done!");
        }

        private void GenerateKeysForDueDate()
        {
            
        }

        private void SaveKeys(IEnumerable<string> keys)
        {
            File.WriteAllLines("keys.csv", keys);
        }
        
    }
}