using pyramid.Models;
using System;

namespace pyramid.Helpers
{
    public static class UserExtensions
    {
        public static User StringToUser(this string item)
        {
            var values = item.Split(',');
            
            if(values.Length == 2)
            {
                try
                {
                    return new User
                    {
                        ID = int.Parse(values[0]),
                        Sponsor = int.Parse(values[1])
                    };
                }
                catch(Exception e)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
