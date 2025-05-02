using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace TyEmuNuzhen.MyClasses
{
    internal class ActualProgramClass
    {
        public static string GetIDLastActualProgramChildren(string idChild)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"SELECT MAX(ID) FROM actual_program WHERE idChild = '{idChild}' AND idCurator = '{CuratorClass.idCurator}'";
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

        public static bool AddChildrenActualProgram(string idChild, string idProgramType, string idCurator)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"INSERT INTO actual_program
                    VALUES (null, '{idChild}', '{idCurator}', '{idProgramType}')";
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
    }
}
