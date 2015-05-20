using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core
{
    public class DataTable
        {
            

            List<DataColumn> _Columns = new List<DataColumn>();

            public List<DataColumn> Columns
            {
                get { return _Columns; }
                set { _Columns = value; }
            }

            List<DataRow> _Rows = new List<DataRow>();

            public List<DataRow> Rows
            {
                get { return _Rows; }
                set { _Rows = value; }
            }
        }
}
