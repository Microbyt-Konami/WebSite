using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Application.Services;

public class RandomServices : IRandomServices
{
    private readonly Random _random = new Random();

    public int NetInt(int max) => _random.Next(max);
}
