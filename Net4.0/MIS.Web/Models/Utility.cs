using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MIS.Web.Model
{
    public class Utility
    {
        public static string GetModelStateErrors(ModelStateDictionary ModelState)
        {
            List<string> errList = new List<string>();

            foreach (var key in ModelState.Keys)
            {
                var errors = ModelState[key].Errors;
                
                foreach (var e in errors)
                {
                    errList.Add(e.ErrorMessage);
                }
            }

            return errList.ToString();
        }

    }
}
