using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BLL_N.JobManager.Hangfire.Job;
using Modelo.EntityFramework.MonitorPontofopag;
using Hangfire.Storage;
using Newtonsoft.Json;
using Hangfire.Annotations;

namespace TesteProgramacao
{
    public class TesteHangfire
    {
        public void Simular(int idJob)
        {
            try
            {
                var job = new Modelo.EntityFramework.MonitorPontofopag.Job();
                using (var db = new MONITOR_PONTOFOPAGEntities())
                {
                    job = db.Job.Where(w => w.Id == idJob).FirstOrDefault();
                }

                InvocationData invocationData = FromJson<InvocationData>(job.InvocationData);
                
                var dados = invocationData.Deserialize();

                Type magicType = Type.GetType(invocationData.Type);
                ConstructorInfo magicConstructor = magicType.GetConstructor(Type.EmptyTypes);
                object magicClassObject = magicConstructor.Invoke(new object[] { });

                MethodInfo magicMethod = magicType.GetMethod(invocationData.Method);
                object magicValue = magicMethod.Invoke(magicClassObject, dados.Args.ToArray());
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public static string InvokeStringMethod
        (string typeName, string methodName, string stringParam)
        {
            // Get the Type for the class
            Type calledType = Type.GetType(typeName);

            // Invoke the method itself. The string returned by the method winds up in s.
            // Note that stringParam is passed via the last parameter of InvokeMember,
            // as an array of Objects.
            String s = (String)calledType.InvokeMember(
                            methodName,
                            BindingFlags.InvokeMethod | BindingFlags.Public |
                                BindingFlags.Static,
                            null,
                            null,
                            new Object[] { stringParam });

            // Return the string that was returned by the called method.
            return s;
        }

        public static string ToJson(object value)
        {
            return value != null
                ? JsonConvert.SerializeObject(value, Formatting.None, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                })
                : null;
        }

        public static T FromJson<T>(string value)
        {
            return value != null
                ? JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                })
                : default(T);
        }

        public static object FromJson(string value, [NotNull] Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return value != null
                ? JsonConvert.DeserializeObject(value, type, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                })
                : null;
        }
    }
}
