using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class ChildrensClass
    {
        public static DataTable dtChildrensList = new DataTable();

        public static bool GetChildrenList(string idRegion)
        {
            try
            {
                string whereClause = string.IsNullOrEmpty(idRegion)
                    ? "childrens.idStatus = '1'"
                    : $"childrens.idRegion = '{idRegion}' AND childrens.idStatus = '1'";

                DBConnection.myCommand.CommandText = $@"SELECT childrens.ID, childrens.numOfQuestionnaire, childrens.urlOfQuestionnaire, 
                        CONCAT (childrens.surname, ' ', childrens.name) AS 'fullName', childrens.middleName, childrens.birthday,
                        (SELECT dateAdded
                         FROM childrens_description
                         WHERE childrens_description.idChild = childrens.ID 
                         ORDER BY childrens_description.ID DESC 
                         LIMIT 1) AS dateDescriptionAdded,
                        (SELECT description
                         FROM childrens_description
                         WHERE childrens_description.idChild = childrens.ID 
                         ORDER BY childrens_description.ID DESC 
                         LIMIT 1) AS description,
                        childrens.dateAdded,
                        (SELECT childphoto.filePath 
                         FROM childphoto 
                         WHERE childphoto.idChild = childrens.ID 
                         ORDER BY childphoto.ID DESC 
                         LIMIT 1) AS latestPhotoPath,
                        childrens.isAlert
                    FROM 
                        childrens
                    WHERE
                        {whereClause}
                    ORDER BY 
                        childrens.isAlert DESC,
                        childrens.ID DESC";
                dtChildrensList.Clear();
                DBConnection.myDataAdapter.Fill(dtChildrensList);
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
