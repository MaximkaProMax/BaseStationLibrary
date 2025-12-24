using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseStationLibrary.Models
{
    /// Результаты расчёта по району
    public class CalculationResult
    {
        public double R0 { get; set; }    // Радиус зоны обслуживания
        public double R { get; set; }     // Радиус покрытия БС
        public double L { get; set; }     // Число сот
        public double C { get; set; }     // Кластер
        public double N { get; set; }     // Количество БС
    }
}