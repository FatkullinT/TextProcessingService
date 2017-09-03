using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using TextProcessingService.Domain;
using TextProcessingService.Domain.Models;
using TextProcessingService.Web.Models;
using TextProcessingService.Web.Utils;

namespace TextProcessingService.Web.Controllers
{
    public class HomeController : Controller
    {
        private ITextProcessor _textProcessor;

        public HomeController(ITextProcessor textProcessor)
        {
            _textProcessor = textProcessor;
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Обработка текстового файла
        /// </summary>
        /// <param name="textFile"></param>
        /// <returns></returns>
        public ActionResult Process(HttpPostedFileBase textFile)
        {
            string text = string.Empty;
            if (textFile != null && textFile.ContentLength > 0)
            {
                using (var reader = new StreamReader(textFile.InputStream))
                {
                    text = reader.ReadToEnd();
                }
            }
            var persons = _textProcessor.FindPersons(text);
            var model = new ProcessedTextModel()
            {
                Text = HighlightingUtil.SetHighlighting(text, persons),
                Persons = persons
            };
            return View("Index", model);
        }
    }
}