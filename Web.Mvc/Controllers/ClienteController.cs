using Framework.Business;
using Framework.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Mvc.Controllers
{
    public class ClienteController : BaseController
    {
        ClienteManager clienteManager = new ClienteManager();
        // GET: Cliente


        public ActionResult ListaRichard(ClienteModel model)
        {

            var data = clienteManager.GetCliente(model);


            return View(data);
        }



        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Cliente", new { Msg = "Logout bem-sucedido" });
        }

        public ActionResult Login(string Msg)
        {
            ViewBag.Retorno = Msg;

            return View();
        }


        [HttpPost]
        public JsonResult JsLogin(string email, string senha)
        {

            var usuarioClienteId = clienteManager.Login(email, senha);

            if (usuarioClienteId > 0)
            {

                Session["UsuarioClienteId"] = usuarioClienteId;

                return Json(new { success = true, mensagem = "Login bem-sucedido" });
            }
            else
            {
                return Json(new { success = false, mensagem = "Credenciais inválidas" });
            }
        }

        public ActionResult Index()
        {


            int usuarioClienteId = _UsuaioLogado().UsuarioClienteId;
            ViewBag.UsuarioClienteId = usuarioClienteId;

            var clienteDataCarregado = clienteManager.GetClienteByUsuarioClienteId(usuarioClienteId);


            return View(clienteDataCarregado);
        }






        [HttpPost]
        public JsonResult JsGetUsuarioCliente(ClienteModel model)
        {
            var data = clienteManager.GetUsuarioCliente(model);

            return Json(new { data = data, recordsTotal = model.PageSize, recordsFiltered = data?.FirstOrDefault()?.RegCount }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult JsSetUsuarioCliente(ClienteModel model)
        {
            var data = clienteManager.SetUsuarioCliente(model);

            return Json(new { mensagem = clienteManager.retorno }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult JsEditCliente(ClienteModel model)
        {
            var data = clienteManager.EditCliente(model);

            return Json(new { mensagem = clienteManager.retorno }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult ListaRichardAjax(ClienteModel model)
        {

            return View();
        }


        public JsonResult JsDeleteCliente(ClienteModel model)
        {

            var data = clienteManager.DeleteCliente(model);

            return Json(new { mensagem = clienteManager.retorno }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult JsDeleteUsuarioCliente(ClienteModel model)
        {

            var data = clienteManager.DeleteUsuarioCliente(model);



            return Json(new { mensagem = clienteManager.retorno }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Lista(ClienteModel model)
        {

            var data = clienteManager.GetCliente(model);


            return View(data);
        }
        public ActionResult ListaAjax(ClienteModel model)
        {

            return View();
        }

        [HttpPost]
        public JsonResult JsGetCliente(ClienteModel model)
        {
            var data = clienteManager.GetCliente(model);

            return Json(new { data = data, recordsTotal = model.PageSize, recordsFiltered = data?.FirstOrDefault()?.RegCount }, JsonRequestBehavior.AllowGet);
        }







        [HttpPost]
        public JsonResult JsSetCliente(ClienteModel model, HttpPostedFileBase imagem)
        {
            try
            {
                if (imagem != null && imagem.ContentLength > 0)
                {
                    var nomeImagem = Guid.NewGuid().ToString() + Path.GetExtension(imagem.FileName);
                    var caminhoCompleto = Path.Combine(Server.MapPath("~/Imagens/"), nomeImagem);

                    imagem.SaveAs(caminhoCompleto);

                    model.CaminhoImagem = "~/Imagens/" + nomeImagem;
                }

                var data = clienteManager.SetCliente(model);

                return Json(new { mensagem = clienteManager.retorno });
            }
            catch (Exception ex)
            {
                return Json(new { erro = "Erro ao salvar a imagem: " + ex.Message });
            }
        }
    }
}




