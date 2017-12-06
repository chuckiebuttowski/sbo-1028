using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace SBOClient.Helpers
{
    public static class ModelValidator
    { 
        public static string ValidateModel<T>(T obj)
        {
            string errMsg = "";
            try
            {
                ValidationContext ctx = new ValidationContext(obj);
                List<ValidationResult> results = new List<ValidationResult>();
                bool valid = Validator.TryValidateObject(obj, ctx, results, true);
                if (!valid)
                {
                    StringBuilder strbldr = new StringBuilder("Model Properties Invalid:");
                    strbldr.AppendLine();
                    foreach (ValidationResult vr in results)
                    {
                        strbldr.AppendFormat("Property:{0}", vr.MemberNames.First());
                        strbldr.AppendLine();
                        strbldr.AppendFormat("Error Message:{0}", vr.ErrorMessage);
                        strbldr.AppendLine();
                    }

                    errMsg = strbldr.ToString();
                }

                return errMsg;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}