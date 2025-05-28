using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Класс для работы с врачами-сотрудниками фонда
    /// </summary>
    internal class DoctorsOnAgreementClass
    {
        public static DataTable dtDoctorsForComboBoxList;
        public static DataTable dtDoctorsList;
        public static DataTable dtDoctorDataList;
        public static DataTable dtDoctorDataForPrint;

        /// <summary>
        /// Получение списка врачей для выпадающего списка по ID должности
        /// </summary>
        /// <param name="idPost"></param>
        public static void GetDoctrosForComboBoxList(string idPost)
        {
            try
            {
                string whereClause = string.IsNullOrEmpty(idPost) ? "" : $"WHERE idPost = '{idPost}'";
                DBConnection.myCommand.CommandText = $@"SELECT ID, CONCAT_WS(' ', surname, name, IFNULL(middleName, '')) AS fullName FROM doctors_on_agreement 
                                                        {whereClause}";
                dtDoctorsForComboBoxList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtDoctorsForComboBoxList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение списка врачей
        /// </summary>
        /// <param name="querySearch"></param>
        /// <param name="orderByValue"></param>
        public static void GetDoctorsList(string querySearch, string orderByValue)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                string whereClause = string.IsNullOrEmpty(querySearch) ? null :
                    $@"AND (doctors_on_agreement.surname LIKE @searchQuery OR doctors_on_agreement.name LIKE @searchQuery OR doctors_on_agreement.middleName LIKE @searchQuery 
                        OR doctors_on_agreement.phoneNumber LIKE @searchQuery OR doctors_on_agreement.email LIKE @searchQuery
                        OR CONCAT_WS(' ', doctors_on_agreement.surname, doctors_on_agreement.name, IFNULL(doctors_on_agreement.middleName, '')) LIKE @searchQuery)";
                string orderBy = string.IsNullOrEmpty(orderByValue) ? null : $"ORDER BY {orderByValue}";
                DBConnection.myCommand.CommandText = $@"SELECT doctors_on_agreement.ID, doctors_on_agreement.surname, doctors_on_agreement.name, IFNULL(doctors_on_agreement.middleName, '-') as 'middleName',
                                            doctors_on_agreement.phoneNumber, doctors_on_agreement.email, doctor_posts.postName, medical_facility.medicalFacilityName 
                                        FROM doctors_on_agreement, medical_facility, doctor_posts
                                        WHERE medical_facility.ID = doctors_on_agreement.idMedicalFacility AND doctor_posts.ID = doctors_on_agreement.idPost {whereClause}
                                        {orderBy}";
                if (whereClause != null)
                {
                    string wildcardSearch = querySearch + "%";
                    DBConnection.myCommand.Parameters.AddWithValue("@searchQuery", wildcardSearch);
                }
                dtDoctorsList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtDoctorsList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение данных о враче по ID
        /// </summary>
        /// <param name="idDoctor"></param>
        public static void GetDoctorData(string idDoctor)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT doctors_on_agreement.ID, doctors_on_agreement.surname, doctors_on_agreement.name, IFNULL(doctors_on_agreement.middleName, '-') as 'middleName',
                                            doctors_on_agreement.phoneNumber, doctors_on_agreement.email, doctors_on_agreement.idPost, doctors_on_agreement.idMedicalFacility
                                        FROM doctors_on_agreement
                                        WHERE doctors_on_agreement.ID = '{idDoctor}'";
                dtDoctorDataList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtDoctorDataList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение данных о враче для печати
        /// </summary>
        /// <param name="idDoctor"></param>
        public static void GetDoctorDataForPrint(string idDoctor)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT doctors_on_agreement.surname, doctors_on_agreement.name, doctors_on_agreement.middleName,
                                            doctors_on_agreement.phoneNumber, doctors_on_agreement.email, doctor_posts.postName, medical_facility.medicalFacilityName, medical_facility.address 
                                        FROM doctors_on_agreement, doctor_posts, medical_facility
                                        WHERE medical_facility.ID = doctors_on_agreement.idMedicalFacility AND doctor_posts.ID = doctors_on_agreement.idPost
                                        AND doctors_on_agreement.ID = '{idDoctor}'";
                dtDoctorDataForPrint = new DataTable();
                DBConnection.myDataAdapter.Fill(dtDoctorDataForPrint);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение количества врачей-сотрудников фонда
        /// </summary>
        /// <returns></returns>
        public static string GetCountAllDoctors()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM doctors_on_agreement";
                Object result = DBConnection.myCommand.ExecuteScalar();
                if (result != null)
                    return result.ToString();
                else
                    return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Добавление нового врача-сотрудника фонда
        /// </summary>
        /// <param name="surname"></param>
        /// <param name="name"></param>
        /// <param name="middleName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="email"></param>
        /// <param name="idPost"></param>
        /// <param name="idMedicalFacility"></param>
        /// <returns></returns>
        public static bool AddDoctor(string surname, string name, string middleName, string phoneNumber, string email, string idPost, string idMedicalFacility)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $"INSERT INTO doctors_on_agreement VALUES (null, @surname, @name, @middleName, @phoneNumber, @email, '{idPost}', '{idMedicalFacility}')";
                DBConnection.myCommand.Parameters.AddWithValue("@surname", surname);
                DBConnection.myCommand.Parameters.AddWithValue("@name", name);
                DBConnection.myCommand.Parameters.AddWithValue("@middleName", middleName);
                DBConnection.myCommand.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                DBConnection.myCommand.Parameters.AddWithValue("@email", email);
                if (DBConnection.myCommand.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Обновление данных о враче-сотруднике фонда
        /// </summary>
        /// <param name="idDoctor"></param>
        /// <param name="surname"></param>
        /// <param name="name"></param>
        /// <param name="middleName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="email"></param>
        /// <param name="idPost"></param>
        /// <param name="idMedicalFacility"></param>
        /// <returns></returns>
        public static bool UpdateDoctor(string idDoctor, string surname, string name, string middleName, string phoneNumber, string email, string idPost, string idMedicalFacility)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"UPDATE doctors_on_agreement SET surname = @surname, name = @name, middleName = @middleName, 
                                                            phoneNumber = @phoneNumber, email = @email, idPost = '{idPost}', idMedicalFacility = '{idMedicalFacility}'
                                                        WHERE ID = '{idDoctor}'";
                DBConnection.myCommand.Parameters.AddWithValue("@surname", surname);
                DBConnection.myCommand.Parameters.AddWithValue("@name", name);
                DBConnection.myCommand.Parameters.AddWithValue("@middleName", middleName);
                DBConnection.myCommand.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                DBConnection.myCommand.Parameters.AddWithValue("@email", email);
                if (DBConnection.myCommand.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при обновлении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Удаление врача-сотрудника
        /// </summary>
        /// <param name="idDoctor"></param>
        /// <returns></returns>
        public static bool DeleteDoctor(string idDoctor)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"DELETE FROM doctors_on_agreement WHERE ID = '{idDoctor}'";
                if (DBConnection.myCommand.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1451)
                {
                    MessageBox.Show($"Запись не может быть удалена, так как она используется в других таблицах.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else
                {
                    MessageBox.Show($"Произошла ошибка при удалении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
