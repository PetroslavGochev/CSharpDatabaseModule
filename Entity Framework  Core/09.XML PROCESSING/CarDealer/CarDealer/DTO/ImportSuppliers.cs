﻿using System.Xml.Serialization;

namespace CarDealer.DTO
{
    [XmlType("Supplier")]
    public class ImportSuppliers
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("isImporter")]
        public bool IsImporter { get; set; }
    }
}
