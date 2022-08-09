using System;

namespace WebAPI.Misc
{
    public static class KeyGenerator
    {

        public static string Generate()
        {
            return Guid.NewGuid().ToString();
        }
        
    }
}