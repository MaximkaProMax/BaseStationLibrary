using System;
using System.Linq;
using BaseStationLibrary.Models;

namespace BaseStationLibrary
{
    /// Класс для расчёта количества базовых станций
    public class BaseStationCalculator
    {
        /// Радиус зоны обслуживания района R0 = sqrt(S / π)
        public double CalculateServiceRadius(double area)
        {
            if (area <= 0)
                throw new ArgumentException("Площадь района должна быть > 0");

            return Math.Sqrt(area / Math.PI);
        }

        /// Радиус покрытия БС R = sqrt(S / π)
        public double CalculateBaseStationRadius(double coverageArea)
        {
            if (coverageArea <= 0)
                throw new ArgumentException("Площадь покрытия БС должна быть > 0");

            return Math.Sqrt(coverageArea / Math.PI);
        }

        /// Число сот L = K * (R0 / R)^2
        public double CalculateL(double r0, double r, double k)
        {
            if (r <= 0)
                throw new ArgumentException("Радиус БС должен быть > 0");

            return k * Math.Pow(r0 / r, 2);
        }

        /// Кластер C = D1*5/2 + D2*3/2 + D3*1/2, где D = 2R
        public double CalculateCluster(double[] radii)
        {
            if (radii == null || radii.Length != 3)
                throw new ArgumentException("Необходимо указать три радиуса.");

            if (radii.Any(r => r <= 0))
                throw new ArgumentException("Радиусы должны быть > 0");

            var sorted = radii.OrderByDescending(r => r).ToArray();

            double d1 = 2 * sorted[0];
            double d2 = 2 * sorted[1];
            double d3 = 2 * sorted[2];

            return Math.Pow(d1, 2.5) + Math.Pow(d2, 1.5) + Math.Pow(d3, 0.5);
        }

        /// Количество БС n = L / C, с учётом хэндовера
        public double CalculateN(double l, double c, double handover, double limit)
        {
            if (c <= 0)
                throw new ArgumentException("Кластер C должен быть > 0");

            double n = l / c;

            if (handover < limit)
                n *= 1.4;

            return n;
        }

        /// Полный расчёт по одному району
        public CalculationResult CalculateForDistrict(DistrictData data)
        {
            double r0 = CalculateServiceRadius(data.Area);
            double r = CalculateBaseStationRadius(data.CoverageArea);
            double l = CalculateL(r0, r, data.BuildCoefficient);
            double c = CalculateCluster(data.Radii);
            double n = CalculateN(l, c, data.HandoverAverage, data.HandoverLimit);

            return new CalculationResult
            {
                R0 = r0,
                R = r,
                L = l,
                C = c,
                N = n
            };
        }

        /// Суммарное количество БС по нескольким районам
        public double CalculateTotal(params DistrictData[] districts)
        {
            return districts.Sum(d => CalculateForDistrict(d).N);
        }
    }
}