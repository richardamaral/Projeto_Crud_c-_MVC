using Framework.Business;
using Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Web.Mvc.Controllers
{
    public class BaseController : Controller
    {


        public ClienteModel _UsuaioLogado()
        {
            var dataUsuaioLogado = new ClienteModel();
            if (Session["UsuarioClienteId"] != null)
            {
                int usuarioClienteId = (int)Session["UsuarioClienteId"];
                if (usuarioClienteId > 0)
                {
                    dataUsuaioLogado = new ClienteManager().GetUsuarioCliente(new ClienteModel
                    {
                        UsuarioClienteId = usuarioClienteId
                    }).FirstOrDefault();

                    if (dataUsuaioLogado == null)
                        Redirect();
                }
                else
                    Redirect();
            }
            else
                Redirect();

            return dataUsuaioLogado;
        }

        private void Redirect()
        {
            Response.Redirect(Url.Action("Login", "Cliente", new { Msg = "Usuario ou senha invalida!" }));
        }
    }
}
