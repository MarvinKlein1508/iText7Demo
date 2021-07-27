#define CREATE_PREVIEW_HTML
using iText.Html2pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iText7Demo.Printing
{
    public class PrintDemoTemplate
    {
        private readonly string _layoutFile = "DemoTemplate.html";
        private string _template = string.Empty;

        public const string LAYOUT_PATH = "Printing\\Templates";
        public string Text { get; }
        private PrintDemoTemplate(string text)
        {
            Text = text;
        }

        public static async Task<PrintDemoTemplate> CreateAsync(string text)
        {
            var myClass = new PrintDemoTemplate(text);
            await myClass.InitializeAsync();
            return myClass;
        }

        private async Task InitializeAsync()
        {
            string layoutPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LAYOUT_PATH, _layoutFile);
            _template = await File.ReadAllTextAsync(layoutPath);
            _template = await SetTemplateVariablesAsync();
        }

        private string GenerateDisplayCss()
        {
            StringBuilder cssBuilder = new StringBuilder();
            cssBuilder.AppendLine("<style>");
            cssBuilder.AppendLine("\t.my-text2{");
            cssBuilder.AppendLine("\t\tcolor: orange;");
            cssBuilder.AppendLine("\t}");
            cssBuilder.AppendLine("</style>");
            return cssBuilder.ToString();
        }
        private Task<string> SetTemplateVariablesAsync()
        {
            _template = _template
                .Replace("{{ DISPLAY_CSS }}", GenerateDisplayCss())
                .Replace("{{ TEXT }}", Text)
                ;

            return Task.FromResult(_template);
        }
        public string Print(string savePath)
        {
            string filename = $"DEMO.pdf";

#if CREATE_PREVIEW_HTML
            File.WriteAllText(Path.Combine(savePath, $"DEMO.html"), _template);
#endif
            byte[] byteArray = Encoding.UTF8.GetBytes(_template);
            MemoryStream stream = new MemoryStream(byteArray);
            ConverterProperties converterProperties = new ConverterProperties()
                .SetBaseUri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LAYOUT_PATH));

            string fullPath = Path.Combine(savePath, filename);
            

            HtmlConverter.ConvertToPdf(stream, new FileStream(fullPath, FileMode.Create, FileAccess.ReadWrite, FileShare.None), converterProperties);

            return fullPath;
        }

    }
}
