using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Framework.Model
{
    public class ClienteModel : DataTableModel
    {
        public int ClienteId { get; set; }
        public int Idade { get; set; }
        public int UsuarioClienteId { get; set; }

        public string CaminhoImagem { get; set; }

        public string Caminho_Imagem { get; set; }

        public string Cliente { get; set; }
        public string Usuario { get; set; }

        public string Nome_Usuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }

        public string Senha { get; set; }
        public string Celular { get; set; }
        public string sDataCadastro { get; set; }

        public bool Ativo { get; set; }
        public DateTime? DataCadastro { get; set; }
    }
}
