using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Syracuse.Mobitheque.Core.Models;

namespace Syracuse.Mobitheque.Core.Services.Files
{
    public class DepartmentService : FileService, IDepartmentService
    {

        private Library dataJson;

        //#region Test

        ////private const string dataFileName = "data.json";
        ////private const string V = "https://syracusepp.archimed.fr/mobidoc/data.json?_s=";

        //#endregion

        //#region Test de la Prod

        //private const string dataFileName = "data-prod.json";
        //private const string V = "https://syracusepp.archimed.fr/mobidoc/data-prod.json?_s=";

        //#endregion

        //#region Prod

        ////private const string dataFileName = "data.json";
        ////private const string V = "https://www.syracuse.cloud/mobidoc/data-prod.json?_s=";

        //#endregion

        //private const string dataUrl = V;
        //public string DataUrl
        //{
        //    get => dataUrl + DateTime.Now.Ticks.ToString();
        //}



        //public async Task<Department[]> GetDepartments()
        //{
        //    if (this.dataJson == null)
        //    {
        //        var dataText = await this.GetDataRessource(dataFileName, DataUrl);
        //        if (dataText != string.Empty)
        //            this.dataJson = JsonConvert.DeserializeObject<DataJson>(dataText);
        //    }
        //    if (this.departments == null)
        //    {
        //        if (this.libraries == null)
        //        {
        //            this.libraries = this.dataJson.Libraries;
        //        }

        //        List<string> departementCode = new List<string>();
        //        foreach (var library in this.libraries)
        //        {
        //            departementCode.Add(library.DepartmentCode);
        //        }
        //        departementCode = departementCode.Distinct().ToList();
        //        Department[] departmentsArray = this.dataJson.Departments;
        //        this.departments = Array.FindAll(departmentsArray,
        //        element => departementCode.Contains(element.Code));


        //    }
        //    return this.departments;
        //}

        public async Task<Library> GetLibraries(string url, bool refresh = false)
        {
            if (this.dataJson == null || refresh)
            {
                var dataText = await this.GetDataRessource(url);
                if (!string.IsNullOrEmpty(dataText))
                    this.dataJson = JsonConvert.DeserializeObject<Library>(dataText);
                else
                    this.dataJson = null;
            }
            return this.dataJson;
        }
    }
}
