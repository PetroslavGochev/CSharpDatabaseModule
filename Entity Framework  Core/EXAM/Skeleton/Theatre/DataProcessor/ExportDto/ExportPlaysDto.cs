using System.Xml.Serialization;

namespace Theatre.DataProcessor.ExportDto
{
    [XmlType("Play")]
    public class ExportPlaysDto
    {
        [XmlAttribute]
        public string Title { get; set; }
        [XmlAttribute]
        public string Duration { get; set; }
        [XmlAttribute]
        public string Rating { get; set; }
        [XmlAttribute]
        public string Genre { get; set; }
        [XmlArray("Actors")]
        public ExportActorsDto[] Actors { get; set; }
    }
}
