using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
public class JsonBinderAttribute : CustomModelBinderAttribute
{

    public override IModelBinder GetBinder()
    {

        return new JsonModelBinder();

    }



    public class JsonModelBinder : IModelBinder
    {

        public object BindModel(

            ControllerContext controllerContext,

            ModelBindingContext bindingContext)
        {

            try
            {

                var json = controllerContext.HttpContext.Request

                           .Params[bindingContext.ModelName];



                if (String.IsNullOrWhiteSpace(json))

                    return null;



                // Swap this out with whichever Json deserializer you prefer.
                var model = JsonConvert.DeserializeObject(json, bindingContext.ModelType, new IsoDateTimeConverter());

                return model;
            }

            catch (Exception ex)
            {
                ex = ex;
                bindingContext.ModelState.AddModelError("CustomError", ex.Message);
                return null;

            }

        }

    }

}

