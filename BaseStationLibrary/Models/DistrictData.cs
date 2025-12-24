using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseStationLibrary.Models
{
    /// Входные данные для расчёта базовых станций по району.
    public class DistrictData
    {
        public string Name { get; set; }                      // Название района
        public double Area { get; set; }                      // Площадь района (км²)
        public double CoverageArea { get; set; }              // Площадь покрытия одной БС (км²)
        public double[] Radii { get; set; }                   // Радиусы трёх БС (км)
        public double HandoverAverage { get; set; }           // Средний хэндовер
        public double HandoverLimit { get; set; }             // Допустимый хэндовер
        public double BuildCoefficient { get; set; }          // Коэффициент застройки (K)
    }
}