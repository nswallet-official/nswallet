using System.Collections.Generic;
using System.Xml.Serialization;

namespace NSWallet.Shared
{
    [XmlRoot(ElementName = "name")]
    public class Name
    {
        [XmlElement(ElementName = "en")]
        public string En { get; set; }
        [XmlElement(ElementName = "ru")]
        public string Ru { get; set; }
        [XmlElement(ElementName = "search")]
        public string Search { get; set; }
    }

    [XmlRoot(ElementName = "icongroup")]
    public class Icongroup
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "name")]
        public Name Name { get; set; }
    }

    [XmlRoot(ElementName = "icon")]
    public class Icon
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "name")]
        public Name Name { get; set; }
        [XmlElement(ElementName = "type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "group")]
        public string Group { get; set; }
    }

    [XmlRoot(ElementName = "iconset")]
    public class IconSet
    {
        [XmlElement(ElementName = "app_version")]
        public string App_version { get; set; }
        [XmlElement(ElementName = "icongroup")]
        public List<Icongroup> Icongroup { get; set; }
        [XmlElement(ElementName = "icon")]
        public List<Icon> Icon { get; set; }
    }
}