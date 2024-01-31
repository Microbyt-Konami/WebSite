using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Tools.Fortunes
{
    internal class App
    {
        private readonly IPostalesServices _services;

        public App(IPostalesServices services)
        {
            _services = services;
        }

        public async Task RunAsync(string[] args)
        {
            var postales = await  _services.GetFelicitacionAsync(new GetFelicitacionIn { Anyo = 2023, Intervals = new IntegerIntervals() });

            Console.WriteLine("Hello World");
        }
    }
}
