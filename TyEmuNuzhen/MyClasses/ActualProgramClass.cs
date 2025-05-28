using Microsoft.WindowsAPICodePack.Dialogs;
using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Xml.Linq;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Класс для работы с актуальными программами
    /// </summary>
    internal class ActualProgramClass
    {
        public static DataTable dtActualProgramDataForPrint;
        public static DataTable dtProgramsHistoryList;

        /// <summary>
        /// Получение списка пройденных программ по детям
        /// </summary>
        /// <param name="idChild"></param>
        /// <param name="idProgramType"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        public static void GetProgramsHistoryList(string idChild, string idProgramType, string dateBegin, string dateEnd)
        {
            try
            {
                string whereClause = "";
                if (!String.IsNullOrEmpty(idProgramType))
                    whereClause += $" AND actual_program.idProgramType = '{idProgramType}'";
                if (!String.IsNullOrEmpty(dateBegin) && !String.IsNullOrEmpty(dateEnd))
                    whereClause += $" AND actual_program.dateBegin >= '{dateBegin}' AND actual_program.dateEnd <= '{dateEnd}'";
                DBConnection.myCommand.CommandText = $@"SELECT actual_program.ID, CONCAT_WS(' ', curators.surname, curators.name, IFNULL(curators.middleName, '')) as FIOCurator,  
                                                            program_type.programType, CONCAT(DATE_FORMAT(actual_program.dateBegin, '%d.%m.%Y'), ' - ', DATE_FORMAT(actual_program.dateEnd, '%d.%m.%Y')) as period, 
                                                            actual_program.filePath
                                                        FROM actual_program, curators, program_type
                                                        WHERE actual_program.idCurator = curators.ID AND actual_program.idProgramType = program_type.ID AND actual_program.idChild = '{idChild}'
                                                        {whereClause}";
                dtProgramsHistoryList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtProgramsHistoryList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение ID последней актуальной программы по конкретному ребёнку
        /// </summary>
        /// <param name="idChild"></param>
        /// <returns></returns>
        public static string GetIDLastActualProgramChildren(string idChild)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"SELECT MAX(ID) FROM actual_program WHERE idChild = '{idChild}'";
                Object resultID = DBConnection.myCommand.ExecuteScalar();
                if (resultID != null)
                {
                    return resultID.ToString();
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

        /// <summary>
        /// Получение наименование актуальной программы
        /// </summary>
        /// <param name="idActualProgram"></param>
        /// <returns></returns>
        public static string GetLastActualProgramChildren(string idActualProgram)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT program_type.programType FROM actual_program, program_type 
                                                        WHERE program_type.ID = actual_program.idProgramType AND actual_program.ID = '{idActualProgram}'";
                Object resultID = DBConnection.myCommand.ExecuteScalar();
                if (resultID != null)
                {
                    return resultID.ToString();
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

        /// <summary>
        /// Получение данных актуальной программы для печати
        /// </summary>
        /// <param name="idActualProgram"></param>
        public static void GetActualProgramDataToBeOnTimeForPrint(string idActualProgram)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT actual_program.dateBegin, actual_program.dateEnd, hospitalization.ID AS hospitalizationID,
                                                            CONCAT_WS(' ', curators.surname, curators.name, IFNULL(curators.middleName, '')) AS curatorFullName, curators.phoneNumber, curators.email, 
                                                            medical_facility.medicalFacilityName, medical_facility.address, hospitalization.dateHospitalization, hospitalization.dateDischarge, hospitalization.totalCost,
                                                            (SELECT COUNT(*) FROM nannies_on_program WHERE idActualProgram = actual_program.ID) AS countNanniesOnProgram
                                                        FROM actual_program, curators, medical_facility, hospitalization, nannies_on_program
                                                        WHERE actual_program.ID = hospitalization.idActualProgram AND actual_program.idCurator = curators.ID AND hospitalization.idMedicalFacility = medical_facility.ID AND
                                                            actual_program.ID = nannies_on_program.idActualProgram AND actual_program.ID = '{idActualProgram}'";
                dtActualProgramDataForPrint = new DataTable();
                DBConnection.myDataAdapter.Fill(dtActualProgramDataForPrint);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Проверка наличия программ у ребёнка
        /// </summary>
        /// <param name="idChild"></param>
        /// <returns></returns>
        public static bool HaveProgramsChild(string idChild)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM actual_program WHERE idChild = '{idChild}'";
                if (Convert.ToInt32(DBConnection.myCommand.ExecuteScalar()) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Добавление актуальной программы для ребёнка
        /// </summary>
        /// <param name="idChild"></param>
        /// <param name="idProgramType"></param>
        /// <param name="idCurator"></param>
        /// <returns></returns>
        public static bool AddChildrenActualProgram(string idChild, string idProgramType, string idCurator)
        {
            try
            {
                string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
                DBConnection.myCommand.CommandText = $@"INSERT INTO actual_program
                    VALUES (null, '{idChild}', '{idCurator}', '{idProgramType}', '{dateNow}', null, null, 1)";
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
        /// Обновление статуса, добавления даты окончания и добавление пути к файлу сформированного отчёта прохождения программы 
        /// </summary>
        /// <param name="idProgram"></param>
        /// <param name="dateEnd"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool UpdateChildrenActualProgramEndProgram(string idProgram, string dateEnd, string filePath)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"UPDATE actual_program
                    SET dateEnd = '{dateEnd}', filePath = '{filePath}',status = 0 WHERE ID = '{idProgram}'";
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
    }
}
