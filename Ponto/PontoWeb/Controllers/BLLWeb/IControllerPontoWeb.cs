using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PontoWeb.Controllers.BLLWeb
{
    public abstract class IControllerPontoWeb<T> : Controller where T : Modelo.ModeloBase, new()
    {
        public abstract ActionResult Grid();
        public abstract ActionResult Consultar(int id);
        public abstract ActionResult Cadastrar();
        public abstract ActionResult Alterar(int id);
        public abstract ActionResult Cadastrar(T obj);
        public abstract ActionResult Alterar(T obj);
        public abstract ActionResult Excluir(int id);
        protected abstract ActionResult Salvar(T obj);
        protected abstract ActionResult GetPagina(int id);
        protected abstract void ValidarForm(T obj);

        protected void MostrarErro(Exception ex, string msgCustomizado = null)
        {
            var id = Employer.PlataformaLog.LogError.WriteLog(ex);
            var msg = string.IsNullOrWhiteSpace(msgCustomizado) ? ex.Message : msgCustomizado;
            ModelState.AddModelError("CustomError", string.Format("{0} - {1}", id, msg));
        }

        public void ValidaRetornoBLLSalvar(Dictionary<string, string> erros)
        {
            List<string> CustomError = new List<string>();
            foreach (KeyValuePair<string, string> erro in erros)
            {
                if (ModelState.ContainsKey(erro.Key))
                {
                    ModelState[erro.Key].Errors.Add(erro.Value);
                }
                else
                {
                    CustomError.Add(erro.Value);
                }
            }

            if (CustomError.Count() > 0)
            {
                throw new Exception(String.Join(", ", CustomError));
            }
        }
        
    }
}
