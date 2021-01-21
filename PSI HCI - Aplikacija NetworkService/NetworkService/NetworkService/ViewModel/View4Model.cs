using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.ViewModel
{
    public class View4Model : BindableBase, IDataErrorInfo
    {

        public View4Model()
        {

        }
        public string this[string columnName] => throw new NotImplementedException();

        public string Error => throw new NotImplementedException();
    
        
    }
}
