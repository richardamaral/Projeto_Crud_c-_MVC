using Framework.DataAcess;
using Framework.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Framework.Business
{
    public class ClienteManager
    {
        public int retornoId { get; set; }
        public string retorno { get; set; }
        public List<ClienteModel> GetCliente(ClienteModel model)
        {
            return GenericDA.Get<List<ClienteModel>>(model, "GetCliente");

        }

        public List<ClienteModel> GetUsuarioCliente(ClienteModel model)
        {
            return GenericDA.Get<List<ClienteModel>>(model, "GetUsuarioCliente").Select(m =>
            {
                m.sDataCadastro = string.Format("{0:dd/MM/yyyy}", m.DataCadastro);
                return m;
            }).ToList();
        }



        public int SetUsuarioCliente(ClienteModel model)
        {
            try
            {
                retornoId = GenericDA.Set<int>(model, "SetUsuarioCliente");
                retorno = "Usuário cadastrado com sucesso!";
                return retornoId;
            }
            catch (Exception ex)
            {
                retorno = ex.Message;
                return 0;
            }
        }

        public int SetCliente(ClienteModel model)
        {
            try
            {
                if (!ExisteEmail(model))
                {
                    retornoId = GenericDA.Set<int>(model, "SetCliente");
                    retorno = "Dados salvos com sucesso!";
                    return retornoId;
                }
                else
                {
                    retorno = "E-mail já existe!";
                    return 0;
                }
            }
            catch (Exception ex)
            {
                retorno = ex.Message;
                return 0;
            }
        }

        private bool ExisteEmail(ClienteModel model)
        {
            if (model.ClienteId == 0)
            {
                model.DescricaoFiltro = model.Email;
                var data = GetCliente(model);
                if (data.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }


        public int DeleteUsuarioCliente(ClienteModel model)
        {
            try
            {
                retornoId = GenericDA.Set<int>(model, "DeleteUsuarioCliente");
                retorno = "Dados excluído com sucesso!";
                return retornoId;
            }
            catch (Exception ex)
            {
                retorno = ex.Message;
                return 0;
            }
        }

        public int DeleteCliente(ClienteModel model)
        {
            try
            {
                retornoId = GenericDA.Set<int>(model, "DeleteCliente");
                retorno = "Dados excluído com sucesso!";
                return retornoId;
            }
            catch (Exception ex)
            {
                retorno = ex.Message;
                return 0;
            }
        }



        public int Login(string email, string senha)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(senha))
            {
                var usuarioCliente = GetUsuarioCliente(new ClienteModel
                {
                    Email = email,
                    Senha = senha
                }).FirstOrDefault();

                if (usuarioCliente != null)
                    return usuarioCliente.UsuarioClienteId;
                else 
                    return 0;
            }
            else
                return 0; // Se as credenciais não coincidirem
        }
        private ClienteModel GetUsuarioClienteByEmail2(string email)
        {
            return GenericDA.Get<List<ClienteModel>>(new ClienteModel { Email = email }, "GetUsuarioClienteByEmail").FirstOrDefault();
        }
       //private ClienteModel GetUsuarioClienteByEmail(string email)
       //{
       //    ClienteModel usuarioCliente = null;
       //
       //    string connectionString = "Data Source=26.237.127.87;Initial Catalog=DbArthur;Persist Security Info=True;User ID=sa_teste;Password=Nova@1233";
       //
       //    using (SqlConnection connection = new SqlConnection(connectionString))
       //    {
       //        connection.Open();
       //
       //        using (SqlCommand command = new SqlCommand("GetUsuarioClienteByEmail", connection))
       //        {
       //            command.CommandType = CommandType.StoredProcedure;
       //            command.Parameters.AddWithValue("@Email", email);
       //
       //            using (SqlDataReader reader = command.ExecuteReader())
       //            {
       //                if (reader.Read())
       //                {
       //                    usuarioCliente = new ClienteModel
       //                    {
       //                        UsuarioClienteId = Convert.ToInt32(reader["UsuarioClienteId"]),
       //                        Nome = reader["Nome"].ToString(),
       //                        Email = reader["Email"].ToString(),
       //                        Senha = reader["Senha"].ToString(),
       //                        Ativo = Convert.ToBoolean(reader["Ativo"]),
       //                        DataCadastro = Convert.ToDateTime(reader["DataCadastro"]),
       //                        ClienteId = Convert.ToInt32(reader["ClienteId"])
       //                    };
       //                }
       //            }
       //        }
       //    }
       //
       //    return usuarioCliente;
       //}



       public List<ClienteModel> GetClienteByUsuarioClienteId(int usuarioClienteId)
       {
            return GenericDA.Get<List<ClienteModel>>(new ClienteModel { UsuarioClienteId = usuarioClienteId }, "GetClienteByUsuarioClienteId");
       }
       //
       //
       //public List<ClienteModel> GetClienteByUsuarioClienteId2(int usuarioClienteId)
       //{
       //
       //    string connectionString = "Data Source=26.237.127.87;Initial Catalog=DbArthur;Persist Security Info=True;User ID=sa_teste;Password=Nova@1233";
       //    string storedProcedureName = "GetClienteByUsuarioClienteId";
       //
       //    List<ClienteModel> clienteData = new List<ClienteModel>();
       //
       //    using (SqlConnection connection = new SqlConnection(connectionString))
       //    {
       //        using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
       //        {
       //            command.CommandType = CommandType.StoredProcedure;
       //            command.Parameters.AddWithValue("@UsuarioClienteId", usuarioClienteId);
       //
       //            connection.Open();
       //
       //            using (SqlDataReader reader = command.ExecuteReader())
       //            {
       //                while (reader.Read())
       //                {
       //                    ClienteModel cliente = new ClienteModel
       //                    {
       //                        ClienteId = Convert.ToInt32(reader["ClienteId"]),
       //                        Nome = reader["Nome"].ToString(),
       //                        Email = reader["Email"].ToString(),
       //                        Celular = reader["Celular"].ToString(),
       //                        Idade = Convert.ToInt32(reader["Idade"])
       //                    };
       //
       //                    clienteData.Add(cliente);
       //                }
       //            }
       //        }
       //
       //
       //    }
       //
       //    return clienteData;
       //}





        public int EditCliente(ClienteModel model)
        {
            try
            {
                retornoId = GenericDA.Set<int>(model, "EditCliente");
                retorno = "Dados editados com sucesso!";
                return retornoId;
            }
            catch (Exception ex)
            {
                retorno = ex.Message;
                return 0;
            }
        }
    }
}




