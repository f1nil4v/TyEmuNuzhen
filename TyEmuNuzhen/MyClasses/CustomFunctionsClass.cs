using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyEmuNuzhen.MyClasses
{
    internal class CustomFunctionsClass
    {
        public static string CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;

            int years = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-years)) years--;

            if (years < 1)
            {

                int totalMonths = (today.Year - birthDate.Year) * 12 + today.Month - birthDate.Month;

                if (today.Day < birthDate.Day)
                {
                    totalMonths--;
                }

                if (totalMonths <= 0)
                {
                    int days = (today - birthDate).Days;
                    return days + " " + GetDayDeclension(days);
                }

                return totalMonths + " " + GetMonthDeclension(totalMonths);
            }
            else
            {
                return years + " " + GetYearDeclension(years);
            }
        }

        private static string GetYearDeclension(int number)
        {
            int lastDigit = number % 10;
            int lastTwoDigits = number % 100;

            if (lastDigit == 1 && lastTwoDigits != 11)
            {
                return "год";
            }
            else if ((lastDigit >= 2 && lastDigit <= 4) &&
                     !(lastTwoDigits >= 12 && lastTwoDigits <= 14))
            {
                return "года";
            }
            else
            {
                return "лет";
            }
        }

        private static string GetMonthDeclension(int number)
        {
            int lastDigit = number % 10;
            int lastTwoDigits = number % 100;

            if (lastDigit == 1 && lastTwoDigits != 11)
            {
                return "месяц";
            }
            else if ((lastDigit >= 2 && lastDigit <= 4) &&
                     !(lastTwoDigits >= 12 && lastTwoDigits <= 14))
            {
                return "месяца";
            }
            else
            {
                return "месяцев";
            }
        }

        private static string GetDayDeclension(int number)
        {
            int lastDigit = number % 10;
            int lastTwoDigits = number % 100;

            if (lastDigit == 1 && lastTwoDigits != 11)
            {
                return "день";
            }
            else if ((lastDigit >= 2 && lastDigit <= 4) &&
                     !(lastTwoDigits >= 12 && lastTwoDigits <= 14))
            {
                return "дня";
            }
            else
            {
                return "дней";
            }
        }
    }
}
