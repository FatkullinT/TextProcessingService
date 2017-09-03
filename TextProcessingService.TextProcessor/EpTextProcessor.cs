using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EP;
using EP.Semantix;
using TextProcessingService.Domain;
using TextProcessingService.Domain.Models;

namespace TextProcessingService.TextProcessor
{
    /// <summary>
    /// Адаптор текстового процессора
    /// </summary>
    public class EpTextProcessor : ITextProcessor
    {
        public EpTextProcessor()
        {
            ProcessorService.Initialize(false);
            (new PersonInitializer()).Initialize();
        }

        /// <summary>
        /// Поиск персон
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public List<Person> FindPersons(string text)
        {
            using (Processor processor = new Processor())
            {
                var result = processor.Process(new SourceOfAnalysis(text));
                var persons = new List<Person>();
                foreach (var personEntity in result.Entities.Where(e=>string.Equals(e.TypeName, "PERSON", StringComparison.InvariantCultureIgnoreCase)).Select(e=>(PersonReferent)e))
                {
                    var person = new Person()
                    {
                        FullName = personEntity.ToString(),
                        Positions = MapPositions(personEntity.Occurrence),
                        Properties = MapProperties(
                            personEntity.Slots
                                .Where(s => string.Equals(s.TypeName, "ATTRIBUTE",
                                                StringComparison.InvariantCultureIgnoreCase) && s.Value is PersonPropertyReferent)
                                .Select(s => (PersonPropertyReferent) s.Value)
                                .Where(r => string.Equals(r.TypeName, "PERSONPROPERTY",
                                    StringComparison.InvariantCultureIgnoreCase))
                                .ToList())
                    };
                    persons.Add(person);
                }
                return persons;
            }
        }


        /// <summary>
        /// Получение свойств персон (должностей)
        /// </summary>
        /// <param name="personPropertyEntities"></param>
        /// <returns></returns>
        private List<PersonProperty> MapProperties(List<PersonPropertyReferent> personPropertyEntities)
        {
            var personProperties = new List<PersonProperty>();
            foreach (var personPropertyEntity in personPropertyEntities)
            {
                var personProperty = new PersonProperty()
                {
                    Name = personPropertyEntity.Name,
                    Positions = MapPositions(personPropertyEntity.Occurrence)
                };
                personProperties.Add(personProperty);
            }
            return personProperties;
        }

        /// <summary>
        /// Получение данных о раположении объекта в тесте
        /// </summary>
        /// <param name="personEntityOccurrence"></param>
        /// <returns></returns>
        private List<Position> MapPositions(List<TextAnnotation> personEntityOccurrence)
        {
            var positions = new List<Position>();
            foreach (var textAnnotation in personEntityOccurrence)
            {
                positions.Add(new Position()
                {
                    FirstChar = textAnnotation.BeginChar,
                    LastChar = textAnnotation.EndChar
                });
            }
            return positions;
        }
    }
}
