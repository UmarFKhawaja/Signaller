using System;
using System.Collections.Generic;

namespace Signaller.Apps.ApiApp.Models
{
    public class User
    {
        public virtual string ID { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<Post> Posts { get; } = new List<Post>();
    }
}
