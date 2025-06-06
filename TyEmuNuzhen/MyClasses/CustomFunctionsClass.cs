using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Проектные методы, вынесенные в отдельный класс
    /// </summary>
    internal class CustomFunctionsClass
    {
        /// <summary>
        /// Метод для вычисления возраста на основе даты рождения.
        /// </summary>
        /// <param name="birthDate"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Метод для получения склонения слова "год" в зависимости от числа.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Метод для получения склонения слова "месяц" в зависимости от числа.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Метод для получения склонения слова "день" в зависимости от числа.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Проверка корректности введённого email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string email)
        {
            char[] parseEmail = email.ToCharArray();
            int lastInd = parseEmail.Length - 1;
            int k = 0;
            int d = 0;
            int k1 = 0;
            int d1 = 0;
            if (parseEmail[0] == '@' || parseEmail[0] == '.' || parseEmail[lastInd] == '@' || parseEmail[lastInd] == '.')
                return false;
            foreach (char symbol in parseEmail)
            {
                if (symbol == '@')
                    k++; // 1
                if (symbol == '.')
                    d++; //1
                if (d > k)
                    return false;
                if (d > 0 && k > 0)
                    if (k == d)
                        break;
            }

            foreach (char symbol in parseEmail)
            {
                if (symbol == '.')
                    d1++;
                if (symbol == '@')
                    k1++;
                if (d1 > 1)
                    return false;
                if (k1 > 1)
                    return false;
            }

            if (k == 1 && d == 1)
                return true;
            else
            {
                MessageBox.Show("Некорректно введён email", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }

        /// <summary>
        /// Проверка корректности введённого пароля.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsValidPassword(string password)
        {
            char[] parsePassword = password.ToCharArray();
            if (password.Length < 8)
            {
                MessageBox.Show("Длина пароля должна быть больше 8-ти символов", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            foreach (char symbol in parsePassword)
            {
                if (symbol == '.' || symbol == ',' || symbol == '!' || symbol == '?' || symbol == '-' || symbol == ':' || symbol == ';')
                {
                    MessageBox.Show("Недопустимые знаки в пароле!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (symbol == 32)
                {
                    MessageBox.Show("Пробел в пароле!");
                    return false;
                }

                if ((symbol < 'A' || symbol > 'z') && (symbol < '0' || symbol > '9'))
                {
                    MessageBox.Show("Кириллица недопустима в пароле!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Проверка на уникальность email в базе данных.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="id"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static bool CheckSameEmail(string email, string id = null, string table = null)
        {
            try
            {
                string[] tables = { "curators", "doctors_on_agreement", "orphanages", "directors", "volunteers", "nannies" };
                foreach (string tableName in tables)
                {
                    DBConnection.myCommand.Parameters.Clear();
                    string whereClause = "email = @email";
                    if (id != null && table != null && tableName == table)
                    {
                        whereClause += " AND ID <> @id";
                        DBConnection.myCommand.Parameters.AddWithValue("@id", id);
                    }

                    DBConnection.myCommand.CommandText = $"SELECT COUNT(ID) FROM {tableName} WHERE {whereClause}";
                    DBConnection.myCommand.Parameters.AddWithValue("@email", email);

                    if (Convert.ToInt32(DBConnection.myCommand.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show($"Email '{email}' уже используется в системе. Пожалуйста, используйте другой email.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Проверка на уникальность номера телефона в базе данных.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="id"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static bool CheckSamePhoneNumber(string phoneNumber, string id = null, string table = null)
        {
            try
            {
                string[] tables = { "curators", "doctors_on_agreement", "directors", "volunteers", "nannies" };
                foreach (string tableName in tables)
                {
                    DBConnection.myCommand.Parameters.Clear();
                    string whereClause = "phoneNumber = @phoneNumber";
                    if (id != null && table != null && tableName == table)
                    {
                        whereClause += " AND ID <> @id";
                        DBConnection.myCommand.Parameters.AddWithValue("@id", id);
                    }

                    DBConnection.myCommand.CommandText = $"SELECT COUNT(ID) FROM {tableName} WHERE {whereClause}";
                    DBConnection.myCommand.Parameters.AddWithValue("@phoneNumber", phoneNumber);

                    if (Convert.ToInt32(DBConnection.myCommand.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show($"Номер телефона '{phoneNumber}' уже используется в системе. Пожалуйста, используйте другой номер телефона.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
