using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TextProcessingService.Domain.Models;

namespace TextProcessingService.Web.Models
{
    public class ProcessedTextModel
    {
        public string Text { get; set; }

        public List<Person> Persons { get; set; }
    }
}