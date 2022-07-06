using System;

namespace MuizClient.Models
{
    public class TitleAttribute : Attribute
    {
        public TitleAttribute(string title)
        {
            Title = title;
        }

        public string Title { get; }
    }
}