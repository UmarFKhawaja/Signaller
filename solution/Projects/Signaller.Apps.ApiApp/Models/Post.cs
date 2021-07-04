using System;
using System.Collections.Generic;

namespace Signaller.Apps.ApiApp.Models
{
    public class Post
    {
        public virtual string ID { get; set; }

        public virtual string Subject { get; set; }

        public virtual string Text { get; set; }

        public virtual User User { get; set; }
    }
}
