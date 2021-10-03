using System;

namespace ModJsonGenerator
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ModJsonInfoAttribute : Attribute
    {
        private readonly int _id;
        private readonly string _description;
        private readonly string[] _searchtags;
        private readonly string[] _requirements;
        private readonly string _changelog;
        private readonly string _embedcolor;

        public ModJsonInfoAttribute(int id, string description, string[] searchtags, string[] requirements, string changelog, string embedcolor = "#000000")
        {
            _id = id;
            _description = description;
            _searchtags = searchtags;
            _requirements = requirements;
            _changelog = changelog;
            _embedcolor = embedcolor;
        }
    }
    
}