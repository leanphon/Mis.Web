using Apps.Model.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MIS.Web.Model
{
    public class Utility
    {
        public static string GetModelStateErrors(ModelStateDictionary ModelState)
        {
            var msg = "";

            foreach (var key in ModelState.Keys)
            {
                var errors = ModelState[key].Errors;
                
                foreach (var e in errors)
                {
                    msg += e.ErrorMessage + ";";
                }
            }


            return msg;
        }

    }
}
