using System;

namespace Signaller.Apps.ApiApp.Models
{
    public class Query
    {
        public virtual User GetUser(string name) => new User
        {
            ID = Guid.NewGuid().ToString(),
            Name = name
        };

        public virtual Post GetPost(string id) => new Post
        {
            ID = Guid.NewGuid().ToString(),
            Subject = "C# in Depth",
            Text = "Yada Yada Yada",
            User = new User
            {
                ID = Guid.NewGuid().ToString(),
                Name = "UFK"
            }
        };
    }
}