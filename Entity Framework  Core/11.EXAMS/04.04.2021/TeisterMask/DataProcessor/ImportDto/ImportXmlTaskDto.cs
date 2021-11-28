using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Task")]
    public class ImportXmlTaskDto
    {
        [Required]
        [MaxLength(40)]
        [MinLength(2)]
        public string Name { get; set; }
        [Required]
        public string OpenDate { get; set; }
        [Required]
        public string DueDate { get; set; }
        [Required]
        [Range(0,3)]
        public int ExecutionType { get; set; }
        [Required]
        [Range(0, 4)]
        public int LabelType { get; set; }
    }
}
