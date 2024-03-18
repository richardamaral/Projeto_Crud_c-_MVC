using Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Framework.Model
{
    public class DataTableModel 
    {
        // Paginação DataTable
        public int start { get; set; }
        public int length { get; set; }
        public int recordsTotal { get; set; }
        public int RegCount { get; set; }
        private string _descricaoFiltro;
        public string DescricaoFiltro
        {
            get
            {
                string search = HttpContext.Current == null ? string.Empty : HttpContext.Current.Request["search[value]"];
                return !string.IsNullOrEmpty(search) ? search : _descricaoFiltro;
            }
            set { _descricaoFiltro = value; }
        }
        private string _filtroTexto;
        public string FiltroTexto
        {
            get
            {
                string search = HttpContext.Current == null ? string.Empty : HttpContext.Current.Request["search[value]"];
                return !string.IsNullOrEmpty(search) ? search : _filtroTexto;
            }
            set { _filtroTexto = value; }
        }
        public int OrderColumn
        {
            get { return HttpContext.Current == null ? 0 : HttpContext.Current.Request["order[0][column]"].ToInt(); }
            set { value = 0; }
        }
        public string OrderDir
        {
            get { return HttpContext.Current == null ? string.Empty : HttpContext.Current.Request["order[0][dir]"]; }
            set { value = null; }
        }

        private int _pageNumber;
        public int PageNumber
        {
            get
            {
                double start = HttpContext.Current == null ? 0 : HttpContext.Current.Request["start"].ToDouble();
                double length = PageSize.ToDouble();
                return (_pageNumber > 0 ? _pageNumber : (Math.Ceiling(start / length).ToInt()) + 1);
            }
            set
            {
                _pageNumber = value;
            }
        }

        private int _pageSize;
        public int PageSize
        {
            get
            {
                return (_pageSize > 0 ? _pageSize : HttpContext.Current == null ? 0 : HttpContext.Current.Request["length"].ToInt());
            }
            set
            {
                _pageSize = value;
            }
        }


    }
}
