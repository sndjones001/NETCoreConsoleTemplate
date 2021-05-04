using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace testKonsolenProjekt
{
    public interface IWeatherService
    {
        Task<IReadOnlyList<int>> GetFiveDayTemperaturesAsync();
    }
}
