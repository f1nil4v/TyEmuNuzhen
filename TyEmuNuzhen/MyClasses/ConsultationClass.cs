using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class ConsultationClass
    {
        public static DataTable dtConsultationsChildList;

        public static void GetConsultationsChildList(string idChild)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT consultation.ID, doctor_posts.postName, doctors_on_agreement.surname, doctors_on_agreement.name, doctors_on_agreement.middleName,
                    consultation.filePath, consultation.dateConsultation
                    FROM consultation, doctor_posts, doctors_on_agreement
                    WHERE consultation.idDoctor = doctors_on_agreement.ID AND doctor_posts.ID = doctors_on_agreement.idPost AND consultation.idChild = '{idChild}'
                    ORDER BY consultation.ID DESC";
                dtConsultationsChildList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtConsultationsChildList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static bool AddChildrenConsultation(string idDoctor, string idChild, string filePath, string dateConsiltastion)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $"INSERT INTO consultation VALUES (null, '{idDoctor}', '{idChild}', @path, '{dateConsiltastion}')";
                DBConnection.myCommand.Parameters.AddWithValue("@path", filePath);
                if (DBConnection.myCommand.ExecuteNonQuery() < 0)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static string GetLastIdConsultation()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT MAX(ID) FROM consultation";
                Object result = DBConnection.myCommand.ExecuteScalar();
                if (result != null)
                {
                    return result.ToString();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
