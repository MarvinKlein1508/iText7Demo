using iText7Demo.Printing;
using System;
using System.Threading.Tasks;

namespace iText7Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var tmp = await PrintDemoTemplate.CreateAsync("Hello world");
            // Save it where ever you want
            tmp.Print("F:");
            
            // Publish this project to C:\Progam Files and start it from there
        }
    }
}
