using System.Collections.Generic;
using TextProcessingService.Domain.Models;

namespace TextProcessingService.Domain
{
    public interface ITextProcessor
    {
        List<Person> FindPersons(string text);
    }
}